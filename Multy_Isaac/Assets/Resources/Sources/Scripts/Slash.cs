using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Slash : MonoBehaviourPunCallbacks
{
    private bool canDmg = true;
    public float DestroyTime = 1;
    public PhotonView pv;
    void Start()
    {
        Destroy(gameObject,DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine && !pv.IsMine&&!other.GetComponent<Player>().isSuper)
            {
                if (canDmg)
                {
                    other.GetComponent<Player>().Hit(30,pv.Controller.NickName,transform.position);
                    canDmg = false;
                }
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (PhotonNetwork.OfflineMode)
            {
                other.GetComponent<Enemy>().HitRPC(10);
            }
            else
            {
                if (pv.IsMine)
                {
                    other.GetComponent<Enemy>().pv.RPC("HitRPC", RpcTarget.AllBuffered, 10);
                }
            }
        }
    }
    

    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
