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

public class Enemy : MonoBehaviour //PunCallbacks, IPunObservable
{
  public SoundManager sound;
  private Zombie zombie;
  public float fireDamageTick=0.1f;
  private float fireTime=0;
  private bool isDead = false;
  public Animator Exclamation;
  private Rigidbody2D rigid;
  public float nuckBackDistance;
  public float nuckBackTime;
  public Ease nuckBackEase;
  public GameObject[] DropTem;
  public int[] DropTemPercent;

  private FlashWhite flashwhite;
  public int hp = 50;
  public PhotonView pv;
  public int CollsionDamage = 20;
  public float damageDelay = 1f;
  public float time;
  public bool canMove = true;
  private Animator anim;
  private float localX;
  private TemManager temMgr;

  //길찾기
  public bool isFinding = false;
  public Transform targetPosition;
  public Seeker seeker;

  public void ExclamationOpen() //느낌표열기 
  {
    if (PhotonNetwork.OfflineMode)
    {
      Open();
    }
    else
    {
      pv.RPC("Open", RpcTarget.All);
    }
  }

  public void ExclamationClose() //느낌표닫기
  {
    if (PhotonNetwork.OfflineMode)
    {
      Close();
    }
    else
    {
      pv.RPC("Close", RpcTarget.All);
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
    localX = transform.localScale.x * -1;
    seeker = GetComponent<Seeker>();
    temMgr = FindObjectOfType<TemManager>();
    zombie = GetComponent<Zombie>();
  }

  private void Update()
  {
    if (time < damageDelay)
    {
      time += Time.deltaTime;
    }
    if (fireTime > 0)
      fireTime -= Time.deltaTime;
  }

  public void Hit(int value, Vector3 pos = default(Vector3), float nuckBackDistance = 0, Player.bulletType type=Player.bulletType.common)
  {
    sound.Play(1,true,0.3f);
    if (PhotonNetwork.OfflineMode)
      HitRPC(value, pos, nuckBackDistance,type);
    else
    {
      if (PhotonNetwork.IsMasterClient)
        pv.RPC("HitRPC", RpcTarget.All, value, pos, nuckBackDistance,type);
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Explosion")) //폭탄
    {
      DelayDestroy enemy = other.GetComponent<DelayDestroy>();
      Hit(enemy.damage, enemy.transform.position,enemy.nuckBackDistance);
    }
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Fire"))
    {
      if (fireTime <=0)
      {
        LoseHP(other.GetComponent<DelayDestroy>().damage);
        fireTime = fireDamageTick;
      }
    }
  }

  void LoseHP(int value)
  {
    if(PhotonNetwork.OfflineMode)
      LoseHPRPC(value);
    else
      pv.RPC("LoseHPRPC",RpcTarget.All,value*2);
  }
  [PunRPC]
  void LoseHPRPC(int value)
  {
    hp -= value;
    flashwhite.Flash();
    if (hp <= 0)
    {
      Die();
    }
  }
  [PunRPC]
  void HitRPC(int value, Vector3 pos = default(Vector3), float nuckBackDistance = 0,Player.bulletType type=Player.bulletType.common)
  {
    flashwhite.Flash();
    hp -= value;

    if (hp <= 0)
      setTrigger("Die");
    else
      setTrigger("Hit");
    
    if (pos != Vector3.zero)
    {
      zombie.OnDisable();
      canMove = false;
      isFinding = false;

      Vector3 dir = (transform.position - pos).normalized;
      Vector2 savedVel = GetComponent<Rigidbody2D>().velocity;
      rigid.velocity = Vector2.zero;
      if (type == Player.bulletType.electric)
      {
        rigid.DOMove(transform.position, nuckBackTime*2).SetEase(nuckBackEase).OnComplete(() =>
        {
          rigid.velocity = savedVel;
          canMove = true;
          if (hp <= 0)
          {
            Die();
          }
          else
          {
            if (zombie.zombieIndex == 1 || zombie.zombieIndex == 3 || zombie.zombieIndex == 4)
            {
              setAnim("Walk");
            }
            else if (zombie.zombieIndex == 2)
            {
              setAnim("Idle");
            }
          }
          if(zombie.zombieIndex==2)
            zombie.stopPoison();
        });
      }
      else
      {
        rigid.DOMove(transform.position + dir * nuckBackDistance, nuckBackTime).SetEase(nuckBackEase).OnComplete(() =>
        {
          rigid.velocity = savedVel;
          canMove = true;
          if (hp <= 0)
          {
            Die();
          }
          else
          {
            if (zombie.zombieIndex == 1 || zombie.zombieIndex == 3 || zombie.zombieIndex == 4)
            {
              setAnim("Walk");
            }
            else if (zombie.zombieIndex == 2)
            {
              setAnim("Idle");
            }
          }
          if(zombie.zombieIndex==2)
            zombie.stopPoison();
        }); 
      }

      zombie.Detect(15);
    }
    else
    {
      if (hp <= 0)
        {
          Die();
        }
        else
      {
        if (zombie.zombieIndex == 1 || zombie.zombieIndex == 3 || zombie.zombieIndex == 4)
        {
          setAnim("Walk");
        }
        else if (zombie.zombieIndex == 2)
        {
          setAnim("Idle");
        }
      }
    }
  }

  bool percentreturn(int per)
  {
    if (Random.Range(1, 101) <= per)
      return true;
    else
      return false;
  }

  void Die()
  {
    if(PhotonNetwork.OfflineMode)
      DieRPC();
    else
    {
      pv.RPC("DieRPC",RpcTarget.AllBuffered);
    }
  }
  [PunRPC]
  void DieRPC()
  {
    if (!isDead)
    {
      isDead = true;

      for (int i = 0; i < DropTem.Length; i++)
      {
        if (percentreturn(DropTemPercent[i]))
        {
          if(DropTem[i].GetComponent<Item>()!=null) 
            temMgr.setTem(DropTem[i].GetComponent<Item>().item.index, new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f))); 
          else
            temMgr.setBullet(DropTem[i].GetComponent<pickUpTem>().subIndex, new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f))); 
        }
      }
      zombie.StopAllCor();
      transform.GetChild(0).gameObject.SetActive(true);
      transform.GetChild(0).transform.parent = null;
      GetComponent<SpriteRenderer>().sprite = null;
      GetComponent<Collider2D>().enabled = false;
      Destroy(GetComponent<PhotonAnimatorView>());
      Destroy(GetComponent<Animator>());
      canMove = false;
      rigid.velocity=Vector2.zero;
      ExclamationClose();
      sound.Play(2,true,0.1f);
//if (PhotonNetwork.OfflineMode)
      destroyRPC();
//    else
//      pv.RPC("destroyRPC", RpcTarget.All); 
    }
  }

  public void setAnim(string animName)
  {
    if (PhotonNetwork.OfflineMode)
      animRPC(animName);
    else
      pv.RPC("animRPC", RpcTarget.All, animName);
  }

  public void setTrigger(string animName)
  {
    if (PhotonNetwork.OfflineMode)
      triggerRPC(animName);
    else
      pv.RPC("triggerRPC", RpcTarget.All, animName);
  }

  public void setLocalX(float x)
  {
    if (PhotonNetwork.OfflineMode)
      localScaleRPC(x);
    else
      pv.RPC("localScaleRPC", RpcTarget.All, x);
  }

  [PunRPC]
  void destroyRPC()
  {
    Destroy(gameObject,1.5f);
  }


  [PunRPC]
  void animRPC(string animName)
  {
    try
    {
      anim.Play(animName);
    }
    catch (Exception e)
    {
    }
  }

  [PunRPC]
  void triggerRPC(string animName)
  {
    try
    {
      anim.SetTrigger(animName);
    }
    catch (Exception e)
    {
    }
  }

  [PunRPC]
  void localScaleRPC(float x)
  {
    if (x > transform.position.x) //오른쪽에있으면
    {
      transform.localScale = new Vector3(-1*localX, -1*localX, 1);
    }
    else
    {
      transform.localScale = new Vector3(localX, -1*localX, 1);
    }
  }

  public void setPlayer(Transform tr)
  {
    zombie.stopCor();
    isFinding = true;
    targetPosition = tr;
    ExclamationOpen();
    if (zombie.zombieIndex == 1 || zombie.zombieIndex == 3 || zombie.zombieIndex == 4)
    {
      setAnim("Walk");
    }
    else if (zombie.zombieIndex == 2)
    {
      setAnim("Idle");
    }
  }
  
  public void setPlayerRPC(int viewId)
  {
    pv.RPC("SPR",RpcTarget.All,viewId);
  }

  [PunRPC]
  void SPR(int viewId)
  {
    if (PhotonNetwork.IsMasterClient)
    {
      setPlayer(PhotonView.Find(viewId).transform);
    }
  }
}