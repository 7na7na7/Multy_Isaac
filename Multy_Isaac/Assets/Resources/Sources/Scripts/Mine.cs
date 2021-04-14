using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public bool isMine = true;
    public float invisibleTime;
    public float delay;
    public bool canExplode = false;
    public GameObject explosion;

    private void Start()
    {
        StartCoroutine(mineCor());
    }

    void Explode()
    {
        if(!PhotonNetwork.OfflineMode)
        {
            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(explosion.name, transform.position, quaternion.identity);
        }
        Destroy(gameObject);
    }

    [PunRPC]
    void can()
    {
        canExplode = true;
    }

    IEnumerator mineCor()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().DOColor(Color.clear, invisibleTime);
        yield return new WaitForSeconds(invisibleTime);
        if(PhotonNetwork.OfflineMode)
            can();
        else
            GetComponent<PhotonView>().RPC("can",RpcTarget.All);
    }

    public void DestroyRPC()
    {
        GetComponent<PhotonView>().RPC("destroyRPC",RpcTarget.All);
    }

    [PunRPC]
    void destroyRPC()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canExplode)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<PhotonView>().IsMine && GetComponent<PhotonView>().IsMine)
                {
                    
                }
                else
                {
                    if (isMine)
                    {
                        Explode();
                    }
                }
                
            }
        }
    }
}
