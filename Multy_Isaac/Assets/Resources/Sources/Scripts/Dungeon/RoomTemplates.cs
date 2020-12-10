using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomTemplates : MonoBehaviour
{
   public int StraightCount = 0;
   public int minRoomCount = 7;
   public int maxRoomCount = 50;
   public int maxRoomCountSave;
   public int PlayerSpawnMinusValue = 3;
   //한칸짜리 방들
   public GameObject[] bottomRooms;
   public GameObject[] topRooms;
   public GameObject[] leftRooms;
   public GameObject[] rightRooms;
   [Header("큰방이 생성된확률(백분율)")]
   public int BigRoomPercent;
   //큰방들
   public GameObject[] bottomRooms_B;
   public GameObject[] topRooms_B;
   public GameObject[] leftRooms_B;
   public GameObject[] rightRooms_B;
   
   
   public GameObject closedRoom;

   public List<GameObject> rooms;

   public float waitTime;
   public float DestroyerWaitTime;
   public float ReLoadTime;
   public GameObject boss;

   private Vector3 pos;
   public int privateCount;
   public int publicCount;

   public Vector2 oneBox;
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
            privateCount = FindObjectOfType<playerCountSave>().playerCount;
            publicCount = privateCount;

            maxRoomCount = privateCount * maxRoomCount;
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
         int PlayerCount = privateCount;
         GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
         
         PhotonNetwork.InstantiateRoomObject(boss.name,  rooms[rooms.Count-1].transform.position, quaternion.identity);
         foreach (GameObject p in players)
         {
            print(p.GetComponent<Player>().nickname.text);
         }
         for (int i = 0; i < rooms.Count-1; i++)
         {
            if (rooms[i].CompareTag("Entry"))
            {
               if (PlayerCount > 0)
               {
                  players[privateCount - PlayerCount].GetComponent<Player>().pv.RPC("Move",RpcTarget.All,rooms[i].transform.position);
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
