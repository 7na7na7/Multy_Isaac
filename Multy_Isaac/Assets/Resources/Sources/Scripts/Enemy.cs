using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
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
  private Animator anim;
  private float localX;
  
  //길찾기
  public bool isFinding = false;
  public Transform targetPosition;
  public Seeker seeker;
  
  private void Start()
  { 
    rigid = GetComponent<Rigidbody2D>(); 
    flashwhite = GetComponent<FlashWhite>(); 
    anim = GetComponent<Animator>();
    localX = transform.localScale.x*-1;
    seeker = GetComponent<Seeker>();
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

    if(hp<=0)
      setTrigger("Die");
    else
      setTrigger("Hit");
    if (pos != Vector3.zero)
    {
      GetComponent<RegularZombie>().OnDisable();
      canMove = false;
      isFinding = false;
      
      Vector3 dir = (transform.position - pos).normalized;
      Vector2 d = GetComponent<Rigidbody2D>().velocity;
      rigid.velocity=Vector2.zero;
      rigid.DOMove(transform.position +dir * nuckBackDistance, nuckBackTime).SetEase(nuckBackEase).OnComplete(()=>
      {
        rigid.velocity = d;
        canMove = true;
        if (hp <= 0)
          Die();
        else
          setAnim("Walk");
      }); 
    }
    else
    {
      if (hp <= 0)
        Die();
      else
        setAnim("Walk");
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

  void Die()
  {
    for (int TemIndex = 0; TemIndex < ItemIndex.Length; TemIndex++)
    {
      if (Random.Range(1, 101) <= ItemPercent[TemIndex])
      {
        if (PhotonNetwork.OfflineMode)
          Instantiate(Resources.Load("item" + ItemIndex[TemIndex]),
            new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f),
              transform.position.y + Random.Range(-0.3f, 0.3f)), Quaternion.identity);
        else
          PhotonNetwork.InstantiateRoomObject("item" + ItemIndex[TemIndex],
            new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f),
              transform.position.y + Random.Range(-0.3f, 0.3f)), Quaternion.identity);
      }
    }

    if (PhotonNetwork.OfflineMode)
    {
      GameObject g = Instantiate(corpes, transform.position, Quaternion.identity);
            
      if (transform.localScale.x < 0)
        g.transform.localScale = new Vector3(g.transform.localScale.x * -1, g.transform.localScale.y, g.transform.localScale.z);
    }
    else
    {
      if (PhotonNetwork.IsMasterClient)
      {
        GameObject g=PhotonNetwork.InstantiateRoomObject(corpes.name, transform.position, Quaternion.identity);
        if (transform.localScale.x < 0)
          g.transform.localScale = new Vector3(g.transform.localScale.x * -1, g.transform.localScale.y, g.transform.localScale.z);
      }
    }


    if (PhotonNetwork.OfflineMode)
      destroyRPC();
    else
      pv.RPC("destroyRPC", RpcTarget.All);
  }
  public void setAnim(string animName)
  {
    if(PhotonNetwork.OfflineMode)
      animRPC(animName);
    else
      pv.RPC("animRPC",RpcTarget.All,animName);
  }

  public void setTrigger(string animName)
  {
    if(PhotonNetwork.OfflineMode)
      triggerRPC(animName);
    else
      pv.RPC("triggerRPC",RpcTarget.All,animName);
  }
  
  public void setLocalX(float x)
  {
    if(PhotonNetwork.OfflineMode)
      localScaleRPC(x);
    else
      pv.RPC("localScaleRPC",RpcTarget.All,x);
  }
  [PunRPC]
  void destroyRPC()
  {
    Destroy(gameObject);
  }
  
  
  [PunRPC]
  void animRPC(string animName)
  {
    try
    {
      anim.Play(animName);
    }
    catch (Exception e)
    { }
  }

  [PunRPC]
  void triggerRPC(string animName)
  {
    try
    {
      anim.SetTrigger(animName);
    }
    catch (Exception e)
    { }
  }
  
  [PunRPC]
  void localScaleRPC(float x)
  {
    if (x > transform.position.x) //오른쪽에있으면
    {
      transform.localScale=new Vector3(localX*-1,transform.localScale.y,transform.localScale.z);
    }
    else
    {
      transform.localScale=new Vector3(localX,transform.localScale.y,transform.localScale.z);
    }
  }
  
}
