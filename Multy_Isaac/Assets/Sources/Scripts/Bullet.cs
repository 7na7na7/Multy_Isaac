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
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            else if(!other.GetComponent<Player>().pv.IsMine && pv.IsMine)
            {
                StartCoroutine(delayDestroy());
            }
        }
    }

    IEnumerator delayDestroy()
    {
        yield return new WaitForSeconds(0.05f);
        pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
