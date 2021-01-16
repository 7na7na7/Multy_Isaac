using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour//PunCallbacks, IPunObservable
{
  public int[] ItemIndex;
  public int[] ItemPercent;
  private FlashWhite flashwhite;
  public int hp = 50;
  public PhotonView pv;
  public int CollsionDamage = 20;
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
    if (hp <= 0)
    {
      for (int TemIndex=0;TemIndex<ItemIndex.Length;TemIndex++)
      {
        if (Random.Range(1, 101) <= ItemPercent[TemIndex])
        { 
          if(PhotonNetwork.OfflineMode) 
            Instantiate(Resources.Load("item"+ItemIndex[TemIndex]),new Vector3(transform.position.x+Random.Range(-0.3f,0.3f),transform.position.y+Random.Range(-0.3f,0.3f)) , Quaternion.identity);
          else
            PhotonNetwork.InstantiateRoomObject("item"+ItemIndex[TemIndex],new Vector3(transform.position.x+Random.Range(-0.3f,0.3f),transform.position.y+Random.Range(-0.3f,0.3f)) , Quaternion.identity);
        }
      }

      Destroy(gameObject); 
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (time >= damageDelay)
      {
        time = 0;
        if(name.Contains("(")) 
          other.GetComponent<Player>().Hit(CollsionDamage, name.Substring(0, name.IndexOf("(")),transform.position);
        else
          other.GetComponent<Player>().Hit(CollsionDamage, name,transform.position);
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
          if(name.Contains("(")) 
            other.GetComponent<Player>().Hit(CollsionDamage, name.Substring(0, name.IndexOf("(")),transform.position);
          else
            other.GetComponent<Player>().Hit(CollsionDamage, name,transform.position);
        }
    }
  }
}
