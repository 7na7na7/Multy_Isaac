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

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void Destroy()
    {
        pv.RPC("DestroyRPC", RpcTarget.All);
    }
    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
