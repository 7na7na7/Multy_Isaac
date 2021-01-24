using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class Roof : MonoBehaviour
{
    public float alpha = 0.01f;
    private SpriteRenderer spr;
   
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }


    void invisible()
    {
        Color color = spr.color;
        color.a = alpha;
        spr.DOColor(color, 1f);
    }

    void unInvisible()
    {
        Color color = spr.color;
        color.a = 1f;
        spr.DOColor(color, 1f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<PhotonView>().IsMine) 
                invisible();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<PhotonView>().IsMine) 
                unInvisible();
        }
    }
}
