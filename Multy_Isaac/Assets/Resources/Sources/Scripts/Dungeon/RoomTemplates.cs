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
   public float delay;
   public int TBLRper = 90;
   public GameObject[] RoomProps;
   public GameObject[] RoomProps_Big;
   public GameObject oneRoom;
   public bool isOneRoom = false;

   //한칸짜리 방들
   public GameObject[] bottomRooms;
   public GameObject[] topRooms;
   public GameObject[] leftRooms;
   public GameObject[] rightRooms;
   [Header("큰방이 생성된확률(백분율)")]
   public int BigRoomPercent;


   public GameObject closedRoom;

   public List<GameObject> rooms;
   
   public float ReLoadTime;

   private Vector3 pos;

   private void Start()
   {
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
}
