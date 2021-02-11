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
  private List<GameObject> dropTemList;
  public Animator Exclamation;
  private Rigidbody2D rigid;
  public float nuckBackDistance;
  public float nuckBackTime;
  public Ease nuckBackEase;
  public int TemCount;
  public GameObject[] DropTem;
  public int[] DropTemPercent;
  public GameObject[] DropExp;
  public int[] DropExpCount;
  private FlashWhite flashwhite;
  public int hp = 50;
  public PhotonView pv;
  public int CollsionDamage = 20;
  public float damageDelay = 1f;
  private float time;
  public bool canMove = true;
  private Animator anim;
  private float localX;
  private TemManager temMgr;
  
  //길찾기
  public bool isFinding = false;
  public Transform targetPosition;
  public Seeker seeker;
  
  public void ExclamationOpen()
  {
    if (PhotonNetwork.OfflineMode)
    {
      Open();
    }
    else
    {
      pv.RPC("Open",RpcTarget.All);
    }
  }

  public void ExclamationClose()
  {
    if (PhotonNetwork.OfflineMode)
    {
      Close();
    }
    else
    {
      pv.RPC("Close",RpcTarget.All);
    }
  }

  [PunRPC]
  void Open()
  {
    Exclamation.Play("Open");
  }
  
  [PunRPC]
  void Close()
  {
    Exclamation.Play("Close");
  }
  
  private void Start()
  { 
    rigid = GetComponent<Rigidbody2D>(); 
    flashwhite = GetComponent<FlashWhite>(); 
    anim = GetComponent<Animator>();
    localX = transform.localScale.x*-1;
    seeker = GetComponent<Seeker>();
    dropTemList=new List<GameObject>();
    temMgr = FindObjectOfType<TemManager>();
    for (int i = 0; i < DropTem.Length; i++)
    {
      for (int j = 0; j < DropTemPercent[i]; j++)
      {
        dropTemList.Add(DropTem[i]);
      }
    }
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
        {
         if(PhotonNetwork.OfflineMode)
           Die();
         else
           pv.RPC("Die",RpcTarget.All);
        }
        else
          setAnim("Walk");
      }); 
    }
    else
    {
      if (hp <= 0)
      {
        if(PhotonNetwork.OfflineMode)
          Die();
        else
          pv.RPC("Die",RpcTarget.All);
      }
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

  [PunRPC]
  void Die()
  {
    for (int i = 0; i < DropExp.Length; i++)
    {
      for (int j = 0; j < Random.Range(0,DropExpCount[i]+1); j++)
      {
        if (PhotonNetwork.OfflineMode)
          Instantiate(DropExp[i], new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + Random.Range(-0.3f, 0.3f)), Quaternion.identity);
        else
          PhotonNetwork.InstantiateRoomObject(DropExp[i].name, new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + Random.Range(-0.3f, 0.3f)), Quaternion.identity);
 
      }
    }

    for (int i = 0; i < Random.Range(0,TemCount+1); i++)
    {
      temMgr.setTem(dropTemList[Random.Range(0,dropTemList.Count)].GetComponent<Item>().item.index,
        new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + Random.Range(-0.3f, 0.3f))); 
    }
    
    transform.GetChild(0).gameObject.SetActive(true);
transform.GetChild(0).transform.parent = null;

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

  public void setPlayer(Transform tr)
  {
    GetComponent<RegularZombie>().stopCor();
    isFinding = true;
    targetPosition = tr; 
    ExclamationOpen();
    setAnim("Walk");
  }
}
