using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
   public int minRoomCount = 7;
   public int maxRoomCount = 50;
   public int PlayerCount = 4;
   public GameObject[] bottomRooms;
   public GameObject[] topRooms;
   public GameObject[] leftRooms;
   public GameObject[] rightRooms;
   
   
   public GameObject closedRoom;

   public List<GameObject> rooms;

   public float waitTime;
   private bool spawnedBoss = false;
   public GameObject boss;

   void Update()
   {
      if (waitTime <= 0 && spawnedBoss == false) 
      {
         Instantiate(boss, rooms[rooms.Count-1].transform.position, quaternion.identity);
         spawnedBoss = true;
      }
      else
      {
         if(waitTime>0) 
            waitTime -= Time.deltaTime;
      }
   }
}
