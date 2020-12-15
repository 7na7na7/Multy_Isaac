using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Slash : MonoBehaviour
{
   public PhotonView pv;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         if (other.GetComponent<PhotonView>().IsMine && !pv.IsMine && !other.GetComponent<Player>().isSuper)
         {
            other.GetComponent<Player>().Hit(10, pv.Controller.NickName);
         }
      }
   }
}
