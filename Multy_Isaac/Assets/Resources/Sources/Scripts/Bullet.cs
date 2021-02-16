using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public int Dmg=10;
    public float nuckBackDistance;
    public Sprite none;
    public float speed = 5;
    public float DestroyTime = 1;
    private int dir;
    public PhotonView pv;
    private SpriteRenderer spr;

    private Vector3 savedLocalScale;
    void Start()
    {
        Destroy(gameObject,DestroyTime);
        spr = GetComponent<SpriteRenderer>();
        savedLocalScale = transform.localScale;
        transform.localScale=Vector3.zero;
        transform.DOScale(savedLocalScale, 0.1f);
    }
    
    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine && !pv.IsMine&&!other.GetComponent<Player>().isSuper)
            {
                other.GetComponent<Player>().Hit(Dmg,pv.Controller.NickName,nuckBackDistance,transform.position);
                spr.sprite = none;
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
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
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
            }
            else
            {
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            other.GetComponent<Enemy>().Hit(Dmg,transform.position, nuckBackDistance);
        }
        else if (other.gameObject.tag=="Wall")
        {
            if (PhotonNetwork.OfflineMode)
                Destroy(gameObject);
            else
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);   
        }
    }
    

    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
