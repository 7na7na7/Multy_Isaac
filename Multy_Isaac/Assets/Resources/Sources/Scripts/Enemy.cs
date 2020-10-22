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
  public float damageDelay = 1f;
  private float time;
  private void Start()
  {
    flashwhite = GetComponent<FlashWhite>();
  }

  private void Update()
  {
    if (time < damageDelay)
    {
      time += Time.deltaTime;
    }
  }

  [PunRPC]
  public void HitRPC(int value)
  {
    flashwhite.Flash();
    hp -= value;
    if(hp<=0)
      Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (time >= damageDelay)
      {
        time = 0;
        other.GetComponent<Player>().Hit(CollsionDamage, name.Substring(0, name.IndexOf("(")));
      }
    }
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (time >= damageDelay)
      {
          time = 0;
          other.GetComponent<Player>().Hit(CollsionDamage, name.Substring(0, name.IndexOf("(")));
        }
    }
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
