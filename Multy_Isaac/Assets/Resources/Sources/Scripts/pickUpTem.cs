using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class pickUpTem : MonoBehaviour
{
    public float speed;
    public int ExpAmount = 1;
    public float radius;
    public LayerMask player;



    void Update()
    {
        //자신 기준으로 radius반경의 plaeyer탐색
        Collider2D col = Physics2D.OverlapCircle(transform.position, radius, player);
        if (col != null) //플레이어가 비지 않았다면
        {
            transform.position=Vector2.Lerp(transform.position, col.transform.position, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<Player>().getEXP(ExpAmount);
                
                if(PhotonNetwork.OfflineMode)
                    Destroy(gameObject);
                else
                    GetComponent<PhotonView>().RPC("punDestroy",RpcTarget.AllBuffered);     
            }
        }
    }

    [PunRPC]
    void punDestroy()
    {
        Destroy(gameObject);
    }
}
