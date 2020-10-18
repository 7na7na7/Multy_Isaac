﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
[System.Serializable]
public class tem
{
    public int index=0;
    public string ItemName="";
    public Sprite ItemSprite=null;
    public itemType type = itemType.item;

    public void Clear()
    {
        index = 0;
        ItemName = "";
        ItemSprite = null;
        type = itemType.item;
    }
}
public enum itemType { Weapon, Guard, Food, item}
public class Item : MonoBehaviour
{
    public tem item;
    private PhotonView pv;
    public Material outlineMat;
    private Material defaultMat;
    private SpriteRenderer spr;
    public bool isGet = false;
    public Ease ease;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        spr = GetComponent<SpriteRenderer>();
        defaultMat = spr.material;
        StartCoroutine(YuraYura());
        spr.sprite = item.ItemSprite;
    }

    public void Destroy()
    {
        if(InGameNetwork.instance.isOffline)
            Destroy(gameObject);
        else
            pv.RPC("DestroyRPC", RpcTarget.All);
    }
    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }

    public bool canGet()
    {
        return isGet;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spr.material = outlineMat;
            isGet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spr.material = defaultMat;
            isGet = false;
        }
    }

    IEnumerator YuraYura()
    {
        Vector3 pos1 = transform.position + new Vector3(0, 0.12f, 0);
        Vector3 pos2 = transform.position + new Vector3(0, -0.12f, 0);
        while (true)
        {
            transform.DOMove(pos1,2f).SetEase(ease);
            yield return new WaitForSeconds(2f);
            transform.DOMove(pos2,2f).SetEase(ease);
            yield return new WaitForSeconds(2f);
        }
    }
}
