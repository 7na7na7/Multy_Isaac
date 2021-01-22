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


    private bool gizmoOn = false;
    private Vector2 first, second;
    
    void set()
    {
        transform.parent.GetChild(0).gameObject.SetActive(true);
        Destroy(gameObject);
    }

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
        Invoke("set",waitTime);  
        Destroy(gameObject,waitTime);
          templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
                       Invoke("Spawn",0.1f);
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
            
            SimpleSpawn();

           
                if (PercentReturn(templates.BigRoomPercent)) //큰방생성
                {
                    // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
                        RaycastHit2D[] hit = Physics2D.BoxCastAll((Vector2)transform.position+bigRooms[rand].GetComponent<AddRoom>().offset,bigRooms[rand].GetComponent<AddRoom>().BoxSize,0,Vector2.down,0);

                        
                        bool canSpawn = true;
                        foreach (RaycastHit2D c in hit)
                        {
                            gizmoOn = true;
                            first = (transform.position + (Vector3) bigRooms[rand].GetComponent<AddRoom>().offset) + (transform.forward * c.distance);
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
                                //print(bigRooms[rand].name+"스폰하려다 "+rooms[rand].name+"소환!"+transform.position);
                                GameObject g= Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation); 
                                g.GetComponent<AddRoom>().SetRoom();
                            }
                            else
                            {
                                AddRoom pv = PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position, rooms[rand].transform.rotation).GetComponent<AddRoom>();
                                pv.SetRoom();
                            }
                        }
                }
                else
                {
                    if (PhotonNetwork.OfflineMode)
                    {
                        GameObject g= Instantiate(rooms[rand], transform.position,rooms[rand].transform.rotation); 
                        //g.GetComponent<AddRoom>().SetRoom(specialValue);
                    }
                    else
                    {
                        AddRoom pv = PhotonNetwork.InstantiateRoomObject(rooms[rand].name, transform.position, rooms[rand].transform.rotation).GetComponent<AddRoom>();
                        //pv.SetRoom(specialValue);
                    }
                }

                
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
        if (other.CompareTag("SpawnPoint") && !isConstant)
                {
                    if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false) //겹친 방이 아직 생성되지 않았고, 자신도 생성되지 않았다면
                    {
                        if (PhotonNetwork.OfflineMode)
                            Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                        else
                            PhotonNetwork.InstantiateRoomObject(templates.closedRoom.name, transform.position, Quaternion.identity);
                        other.GetComponent<RoomSpawner>().spawned = true;
                    }
                    spawned = true;
                }
            else if (other.CompareTag("SpawnPoint") && isConstant)
            {
                if (other.GetComponent<RoomSpawner>().isConstant) //둘다 isConstant면
                {
                    print(transform.position);
                    if(other.transform.parent.GetComponent<AddRoom>().BoxSize==Vector2.zero) //상대가 작은방이면 파괴
                        Destroy(other.gameObject.transform.parent.gameObject);
                    else //아니면 나를 파괴
                        Destroy(gameObject.transform.parent.gameObject);
                    
                }
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
