using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour//PunCallbacks, IPunObservable
{
  private Rigidbody2D rigid;
  public float nuckBackDistance;
  public GameObject corpes;
  public float nuckBackTime;
  public Ease nuckBackEase;
  public int[] ItemIndex;
  public int[] ItemPercent;
  private FlashWhite flashwhite;
  public int hp = 50;
  public PhotonView pv;
  public int CollsionDamage = 20;
  public float damageDelay = 1f;
  private float time;
  public bool canMove = true;
  private void Start()
  {
//    if (PhotonNetwork.OfflineMode)
//      Instantiate(effect, transform.position, quaternion.identity);
//    else
//    {
//      if(PhotonNetwork.IsMasterClient)
//        PhotonNetwork.InstantiateRoomObject(effect.name, transform.position, quaternion.identity);
//    }
    rigid = GetComponent<Rigidbody2D>();
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
  public void HitRPC(int value, Vector3 pos=default(Vector3),float nuckBackDistance=0)
  {
    flashwhite.Flash();
    hp -= value;
    
    if (pos != Vector3.zero)
    {
      canMove = false;
      Vector3 dir = (transform.position - pos).normalized;
      Vector2 d = GetComponent<Rigidbody2D>().velocity;
      rigid.velocity=Vector2.zero;
      rigid.DOMove(transform.position +dir * nuckBackDistance, nuckBackTime).SetEase(nuckBackEase).OnComplete(()=>
      {
        rigid.velocity = d;
        canMove = true;
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
        Instantiate(corpes, transform.position, Quaternion.identity);
        Destroy(gameObject); //죽어버리렴 ㅋ
      } }); 
    }
    else
    {
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
        
        if (PhotonNetwork.OfflineMode)
        {
          Instantiate(corpes, transform.position, Quaternion.identity);
        }
        else
        {
          if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.InstantiateRoomObject(corpes.name, transform.position, Quaternion.identity);
        }
        
        
        if (PhotonNetwork.OfflineMode)
          destroyRPC();
        else
          pv.RPC("destroyRPC",RpcTarget.All);
      }
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
          other.GetComponent<Player>().Hit(CollsionDamage, name.Substring(0, name.IndexOf("(")),nuckBackDistance,transform.position);
        else
          other.GetComponent<Player>().Hit(CollsionDamage, name,nuckBackDistance,transform.position);
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
            other.GetComponent<Player>().Hit(CollsionDamage, name.Substring(0, name.IndexOf("(")),nuckBackDistance,transform.position);
          else
            other.GetComponent<Player>().Hit(CollsionDamage, name,nuckBackDistance,transform.position);
        }
    }
  }

  [PunRPC]
  void destroyRPC()
  {
    Destroy(gameObject);
  }
}
