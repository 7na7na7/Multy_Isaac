using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public Player.bulletType type=Player.bulletType.common;
    public int Dmg=10;
    public float nuckBackDistance;
    public Sprite none;
    public float speed = 5;
    public float DestroyTime = 1;
    private int dir;
    public PhotonView pv;
    private SpriteRenderer spr;
    public GameObject wallEffect;
    public GameObject hitEffect;
    private Vector3 savedLocalScale;
    void Start()
    {
        Invoke("DestroyWall",DestroyTime);
        spr = GetComponent<SpriteRenderer>();
        savedLocalScale = transform.localScale;
        transform.localScale=Vector3.zero;
        transform.DOScale(savedLocalScale, 0.1f);
        if (type == Player.bulletType.snow)
        {
            StartCoroutine(snowDmgUpCor());
        }
    }
    
    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
        if (type == Player.bulletType.snow)
        {
            transform.localScale+=Vector3.one*Time.deltaTime*7f;
            speed -= Time.deltaTime*1.5f;
        }
    }

    IEnumerator snowDmgUpCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            Dmg++;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine && !pv.IsMine&&!other.GetComponent<Player>().isSuper)
            {
                other.GetComponent<Player>().Hit(Dmg,pv.Controller.NickName,nuckBackDistance,transform.position,type);
                spr.sprite = none;
               DestroyHit();
            }
            else if (other.GetComponent<PhotonView>().IsMine && pv.IsMine)
            {
                
            }
            else
            {
                if (!other.GetComponent<Player>().isSuper &&pv.IsMine)
                    spr.sprite = none;
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Hit(Dmg,transform.position, nuckBackDistance,type);
            DestroyHit();
        }
        else if (other.gameObject.tag=="Wall")
        {
           DestroyWall();
        }
    }

    public void DestroyWall()
    {
        if(PhotonNetwork.OfflineMode)
            DestroyRPC();
        else
            pv.RPC("DestroyRPC", RpcTarget.All);   
    }
    [PunRPC]
   void DestroyRPC()
    {
        GameObject go=Instantiate(wallEffect, transform.position, Quaternion.identity);
        if(type==Player.bulletType.snow) 
            go.transform.localScale = transform.localScale/2;
        Destroy(gameObject);
    }
   
   public void DestroyHit()
   {
       if(PhotonNetwork.OfflineMode)
           DestroyRPC2();
       else
           pv.RPC("DestroyRPC2", RpcTarget.All);   
   }
   [PunRPC]
   void DestroyRPC2()
   {
       GameObject go=Instantiate(hitEffect, transform.position, Quaternion.identity);
       if(type==Player.bulletType.snow) 
           go.transform.localScale = transform.localScale/2;
       Destroy(gameObject);
   }
}
