using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
[System.Serializable]
public class tem
{
    public int weaponIndex;
    public Sprite NoneSprite;
    public bool canSee = true;
    public int[] SmallItemIndex; //하위템
    public int[] CompleteItemIndex; //상위템
    public int index=0;
    public string ItemName="";
    public Sprite ItemSprite=null;
    public itemType type = itemType.Item;
    public string ItemDescription = "";
    public string WhereGet = "";
    public void Clear()
    {
        weaponIndex = -1;
        CompleteItemIndex=new int[0];
        index = 0;
        ItemName = "";
        ItemSprite = NoneSprite;
        type = itemType.Item;
        ItemDescription="";
    }
    public tem DeepCopy()
    {
        tem Copytem = new tem();
        Copytem.weaponIndex = this.weaponIndex;
        Copytem.NoneSprite = this.NoneSprite;
        Copytem.canSee = this.canSee;
        Copytem.SmallItemIndex = this.SmallItemIndex;
        Copytem.CompleteItemIndex = this.CompleteItemIndex;
        Copytem.index = this.index;
        Copytem.ItemName = this.ItemName;
        Copytem.ItemSprite = this.ItemSprite;
        Copytem.type = this.type;
        Copytem.ItemDescription = this.ItemDescription;
        Copytem.WhereGet = this.WhereGet;

        return Copytem;
    }
}
public enum itemType { Gun,Melee,Item,Passive,Usable}
public class Item : MonoBehaviour
{
    public int Index;
    public tem item;
    private PhotonView pv;
    public Material outlineMat;
    private Material defaultMat;
    private SpriteRenderer spr;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        spr = GetComponent<SpriteRenderer>();
        defaultMat = spr.material;
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
        if (spr.material == defaultMat)
            return false;
        else
            return true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spr.material = outlineMat;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spr.material = defaultMat;
        }
    }
}
