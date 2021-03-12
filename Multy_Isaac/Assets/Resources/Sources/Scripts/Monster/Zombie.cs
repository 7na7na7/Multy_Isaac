using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.Tweening;
using Pathfinding;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
    private bool isPan1 = false; //나무판인가?
    private bool isPan2 = false;//철판인가?
    public int pan1SpeedPercent = 40;
    public int pan2SpeedPercent = 40;
    public GameObject Poison;
    private Animator anim;
    private IEnumerator poisonCor;
    private bool canPoison = false;
    private float poisonTime=0;
    public int zombieIndex = 1;
    private TimeManager time;
    public float nightDetecctRad;
    public float detectRad= 5;
    public List<Player> Players=new List<Player>();
    public List<Transform> PlayerTrs=new List<Transform>();
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
    private bool isMaster= false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody2D>();
        corr = MoveCor();
        enemy = GetComponent<Enemy>();
        //플레이어들 넣어주기
        Player[] players;
        players = FindObjectsOfType<Player>();

        foreach (Player p in players)
        {
            Players.Add(p);
            PlayerTrs.Add(p.GetComponent<Transform>());
        }
        
        time = FindObjectOfType<TimeManager>();

        if (PhotonNetwork.OfflineMode)
        {
         StartCoroutine(corr);
         StartCoroutine(find());
         isMaster = true;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(corr);
               StartCoroutine(find());
               isMaster = true;
            }
        }

        poisonCor = poisonAttack();
    }

    public void OnPathComplete (Path p) {
        if (isMaster)
        {
            //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

            if (!p.error) {
                path = p;
                // Reset the waypoint counter so that we start to move towards the first point in the path
                currentWaypoint = 0;
            }   
        }
    }
    public void stopCor()
    {
        StopCoroutine(corr);
    }

    public void nightDetect()
    {
        if (!enemy.isFinding && enemy.canMove)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (!Players[i].isDead)
                { 
                    Transform tr = PlayerTrs[i];
                    if (Vector3.Distance(transform.position, tr.position) < nightDetecctRad)
                    { 
                        enemy.setPlayer(tr);
                        break;
                    }
                }
            }
        }
    }

    void GoPath()
    {
        if (path == null) //경로가 비었으면 
        {
            return; //아무것도 안함
        }
        reachedEndOfPath = false;
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position,
                path.vectorPath[currentWaypoint]);
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
        float PanValue = 1;
        if (isPan1)
            PanValue = pan1SpeedPercent / 100f;
        else if (isPan2)
            PanValue = pan2SpeedPercent / 100f;
        var speedFactor =
            reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed * speedFactor*PanValue;
        rigid.velocity = velocity;

        enemy.setAnim("Walk");
        enemy.setLocalX(enemy.targetPosition.transform.position.x);
    }
    public void Update () 
        {
            if (isMaster)
            {
                if (poisonTime < AttackTime)
                    poisonTime += Time.deltaTime;
              if (!enemy.isFinding && enemy.canMove) 
              {
                  for (int i = 0; i < Players.Count; i++)
                          {
                              if (!Players[i].isDead)
                              {
                                  Transform tr = PlayerTrs[i];
                                  if (tr != null)
                                  {
                                      float rad = detectRad;
                                      if (time.isNight)
                                          rad = nightDetecctRad;
                                      if (Vector3.Distance(transform.position, tr.position) < rad)
                                      {
                                          enemy.setPlayer(tr);
                                          break;
                                      }
                                  }
                              }
                          }
 
              }
              else
              {
                  if (enemy.isFinding&& enemy.canMove)
                  {
                      if (enemy.targetPosition.GetComponent<Player>().isDead)
                      {
                          enemy.ExclamationClose();
                          Restart();
                      }
                      if (zombieIndex == 1 || zombieIndex == 4 || zombieIndex == 3)
                      { 
                          GoPath();
                      }
                      else if (zombieIndex == 2)
                      {
                          bool isDetecting = false;
                          Transform tr = null;
                          for (int i = 0; i < Players.Count; i++)
                          {
                              if (!Players[i].isDead)
                              {
                                  tr = PlayerTrs[i];
                                  if (tr != null)
                                  {
                                      if (Vector3.Distance(transform.position, tr.position) < detectRad+1f)
                                      {
                                          isDetecting = true;
                                          break;
                                      }
                                  }
                              }
                          }

                          if (isDetecting)
                          {
                              rigid.velocity=Vector2.zero;
                              enemy.setLocalX(tr.position.x);
                              if (canPoison == false)
                              {
                                  canPoison = true;
                                  StopCoroutine(poisonCor);
                                  StartCoroutine(poisonCor);
                              }
                          }
                          else
                          {
                              canPoison = false;
                              GoPath();
                          }
                      }
                  }
              }
            }
        }

    public void stopPoison()
    {
        canPoison = false;
        GoPath();
    }
    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
{
float dx = x2 - x1;
float dy = y2 - y1;

float rad = Mathf.Atan2(dx, dy);
float degree = rad * Mathf.Rad2Deg;
        
    return degree;
}
    IEnumerator poisonAttack()
    {
        while (true)
        {
            if (poisonTime < AttackTime)
            {
                enemy.setAnim("Idle");
            }
            else
            {
                enemy.setAnim("Attack");
                Vector3 angle=new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, enemy.targetPosition.position.x, enemy.targetPosition.position.y)+90); 
                if (PhotonNetwork.OfflineMode)
                {
                    Instantiate(Poison, transform.position, Quaternion.Euler(angle));
                }
                else
                {
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.InstantiateRoomObject(Poison.name, transform.position, Quaternion.Euler(angle));
                }
                poisonTime = 0;
            }
            
            yield return new WaitForSeconds(0.2f);
            enemy.setAnim("Idle");
            yield return new WaitForSeconds(AttackTime-0.2f);
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
            

            if (Vector2.Distance(transform.position, roamPos) < 2f)
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

    public void OnDisable()
    {
        if(isMaster) 
            enemy.seeker.pathCallback -= OnPathComplete;
    }
    
   private Vector2 GetRoamingPosition()
   {
       float randomMove = Random.Range(minMove, maxMove);
       return transform.position + UtilsClass.GetRandomDir() * randomMove;
   }


   private void OnCollisionStay2D(Collision2D other)
   {
       if (isMaster)
       {
           if (enemy.canMove && !enemy.isFinding)
           {
               if (other.gameObject.CompareTag("Wall")) 
               { 
                   Restart(); 
               }
           }
       }
   }
   
   private void OnCollisionEnter2D(Collision2D other)
   {
       if (isMaster)
       {
           if (enemy.canMove)
           {
               if (other.gameObject.CompareTag("Player") && enemy.canMove)
               {
                   StartCoroutine(Attack());
               }
           }
       }
   }

   void Restart()
   {
       StopCoroutine(corr);
       StartCoroutine(corr);
   }
   
   IEnumerator Attack()
   {
       StopCoroutine(corr);
       enemy.isFinding = false;
       enemy.canMove = false;
       rigid.velocity=Vector2.zero;

       enemy.setLocalX(enemy.targetPosition.position.x);
       enemy.setAnim("Attack");
       
       yield return new WaitForSeconds(AttackTime);
       enemy.setAnim("Walk");
       enemy.canMove = true;
//       for (int i = 0; i < Players.Count; i++)
//       {
//           if (!Players[i].isDead)
//           {
//               Transform tr = PlayerTrs[i];
//               float rad = detectRad;
//               if (time.isNight)
//                   rad = nightDetecctRad;
//               if (Vector3.Distance(transform.position, tr.position) < rad)
//               {
//                   enemy.isFinding = true;
//                   enemy.targetPosition = tr;
//                   enemy.setAnim("Walk");
//                   break;
//               }
//           }
//       }   
   }

   private void OnTriggerStay2D(Collider2D other)
   {
       if (other.CompareTag("Pan1"))
           isPan1 = true;
       if (other.CompareTag("Pan2"))
           isPan2 = true;
   }

   private void OnTriggerExit2D(Collider2D other)
   {
       if (other.CompareTag("Pan1"))
           isPan1 = false;
       if (other.CompareTag("Pan2"))
           isPan2 = false;
   }

   public void Detect(float rad)
   {
       for (int i = 0; i < Players.Count; i++)
       {
           if (!Players[i].isDead)
           {
               Transform tr = PlayerTrs[i];
               if (tr != null)
               {
                   if (Vector3.Distance(transform.position, tr.position) < rad)
                   {
                       enemy.setPlayer(tr);
                       break;
                   }
               }
           }
       }   
   }
}
