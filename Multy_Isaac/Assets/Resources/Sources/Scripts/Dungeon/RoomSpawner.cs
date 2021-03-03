using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    //public long spawnedTick = System.DateTime.Now.Ticks;
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


    private void Awake()
    {
       if(!PhotonNetwork.OfflineMode)
        {
            if(!PhotonNetwork.IsMasterClient) 
                Destroy(gameObject);
        }
    }

    private void Start()
    {
        templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        if(!isConstant) 
            Destroy(gameObject,waitTime);
        Invoke("Spawn",0.05f);
    }

    void SimpleSpawn()
    {
        if (PercentReturn(templates.TBLRper))
            rand = 0;
        else
            rand= Random.Range(1, 4);
    }
    void Spawn()
    {
        int playerValue = 0; //플레이어 

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
            
            SimpleSpawn();
            
                    if (PhotonNetwork.OfflineMode)
                    {
                        Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation); 
                        //g.GetComponent<AddRoom>().SetRoom(specialValue);
                    }
                    else
                    {
                         PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position, rooms[rand].transform.rotation).GetComponent<AddRoom>();
                        //pv.SetRoom(specialValue);
                    }
                

                
            spawned = true; //소환됨으로 바꿈
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint") && !isConstant)
                {
                    if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false) //겹친 방이 아직 생성되지 않았고, 자신도 생성되지 않았다면
                    {
                        if (PhotonNetwork.OfflineMode)
                        {
                            try
                            {
                                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);   
                            }
                            catch (Exception e)
                            { }
                        }
                        else
                        {
                            try
                            {
                                PhotonNetwork.InstantiateRoomObject(templates.closedRoom.name, transform.position, Quaternion.identity);
                            }
                            catch (Exception e)
                            { }
                        }

                        other.GetComponent<RoomSpawner>().spawned = true;
                    }
                    spawned = true;
                }
            else if (other.CompareTag("SpawnPoint") && isConstant)
            {
                spawned = true;
            }
        
        if(other.CompareTag("Space"))
            Destroy(other.gameObject);
    }
    
     bool PercentReturn(int percent)
       {
          if (Random.Range(1, 101) <= percent)
             return true;
          else
             return false;
       }
}
