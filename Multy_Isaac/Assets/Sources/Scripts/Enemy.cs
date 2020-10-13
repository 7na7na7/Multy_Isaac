using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public int hp = 100;
  public PhotonView pv;
  public void Hit(int value)
  {
    hp -= value;
    if(hp<=0)
      Die();
  }

  void Die()
  {
    pv.RPC("DestroyRPC", RpcTarget.AllBuffered);   
  }
  [PunRPC]
  public void DestroyRPC()
  {
    Destroy(gameObject);
  }
}
