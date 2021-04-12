using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
   public bool IsoutDestroyer = false;
   private void Start()
   {
      //Destroy(gameObject,10f);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (IsoutDestroyer)
      {
         if(!other.CompareTag("Player") && !other.CompareTag("Gas")) 
            Destroy(other.gameObject);  
      }
      else
      {
         if(other.CompareTag("Wall"))
            Destroy(gameObject);
      }
   }
}
