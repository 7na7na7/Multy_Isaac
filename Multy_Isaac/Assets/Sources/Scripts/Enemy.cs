using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : MonoBehaviour//PunCallbacks, IPunObservable
{
  public int hp = 50;
  public PhotonView pv;
  
  [PunRPC]
  public void HitRPC(int value)
  {
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
