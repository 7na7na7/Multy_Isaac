using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
   private void Start()
   {
      //Destroy(gameObject,10f);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      Destroy(other.gameObject);
   }
}
