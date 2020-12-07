using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    //1 --> need bottom door
    //2 --> need top door
    //3 --> need left door
    //4 --> need right door
    private RoomTemplates templates;
    
    private int rand;
    public bool spawned = false;

    public float waitTime = 4f;

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
            templates.StraightCount = 3;
            rand = 0;
        }
        else
        {
            templates.StraightCount--;
            rand = Random.Range(0, templates.topRooms.Length-2);
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
                if (templates.publicCount > 0 &&templates.rooms.Count + 1 > playerValue)
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
            print("ROOM COUNT"+" "+rooms[rand].GetComponent<AddRoom>().isBig);

            if (PhotonNetwork.OfflineMode) //오프라인 모드면
            {
                if (PercentReturn(templates.BigRoomPercent) == true) //큰방생성
                {
                    if (bigRooms[rand].GetComponent<AddRoom>().isBig)
                    {
                        // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
                        RaycastHit2D[] hit = Physics2D.BoxCastAll((Vector2)transform.position+bigRooms[rand].GetComponent<AddRoom>().offset,bigRooms[rand].GetComponent<AddRoom>().BoxSize,0,Vector2.down,0);

                        foreach (RaycastHit2D c in hit)
                        {
                            if (c.collider.CompareTag("Wall")) //벽과 닿으면 생성못함
                            {
                                print(c.collider.name+" "+c.collider.transform.parent.gameObject.transform.parent.gameObject.name);
                                rand = rooms.Length - 2;
                                spawned = true;
                                break;
                            }
                        }
                        if(spawned)
                            Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation);
                        else
                            Instantiate(bigRooms[rand], transform.position,bigRooms[rand].transform.rotation);
                    }
                    else //큰방 생성을 못하면 원래 생성하려 했던 작은방 생성
                    {
                        Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation);
                    }
                }
                else
                {
                    Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation);
                }
            }
            else //온라인 모드면
                PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position,rooms[rand].transform.rotation);
            
            if(templates.minRoomCount>0) 
                templates.minRoomCount--;
            templates.maxRoomCount--;
            
            spawned = true; //소환됨으로 바꿈
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.OfflineMode)
        {
            if (other.CompareTag("SpawnPoint"))
                {
           
                    if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false
                    ) //겹친 방이 아직 생성되지 않았고, 자신도 생성되지 않았다면
                    {
                       Instantiate(templates.closedRoom, transform.position,
                            Quaternion.identity);
                        Destroy(gameObject); //방이 겹치면 자신을 파괴   
                    }

                    spawned = true;
                }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (other.CompareTag("SpawnPoint"))
                {
           
                    if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false
                    ) //겹친 방이 아직 생성되지 않았고, 자신도 생성되지 않았다면
                    {
                        PhotonNetwork.InstantiateRoomObject(templates.closedRoom.name, transform.position,
                            Quaternion.identity);
                        Destroy(gameObject); //방이 겹치면 자신을 파괴   
                    }

                    spawned = true;
                }
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
