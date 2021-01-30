using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ParkObjs : MonoBehaviour
{
   private PhotonView pv;
   public bool isFlower = false;

   private void Start()
   {
      pv = GetComponent<PhotonView>();
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (isFlower)
      {
         if (other.CompareTag("Road") || other.CompareTag("Fountain"))
         {
            if(PhotonNetwork.OfflineMode)
               DestroyRPC();
            else
            {
               if (PhotonNetwork.IsMasterClient)
               {
                  pv.RPC("DestroyRPC",RpcTarget.AllBuffered);
               }
            }
         }  
      }
      else
      {
         if (other.CompareTag("Fountain"))
         {
            if(PhotonNetwork.OfflineMode)
               DestroyRPC();
            else
            {
               if (PhotonNetwork.IsMasterClient)
               {
                  pv.RPC("DestroyRPC",RpcTarget.AllBuffered);
               }
            }
         }
      }
   }

   [PunRPC]
   void DestroyRPC()
   { 
      Destroy(gameObject);   
   }
}
