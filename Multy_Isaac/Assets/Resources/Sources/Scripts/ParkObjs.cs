using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ParkObjs : MonoBehaviour
{
   private PhotonView pv;

   private void Start()
   {
      pv = GetComponent<PhotonView>();
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Road"))
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

   [PunRPC]
   void DestroyRPC()
   { 
      Destroy(gameObject);   
   }
}
