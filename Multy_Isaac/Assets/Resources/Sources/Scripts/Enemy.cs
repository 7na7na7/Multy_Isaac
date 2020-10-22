using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : MonoBehaviour//PunCallbacks, IPunObservable
{
  private FlashWhite flashwhite;
  public int hp = 50;
  public PhotonView pv;
  public float CollsionDamage = 20;
  
  private void Start()
  {
    flashwhite = GetComponent<FlashWhite>();
  }

  [PunRPC]
  public void HitRPC(int value)
  {
    flashwhite.Flash();
    hp -= value;
    if(hp<=0)
      Destroy(gameObject);
  }

//  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //변수 동기화
//  {
//    if (stream.IsWriting)
//    {
//      stream.SendNext(hp);
//    }
//    else
//    {
//      hp = (int) stream.ReceiveNext();
//    }
//  }
}
