using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public bool isConstant = false;
    public int openingDirection;
    //1 --> need bottom door
    //2 --> need top door
    //3 --> need left door
    //4 --> need right door
    private RoomTemplates templates;
    
    private int rand;
    public bool spawned = false;

    public float waitTime = 4f;


    private bool gizmoOn = false;
    private Vector2 first, second;
    
    void set()
    {
        transform.parent.GetChild(0).gameObject.SetActive(true);
        Destroy(gameObject);
    }
    
    private void Start()
    {
        Invoke("set",waitTime);  
        Destroy(gameObject,waitTime);
        if (PhotonNetwork.OfflineMode)
        {
            templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            Invoke("Spawn",0.1f);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
                Invoke("Spawn",0.1f);
            }   
        }
    }

    void SimpleSpawn()
    {
        if (templates.StraightCount == 0)
        {
            templates.StraightCount = 4;
            rand = 0;
        }
        else
        {
            templates.StraightCount--;
            rand = Random.Range(0, 4);
        }
    }
    void Spawn()
    {
        int playerValue = 0; //플레이어 
        if (templates.publicCount > 0)
        {
            playerValue= ((templates.maxRoomCountSave-templates.PlayerSpawnMinusValue) / templates.privateCount)*(templates.privateCount-templates.publicCount+1); // 최대 방수 - 지정값(이 값만큼 보스로부터 떨어짐) / 플레이어 수(4부터 점점 줄어듦)
            print(playerValue);
        }
        if (spawned == false)//생성되지 않았으면 생성!
        {
            GameObject[] rooms = null;
            GameObject[] bigRooms = null;
            switch (openingDirection)
            {
                case 1:
                    rooms = templates.bottomRooms;
                    bigRooms = templates.bottomRooms_B;
                    break;
                case 2:
                    rooms = templates.topRooms;
                    bigRooms = templates.topRooms_B;
                    break;
                case 3:
                    rooms = templates.leftRooms;
                    bigRooms = templates.leftRooms_B;
                    break;
                case 4:
                    rooms = templates.rightRooms;
                    bigRooms = templates.rightRooms_B;
                    break;  
            } //방 위치 정해주기
            
            
            if (templates.minRoomCount > 0)//만약 최소방수가 아직 채워지지 않았다면
            {
               SimpleSpawn();
            } 
            else if (templates.maxRoomCount<0)//최소방수가 채워졌고, 최대방수도 채워졌다면
            {
                rand = rooms.Length - 2; //막힌방만 생성
            } 
            else//최소방수가 채워졌고, 최대방수는 채워지지 않았다면(제일많이 호출)
            {
                if (templates.publicCount > 1 &&templates.rooms.Count + 1 > playerValue)
                {
                    //print(templates.rooms.Count+1+" "+playerValue);
                        rand = rooms.Length - 1; //배열 마지막에 있는 Entry를 소환하도록 함
                    templates.publicCount--;
                }
                else
                {
                   SimpleSpawn();
                }
            }

           
                if (PercentReturn(templates.BigRoomPercent)) //큰방생성
                {
                    // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
                        RaycastHit2D[] hit = Physics2D.BoxCastAll((Vector2)transform.position+bigRooms[rand].GetComponent<AddRoom>().offset,bigRooms[rand].GetComponent<AddRoom>().BoxSize,0,Vector2.down,0);

                        
                        bool canSpawn = true;
                        foreach (RaycastHit2D c in hit)
                        {
                            gizmoOn = true;
                            first = (transform.position + (Vector3) bigRooms[rand].GetComponent<AddRoom>().offset) +
                                    (transform.forward * c.distance);
                            second = bigRooms[rand].GetComponent<AddRoom>().BoxSize;
                            if (c.collider.CompareTag("Wall")) //벽과 닿으면 생성못함
                            {
                                //rand = rooms.Length - 2; //큰방
                                canSpawn = false;
                                break;
                            }
                        }

                        if (canSpawn)
                        { //안닿았으면은
                            if (PhotonNetwork.OfflineMode)
                            {
                                GameObject g=Instantiate(bigRooms[rand], transform.position,bigRooms[rand].transform.rotation);
                                g.GetComponent<AddRoom>().SetRoom();
                            }
                            else
                            {
                                AddRoom pv = PhotonNetwork.InstantiateRoomObject(bigRooms[rand].name, transform.position,bigRooms[rand].transform.rotation).GetComponent<AddRoom>();
                                pv.SetRoom();
                            }
                        }
                        else //닿았으면
                        {
                            if (PhotonNetwork.OfflineMode)
                            {
                                print(rand);
                                print(bigRooms[rand].name+"스폰하려다 "+rooms[rand].name+"소환!"+transform.position);
                                GameObject g= Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation); 
                                g.GetComponent<AddRoom>().SetRoom();
                            }
                            else
                            {
                                AddRoom pv = PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position, rooms[rand].transform.rotation).GetComponent<AddRoom>();
                                pv.SetRoom();
                            }
                        }
//                        else //닿았으면 작은방으로 한번더검사
//                        {
//                            Vector3 offset=Vector3.zero;
//                            switch (rooms[rand].transform.GetChild(0).GetComponent<RoomSpawner>().openingDirection) //1위 2아래 3오른 4왼
//                            {
//                                case 1:
//                                    offset.y = -10;
//                                    break;
//                                case 2:
//                                    offset.y = 10;
//                                    break;
//                                case 3:
//                                    offset.x = -18;
//                                    break;
//                                case 4:
//                                    offset.x = 18;
//                                    break;
//                            }
//                            print(offset);
//                            
//                            RaycastHit2D[] hit2 = Physics2D.BoxCastAll(transform.position+offset,templates.oneBox,0,Vector2.down,0);
//
//                        
//                            bool canSpawn2 = true;
//                            foreach (RaycastHit2D c in hit2)
//                            {
//                                gizmoOn = true;
//                                first = (transform.position + offset) +
//                                        (transform.forward * c.distance);
//                                second = templates.oneBox;
//                                if (c.collider.CompareTag("Wall")) //벽과 닿으면 생성못함
//                                {
//                                    rand = rooms.Length - 2;
//                                    canSpawn2 = false;
//                                    break;
//                                }
//                            }
//                            if (canSpawn2) //작은방 생성이 가능하면
//                            {
//                                print("A");
//                                if(PhotonNetwork.OfflineMode)
//                                    Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation);
//                                else
//                                    PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position,rooms[rand].transform.rotation);
//                            }
//                        }
                }
                else
                {
                    if (PhotonNetwork.OfflineMode)
                    {
                        GameObject g= Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation); 
                        g.GetComponent<AddRoom>().SetRoom();
                    }
                    else
                    {
                        AddRoom pv = PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position, rooms[rand].transform.rotation).GetComponent<AddRoom>();
                        pv.SetRoom();
                    }
                }

                
            
            if(templates.minRoomCount>0) 
                templates.minRoomCount--;
            templates.maxRoomCount--;
            
            spawned = true; //소환됨으로 바꿈
            
        }
    }

    private void OnDrawGizmos()
    {
        if (gizmoOn)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(first,second);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.OfflineMode)
        {
            if (other.CompareTag("SpawnPoint") && !isConstant)
                {
                    if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false) //겹친 방이 아직 생성되지 않았고, 자신도 생성되지 않았다면
                    {
                       Instantiate(templates.closedRoom, transform.position,
                            Quaternion.identity);
                       other.gameObject.GetComponent<RoomSpawner>().spawned = true;
                       spawned = true;
                    }
                    spawned = true;
                }
            else if (other.CompareTag("SpawnPoint") && isConstant)
            {
                if (other.GetComponent<RoomSpawner>().isConstant)
                {
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
            }
        }
    }
    
    
    
     bool PercentReturn(int percent)
       {
          if (Random.Range(1, 101) <= percent)
             return true;
          else
             return false;
       }
}
