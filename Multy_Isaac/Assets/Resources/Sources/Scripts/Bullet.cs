using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed = 5;
    public float DestroyTime = 1;
    private int dir;
    public PhotonView pv;
    void Start()
    {
        Destroy(gameObject,DestroyTime);
    }
    
    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().pv.IsMine && !pv.IsMine)
            {
                other.GetComponent<Player>().Hit();
                Destroy(gameObject);
            }
            else if(!other.GetComponent<Player>().pv.IsMine && pv.IsMine)
            {
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (PhotonNetwork.OfflineMode)
            {
                other.GetComponent<Enemy>().HitRPC(10);
                Destroy(gameObject);
            }
            else
            {
                if (pv.IsMine)
                {
                    other.GetComponent<Enemy>().pv.RPC("HitRPC", RpcTarget.AllBuffered, 10);
                    pv.RPC("DestroyRPC", RpcTarget.AllBuffered);   
                }
                else
                {
                    pv.RPC("DestroyRPC", RpcTarget.AllBuffered);   
                }
            }
        }
        else if (other.CompareTag("Wall"))
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
