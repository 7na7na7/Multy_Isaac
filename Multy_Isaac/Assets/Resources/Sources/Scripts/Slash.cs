using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Slash : MonoBehaviourPunCallbacks
{
    public int Dmg;
    public float nuckBackDistance;
    private bool canDmg = true;
    public float DestroyTime = 1;
    public PhotonView pv;
    void Start()
    {
        transform.position=new Vector3(transform.position.x,transform.position.y);
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
                    other.GetComponent<Player>().Hit(Dmg,pv.Controller.NickName,nuckBackDistance,transform.position);
                    canDmg = false;
                }
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Hit(Dmg,transform.position,nuckBackDistance);
        }
    }
    

    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
