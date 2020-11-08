using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
   public int StraightCount = 0;
   public int minRoomCount = 7;
   public int maxRoomCount = 50;
   public int maxRoomCountSave;
   public int PlayerCount = 4;
   public int PlayerSpawnMinusValue = 3;
   public GameObject[] bottomRooms;
   public GameObject[] topRooms;
   public GameObject[] leftRooms;
   public GameObject[] rightRooms;
   
   
   public GameObject closedRoom;

   public List<GameObject> rooms;

   public float waitTime;
   public float DestroyerWaitTime;
   public float ReLoadTime;
   public GameObject boss;

   private Vector3 pos;
   private void Start()
   {
      if (PhotonNetwork.OfflineMode)
      {
         Invoke("Spawn",waitTime);
         Invoke("ReLoad",ReLoadTime);
      }
      else
      {
         if(PhotonNetwork.IsMasterClient) 
            Invoke("Spawn",waitTime);  
      }
   }

   void ReLoad()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
   void Spawn()
   {
      if (PhotonNetwork.OfflineMode)
      {
         Player player = FindObjectOfType<Player>();
         PlayerCount = 1;
         maxRoomCount = PlayerCount * maxRoomCount*2;
         Instantiate(boss,  rooms[rooms.Count-1].transform.position, quaternion.identity);
        for (int i = 0; i < rooms.Count-1; i++)
         {
            if (rooms[i].CompareTag("Entry"))
            {
               if (PlayerCount > 0)
               {
                  player.Move(rooms[i].transform.position);
                  PlayerCount--;
               }
            }
         }
         if(PlayerCount>0)
            print("방 제대로 생성안됐다 시발!!!!!!!!!!!!!!");
      }
      else
      {
         Player[] players = FindObjectsOfType<Player>();
         PlayerCount = players.Length;
         int count = PlayerCount;
         maxRoomCount = count * maxRoomCount;
         PhotonNetwork.InstantiateRoomObject(boss.name,  rooms[rooms.Count-1].transform.position, quaternion.identity);
        
         for (int i = 0; i < rooms.Count-1; i++)
         {
            if (rooms[i].CompareTag("Entry"))
            {
               if (PlayerCount > 0)
               {
                  players[count - PlayerCount].pv.RPC("Move",RpcTarget.All,rooms[i].transform.position);
                  PhotonNetwork.InstantiateRoomObject("HowTo", rooms[i].transform.position, quaternion.identity);
                  PlayerCount--;  
               }
            }
         }
         if(PlayerCount>0)
            print("방 제대로 생성안됐다 시발!!!!!!!!!!!!!!");  
      }
   }
}
