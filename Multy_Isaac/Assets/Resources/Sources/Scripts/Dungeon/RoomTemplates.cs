using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class RoomTemplates : MonoBehaviour
{
   private PhotonView pv;
   public float delay;
   public int TBLRper = 90;
   public GameObject[] RoomProps;
   public GameObject[] RoomProps_Big;
   
   //한칸짜리 방들
   public GameObject[] bottomRooms;
   public GameObject[] topRooms;
   public GameObject[] leftRooms;
   public GameObject[] rightRooms;
   [Header("큰방이 생성된확률(백분율)")]
   public int BigRoomPercent;


   public GameObject closedRoom;

   public List<GameObject> rooms;

   public float waitTime;
   public float ReLoadTime;
   public GameObject boss;

   private Vector3 pos;

   private void Start()
   {
      pv = GetComponent<PhotonView>();
      if (PhotonNetwork.OfflineMode)
      {
         //Invoke("Spawn",waitTime);
         Invoke("ReLoad",ReLoadTime);
      }
      else
      {
         if (PhotonNetwork.IsMasterClient)
         {

            //Invoke("Spawn",waitTime);  
            //Invoke("ReLoad",ReLoadTime);
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
         Player[] players = FindObjectsOfType<Player>();
         
         PhotonNetwork.InstantiateRoomObject(boss.name,  rooms[rooms.Count-1].transform.position, quaternion.identity);
//         foreach (Player p in players)
//         {
//            print(p.nickname.text);
//         }

//         for (int i = 0; i < rooms.Count-1; i++)
//         {
//            if (rooms[i].CompareTag("Entry"))
//            {
//               if (PlayerCount > 0)
//               {
//                  print(players[privateCount-PlayerCount].nickname.text);
//                  players[privateCount - PlayerCount].GetComponent<Player>().pv.RPC("Move",RpcTarget.AllBuffered,rooms[i].transform.position);
//                  PhotonNetwork.InstantiateRoomObject("HowTo", rooms[i].transform.position, quaternion.identity);
//                  //players[privateCount - PlayerCount].GetComponent<Player>().setCam();
//                  PlayerCount--;
//               }
//            }
//         }
      }
   }
   
   
}
