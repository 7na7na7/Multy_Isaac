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
    public float detectRadious = 5;
    public Transform[] PlayerPoses;
    private IEnumerator corr;
    private PhotonView pv;
    public float AttackTime;
    public float minIdleTime=0.5f;
    public float maxIdleTIme = 2f;
    public float minMove;
    public float maxMove;
    public float speed;
    private Rigidbody2D rigid;
    private Enemy enemy;

    public Path path;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody2D>();
        corr = MoveCor();
        enemy = GetComponent<Enemy>();
        //플레이어들 넣어주기
        Player[] players;
        players = FindObjectsOfType<Player>();
        PlayerPoses=new Transform[players.Length];

        for (int i = 0; i < PlayerPoses.Length; i++)
            PlayerPoses[i] = players[i].transform;
            
        if (PhotonNetwork.OfflineMode)
        {
         StartCoroutine(corr);
         StartCoroutine(find());
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(corr);
               StartCoroutine(find());
            }
                
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
            if (!enemy.isFinding && enemy.canMove)
            {
                foreach (Transform tr in PlayerPoses)
                {
                    if (Vector3.Distance(transform.position, tr.position) < detectRadious)
                    {
                        StopCoroutine(corr);
                        enemy.isFinding = true;
                        enemy.targetPosition = tr; 
                        enemy.setAnim("Walk");
                        break;
                    }
                }   
            }
            if (enemy.isFinding)
            {
                if (path == null) //경로가 비었으면 
                {
                    return; //아무것도 안함
                }

                reachedEndOfPath = false;
                float distanceToWaypoint;
                while (true)
                {
                    distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                    if (distanceToWaypoint < nextWaypointDistance)
                    {
                        if (currentWaypoint + 1 < path.vectorPath.Count)
                        {
                            currentWaypoint++;
                        }
                        else
                        {
                            reachedEndOfPath = true;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
                Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                Vector3 velocity = dir * speed * speedFactor;
                rigid.velocity = velocity;
                
                enemy.setLocalX(enemy.targetPosition.transform.position.x);
            }
        }

    IEnumerator find()
    {
        while (true)
        {
            if (enemy.isFinding)
            {
                enemy.seeker.StartPath(transform.position, enemy.targetPosition.position, OnPathComplete);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
        
    IEnumerator MoveCor()
    {
        while (true)
        {
            Vector2 roamPos = GetRoamingPosition();
            
            //가려는 방향에 따라 플립
            enemy.setLocalX(roamPos.x);
            

            if (Vector2.Distance(transform.position, roamPos) < 1.5f)
            {
                enemy.setAnim("Idle");
                
                rigid.velocity=Vector2.zero;
                yield return new WaitForSeconds(Random.Range(minIdleTime,maxIdleTIme));
            }
            else
            {
                enemy.setAnim("Walk");
             
                Vector2 dir =roamPos -  (Vector2)transform.position;
                dir.Normalize();
                float reachedPositionDistance = 0.5f;
                rigid.velocity = dir * speed;
                yield return new WaitUntil(() =>Vector2.Distance( transform.position,roamPos)<reachedPositionDistance);      
            }
        }
    }
    
    public void OnDisable () {
        enemy.seeker.pathCallback -= OnPathComplete;
    }
    
   private Vector2 GetRoamingPosition()
   {
       float randomMove = Random.Range(minMove, maxMove);
       return transform.position + UtilsClass.GetRandomDir() * randomMove;
   }


   private void OnCollisionStay2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Wall") && enemy.canMove && !enemy.isFinding)
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
       if (other.gameObject.CompareTag("Wall") && enemy.canMove && !enemy.isFinding)
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
               StartCoroutine(Attack(other.transform.position.x));
           }
           else
           {
               if (PhotonNetwork.IsMasterClient)
               {
                   StartCoroutine(Attack(other.transform.position.x));
               }
           }
       }
   }

   IEnumerator Attack(float x)
   {
       StopCoroutine(corr);
       enemy.isFinding = false;
       enemy.canMove = false;
       rigid.velocity=Vector2.zero;

       enemy.setLocalX(x);
       enemy.setAnim("Attack");
       
       yield return new WaitForSeconds(AttackTime);

       enemy.canMove = true;
       
       foreach (Transform tr in PlayerPoses)
       {
           if (Vector3.Distance(transform.position, tr.position) < detectRadious)
           {
               enemy.isFinding = true;
               enemy.targetPosition = tr;
               enemy.setAnim("Walk");
               break;
           }
       }   
   }
}
