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
    private bool spawned = false;

    public float waitTime = 4f;
    private void Start()
    {
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

    void Spawn()
    {
        int playerValue = 0; //플레이어 
        if(templates.PlayerCount>0) 
            playerValue= ((templates.maxRoomCountSave-templates.PlayerSpawnMinusValue) / templates.PlayerCount); // 최대 방수 - 지정값(이 값만큼 보스로부터 떨어짐) / 플레이어 수(4부터 점점 줄어듦)
        if (spawned == false)//생성되지 않았으면 생성!
        {
            GameObject[] rooms = null;
            switch (openingDirection)
            {
                case 1:
                    rooms = templates.bottomRooms;
                    break;
                case 2:
                    rooms = templates.topRooms;
                    break;
                case 3:
                    rooms = templates.leftRooms;
                    break;
                case 4:
                    rooms = templates.rightRooms;
                    break;  
            } //방 위치 정해주기

            
            if (templates.minRoomCount > 0)//만약 최소방수가 아직 채워지지 않았다면
            {
                while (true)
                {
                    rand = Random.Range(0, rooms.Length-1);
//                    if (rand != 0&&rand != 1&&rand != 2)
//                        break;
                    if (rand == 2 || rand == 3 || rand == 4)
                        break;
                }
            } 
            else if (templates.maxRoomCount<0)//최소방수가 채워졌고, 최대방수가 채워지지 않았다면(이게 제일 많이 호출됨)
            {
                while (true)
                {
                    rand = Random.Range(0, rooms.Length-1);
                    if (rand == 0)
                        break;
                }
            } 
            else//최소방수가 채워졌고, 최대방수도 채워졌다면
            {
                if (templates.PlayerCount > 0)
                {
                    if (templates.rooms.Count + 1 > playerValue)
                    {
                        rand = rooms.Length - 1; //배열 마지막에 있는 Entry를 소환하도록 함
                        templates.PlayerCount--;
                    }
                    else
                    {
                        rand = Random.Range(0, rooms.Length-1);
                    }
                }
                else
                {
                    rand = Random.Range(0, rooms.Length-1);   
                }
            } 
            
            
            if (PhotonNetwork.OfflineMode) //오프라인 모드면
                Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation);
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
}
