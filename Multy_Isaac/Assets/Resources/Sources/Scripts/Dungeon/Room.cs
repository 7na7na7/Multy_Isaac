using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
   public int Width; //길이
   public int Height;//높이

   public int X;
   public int Y;

   private void Start()
   {
      if (RoomController.instance == null)
      {
         Debug.Log("You pressed play in the wrong scene!");
         return;
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color=Color.red;
      Gizmos.DrawWireCube(transform.position,new Vector3(Width,Height,0));
   }

   public Vector3 GetRoomCenter()
   {
      return new Vector3(X*Width,Y*Height);
   }
}
