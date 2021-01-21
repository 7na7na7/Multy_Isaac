using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour//PunCallbacks, IPunObservable
{
  public float nuckBackDistance;
  public GameObject corpes;
  public float nuckBackTime;
  public Ease nuckBackEase;
  public MonsterSpawner Spawner;
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
  public void HitRPC(int value, Vector3 pos=default(Vector3),float nuckBackDistance=0)
  {
    flashwhite.Flash();
    hp -= value;
    
    if (pos != Vector3.zero)
    {
      Vector3 dir = (GetComponent<SpriteRenderer>().bounds.center - pos).normalized;
      GetComponent<Rigidbody2D>().velocity=Vector2.zero;
      GetComponent<Rigidbody2D>().DOMove(GetComponent<SpriteRenderer>().bounds.center +dir * nuckBackDistance, nuckBackTime).SetEase(nuckBackEase).OnComplete(()=> {   if (hp <= 0)
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

        Spawner.Count++;
        Spawner.StartSpawnCor();
        Instantiate(corpes, transform.position, Quaternion.identity);
        Destroy(gameObject); //죽어버리렴 ㅋ
      } });   ;
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

        Spawner.Count++;
        Spawner.StartSpawnCor();
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
