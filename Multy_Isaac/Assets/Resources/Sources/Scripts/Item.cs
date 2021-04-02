using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class tem
{
    public int Count = 1;
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
        Copytem.Count = this.Count;
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
    public GameObject name;
    public Text txt;
    public int price;
    public int[] prices;
    public int[] shopIndexes;
    private TemManager temMgr;
    public bool isShopTem = false;
    private PhotonView pv;
    public int Index;
    public tem item;
    public Material outlineMat;
    private Material defaultMat;
    private SpriteRenderer spr;
    private GameObject NameCan;

    void setName()
    {
        NameCan = transform.GetChild(2).gameObject; 
        NameCan.transform.GetChild(0).GetComponent<Text>().text = item.ItemName;
    }
    private void Start()
    {
        Instantiate(name, transform);
        NameCan = transform.GetChild(2).gameObject; 
        NameCan.transform.GetChild(0).GetComponent<Text>().fontSize = (int)(NameCan.transform.GetChild(0).GetComponent<Text>().fontSize*(1/transform.localScale.x));
        if(shopIndexes.Length<=0)
            setName();
        else
        {
            NameCan.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition=
               new Vector2(-960f,-540.82f);
        }
        spr = GetComponent<SpriteRenderer>();
        defaultMat = spr.material;
        spr.sprite = item.ItemSprite;
        item.ItemSprite = spr.sprite;
        if (isShopTem)
        {
            pv = GetComponent<PhotonView>();
            temMgr = FindObjectOfType<TemManager>();
            Invoke("Del",5);
        }
        
    }

    public bool canGet()
    {
        if (spr.material == defaultMat)
            return false;
        else
            return true;
    }

    public void Del()
    {
        if (PhotonNetwork.OfflineMode)
        {
            delRPC(Random.Range(0, shopIndexes.Length));
        }
        else
        {
            pv.RPC("delRPC",RpcTarget.All,Random.Range(0, shopIndexes.Length));
        }
    }

    [PunRPC]
    void delRPC(int temIndex)
    {
        item = temMgr.GetItemList(shopIndexes[temIndex]);
        spr.sprite = item.ItemSprite;
        NameCan.transform.GetChild(0).GetComponent<Text>().text = item.ItemName;
        price = prices[temIndex];
        txt.text = "X" +price;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NameCan.SetActive(true);
            spr.material = outlineMat;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NameCan.SetActive(false);
            spr.material = defaultMat;
        }
    }
}
