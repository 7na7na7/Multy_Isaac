using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddRoom : MonoBehaviour
{
    public float delay1, delay2, delay3;
    private RoomTemplates templates;

    private bool gizmoOn = false;
    private Vector2 first, second;

    public bool isBig = false;
    int r;
    private bool canSpawn;
  private void Start()
  {
      if (PhotonNetwork.OfflineMode)
      {
          set();
      }
      else
      {
          if(PhotonNetwork.IsMasterClient)
              set();
      }
  }

  void set()
  {
      templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
      isBig= PercentReturn(templates.BigRoomPercent);

      if (isBig)
      {
          Invoke("checkBig",delay1);
          Invoke("SetRoom",delay2);
      }
      else
      {
          Invoke("SetRoom",delay3);   
      }
  }
  void checkBig()
  {
      r = Random.Range(0, templates.RoomProps_Big.Length);
      RaycastHit2D[] hit = Physics2D.BoxCastAll(
          (Vector2)transform.position+templates.RoomProps_Big[r].GetComponent<RoomProps>().offset,
          templates.RoomProps_Big[r].GetComponent<RoomProps>().BoxSize,0,Vector2.down,0);

      bool a = false;
      foreach (RaycastHit2D c in hit)
      {
          if (c.collider.CompareTag("Space"))
          {
              if (!c.collider.GetComponent<AddRoom>().isBig)
              {
                  a = true;
                  Destroy(c.collider.gameObject);   
              }
          }
      }

      if (a == false)
          isBig = false;
  }
  public void SetRoom()
  {
      templates.rooms.Add(this.gameObject);
    SetRoomProps(); //생성
  }

  void SetRoomProps()
  {
     if (isBig) //큰방생성
     {
         // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
                       
         RaycastHit2D[] hit = Physics2D.BoxCastAll(
             (Vector2)transform.position+templates.RoomProps_Big[r].GetComponent<RoomProps>().offset,
             templates.RoomProps_Big[r].GetComponent<RoomProps>().BoxSize,0,Vector2.down,0);

                        
         canSpawn = true;
         foreach (RaycastHit2D c in hit)
         {
             if (c.collider.CompareTag("Space"))
             {
                 Destroy(c.collider.gameObject);   
             }
             if (c.collider.CompareTag("Wall") || c.collider.CompareTag("SpawnPoint")||c.collider.CompareTag("Destroyer")) //벽과 닿으면 생성못함
             {
                 canSpawn = false;
             }
         }
                        if (canSpawn)
                        { //안닿았으면은
                            if (PhotonNetwork.OfflineMode)
                            {
                                Instantiate(templates.RoomProps_Big[r], transform.position,quaternion.identity);
                            }
                            else
                            {
                                PhotonNetwork.InstantiateRoomObject(templates.RoomProps_Big[r].name, transform.position, quaternion.identity);
                            }
                        }
                        else //닿았으면
                        {
                            justSpawn();
                        }
                }
     else
     {
        justSpawn();
     }

  }

  void justSpawn()
  {
      int randomAreaIndex = Random.Range(0, templates.RoomProps.Length);
      
      if (PhotonNetwork.OfflineMode)
      {
          Instantiate(templates.RoomProps[randomAreaIndex], transform.position, quaternion.identity);
      }
      else
      {
          PhotonNetwork.InstantiateRoomObject(templates.RoomProps[randomAreaIndex].name, transform.position, quaternion.identity);
      }   
  }
  bool PercentReturn(int percent)
  {
      if (Random.Range(1, 101) <= percent)
          return true;
      else
          return false;
  }
  
  private void OnDrawGizmos()
  {
      if (gizmoOn)
      {
          Gizmos.color = Color.red;
          Gizmos.DrawWireCube(first,second);
      }
  }
}
