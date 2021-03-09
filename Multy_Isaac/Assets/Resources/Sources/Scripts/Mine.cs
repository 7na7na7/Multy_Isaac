using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float invisibleTime;
    public float delay;
    private bool canExplode = false;
    public GameObject explosion;

    private void Start()
    {
        StartCoroutine(mineCor());
    }

    void Explode()
    {
        if(PhotonNetwork.OfflineMode) 
            Instantiate(explosion, transform.position, quaternion.identity);
        else
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canExplode)
        {
            if (other.CompareTag("Player"))
            {
                Explode();
            }
        }
    }
}
