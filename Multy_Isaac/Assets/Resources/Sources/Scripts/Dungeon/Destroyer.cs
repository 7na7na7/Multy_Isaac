using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
   private RoomTemplates templates;
   
   private void Start()
   {
      templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
      Invoke("Spawn",templates.DestroyerWaitTime);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      Destroy(other.gameObject);
   }

   void Spawn()
   {
      Destroy(gameObject);
   }
}
