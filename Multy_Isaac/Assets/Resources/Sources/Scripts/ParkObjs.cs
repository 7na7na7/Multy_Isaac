using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ParkObjs : MonoBehaviour
{
   public bool isRoad = true;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.name != "fountain")
      {
         if (isRoad)
         {
            if (other.CompareTag("Flower") || other.CompareTag("Wall") || other.CompareTag("Bush"))
               Destroy(other.gameObject);
         }
         else
         {
            if(other.CompareTag("Flower")||other.CompareTag("Rock")|| other.CompareTag("Wall") ||other.CompareTag("Bush"))
               Destroy(other.gameObject);
         }  
      }
   }
}
