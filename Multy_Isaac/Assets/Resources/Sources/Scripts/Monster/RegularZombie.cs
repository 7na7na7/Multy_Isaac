using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.Tweening;
using Pathfinding;
using Photon.Pun;
using Random = UnityEngine.Random;

public class RegularZombie : MonoBehaviour
{
    private IEnumerator corr;
    private PhotonView pv;
    public float AttackTime;
    public float minIdleTime=0.5f;
    public float maxIdleTIme = 2f;
    public float minMove;
    public float maxMove;
    public float speed;
    private float localX;
    private Rigidbody2D rigid;
    private Vector3 startingPosition;
    private Enemy enemy;
    Animator anim;


//    private void Start()
//    {
//        pv = GetComponent<PhotonView>();
//        rigid = GetComponent<Rigidbody2D>();
//        startingPosition = transform.position;
//        anim = GetComponent<Animator>();
//        corr = MoveCor();
//        localX = transform.localScale.x*-1;
//        enemy = GetComponent<Enemy>();
//        
//        if (PhotonNetwork.OfflineMode)
//        {
//         StartCoroutine(corr);
//        }
//        else
//        {
//            if (PhotonNetwork.IsMasterClient)
//            {
//                //StartCoroutine(velocitySync());
//            }
//                
//        }
//    }

    public Transform targetPosition;
    private Seeker seeker;
    
    public Path path;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;
    
    public void Start () {
        seeker = GetComponent<Seeker>();

        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        while (true)
        {
            targetPosition = GameObject.FindWithTag("Player").transform;
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            yield return new WaitForSeconds(1f);
        }
    }
    public void OnPathComplete (Path p) {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }
    
        public void Update () 
        { 
            if (path == null) //경로가 비었으면 
            {
                return;  //아무것도 안함
            }

            // 다음 경유지로 전환할 수 있을 만큼 현재 경유지에 근접한 경우 루프를 체크인합니다.
// 많은 경유지가 서로 가까울 수 있고 도달할 수 있기 때문에 루프에서 이 작업을 수행합니다.
// 여러 개가 같은 프레임에 있습니다.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
       
        // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
        transform.position += velocity * Time.deltaTime;
    }
        
        
    IEnumerator MoveCor()
    {
        while (true)
        {
            rigid.velocity = Vector2.zero;
            Vector2 roamPos = GetRoamingPosition();
            
            //가려는 방향에 따라 플립
            if(PhotonNetwork.OfflineMode)
                localScaleRPC(roamPos.x);
            else
                pv.RPC("localScaleRPC",RpcTarget.All,roamPos.x);
            
            
            if (Vector2.Distance(transform.position, roamPos) < 1f)
            {
                rigid.velocity = Vector2.zero;
                
                if(PhotonNetwork.OfflineMode)
                    animRPC("RegularZombie_Idle");
                else
                    pv.RPC("animRPC",RpcTarget.All,"RegularZombie_Idle");
                
                yield return new WaitForSeconds(Random.Range(minIdleTime,maxIdleTIme));
            }
            else
            {
                if(PhotonNetwork.OfflineMode)
                    animRPC("RegularZombie_Walk");
                else
                    pv.RPC("animRPC",RpcTarget.All,"RegularZombie_Walk");
                
                Vector2 dir = (roamPos- (Vector2)transform.position).normalized;
                rigid.velocity = dir * speed;
            
                float reachedPositionDistance = 0.2f;
                yield return new WaitUntil(() =>Vector2.Distance(transform.position,roamPos)<reachedPositionDistance);      
            }

//            isFinding = true;
//            startPos = new Vector2Int((int)transform.position.x,(int)transform.position.y);
//            Vector2 roamPos = GetRoamingPosition();
//            targetPos=new Vector2Int((int)roamPos.x,(int)roamPos.y);
//            PathFinding();
//            yield return new WaitUntil(() => isFinding ==false);   
        }
    }
    
    public void OnDisable () {
        seeker.pathCallback -= OnPathComplete;
    }
    
   private Vector2 GetRoamingPosition()
   {
       float randomMove = Random.Range(minMove, maxMove);
       return startingPosition + UtilsClass.GetRandomDir() * randomMove;
   }


   private void OnCollisionStay2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Wall") && enemy.canMove)
       {
           if(PhotonNetwork.OfflineMode)
           {
               StopCoroutine(corr);
               StartCoroutine(corr);
           }
           else
           {
               if(PhotonNetwork.IsMasterClient)
               {
                   StopCoroutine(corr);
                   StartCoroutine(corr);
               }
           }
       }
   }
   private void OnCollisionEnter2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Wall") && enemy.canMove)
       {
           if(PhotonNetwork.OfflineMode)
           {
               StopCoroutine(corr);
               StartCoroutine(corr);
           }
           else
           {
               if(PhotonNetwork.IsMasterClient)
               {
                   StopCoroutine(corr);
                   StartCoroutine(corr);
               }
           }
       }
   }
   private void OnTriggerEnter2D(Collider2D other)
   {
       if (other.CompareTag("Player") && enemy.canMove)
       {
           if(PhotonNetwork.OfflineMode)
           {
               StopCoroutine(corr);
               rigid.velocity=Vector2.zero;
               StartCoroutine(Attack(other.transform.position.x));
           }
           else
           {
               if (PhotonNetwork.IsMasterClient)
               {
                   StopCoroutine(corr);
                   rigid.velocity = Vector2.zero;
                   StartCoroutine(Attack(other.transform.position.x));
               }
           }
       }
   }

   IEnumerator Attack(float x)
   {
     
           if(PhotonNetwork.OfflineMode)
               localScaleRPC(x);
           else
               pv.RPC("localScaleRPC",RpcTarget.All,x);
           
       
       if(PhotonNetwork.OfflineMode)
           animRPC("RegularZombie_Attack");
       else
           pv.RPC("animRPC",RpcTarget.All,"RegularZombie_Attack");
       
       yield return new WaitForSeconds(AttackTime);
       StartCoroutine(corr);
   }


   [PunRPC]
   void animRPC(string animName)
   {
       try
       {
           anim.Play(animName);
       }
       catch (Exception e)
       { }
   }

   [PunRPC]
   void localScaleRPC(float x)
   {
       if (x > transform.position.x) //오른쪽에있으면
       {
           transform.localScale=new Vector3(localX*-1,transform.localScale.y,transform.localScale.z);
       }
       else
       {
           transform.localScale=new Vector3(localX,transform.localScale.y,transform.localScale.z);
       }
   }
}
