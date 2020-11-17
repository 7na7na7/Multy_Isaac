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
   private int count;
   public int publicCount;
   private void Start()
   {
      if (PhotonNetwork.OfflineMode)
      {
         Invoke("Spawn",waitTime);
         Invoke("ReLoad",ReLoadTime);
      }
      else
      {
         if (PhotonNetwork.IsMasterClient)
         {
            count = FindObjectOfType<playerCountSave>().playerCount;
            publicCount = count;
            print(count);
            maxRoomCount = count * maxRoomCount;
            maxRoomCountSave = maxRoomCount;
          
            Invoke("Spawn",waitTime);  
         }
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
         Instantiate(boss,  rooms[rooms.Count-1].transform.position, quaternion.identity);
      }
      else
      {
         int PlayerCount = count;
         Player[] players = FindObjectsOfType<Player>();

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
