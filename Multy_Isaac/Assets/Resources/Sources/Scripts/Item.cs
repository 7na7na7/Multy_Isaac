using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum itemType { Weapon, Guard, Food, item}
public class Item : MonoBehaviour
{
    private PhotonView pv;
    public string ItemName=null;
    public Sprite ItemSprite=null;
    public itemType type = itemType.item;
    public Material outlineMat;
    private Material defaultMat;
    private SpriteRenderer spr;
    public bool isGet = false;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        spr = GetComponent<SpriteRenderer>();
        defaultMat = spr.material;
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
}
