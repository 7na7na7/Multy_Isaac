﻿using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class PlayerItem : MonoBehaviour
{
    //아이템
    public float itemRadious;
    public LayerMask itemLayer;
    public tem[] ItemList;
    public Image[] ItemBoxes;
    public GameObject[] btns;
    private Player player;
    public Sprite NullSprite;
    public ItemSlot[] slots;
    public ItemData itemData;
    
    public void OtherBtnSetFalse(int index)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            if (i != index)
            {
                btns[i].SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (player != null)
        {
            if (player.pv.IsMine)
            {
                //print(ItemList[0].index+" "+ItemList[1].index+" "+ItemList[2].index+" "+ItemList[3].index+" "+ItemList[4].index+" "+ItemList[5].index);
            
                for (int i = 0; i < ItemList.Length; i++)
                {
                    if(ItemList[i].ItemSprite!=null) 
                        ItemBoxes[i].sprite = ItemList[i].ItemSprite;
                    else
                        ItemBoxes[i].sprite = NullSprite;
                }
                if (player.canMove)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Collider2D item = Physics2D.OverlapCircle(transform.position, itemRadious, itemLayer);
                        if (item != null)
                        {
                            if (item.GetComponent<Item>().canGet())
                            {
                                bool isGet = false;
                                for (int i = 0; i < ItemList.Length; i++)
                                {
                                    if (ItemList[i].ItemName == "")
                                    {
                                        isGet = true;
                                        ItemList[i]=item.GetComponent<Item>().item;
                                        item.GetComponent<Item>().Destroy();
                                        break;
                                    }
                                }
                                if(!isGet) 
                                    PopUpManager.instance.PopUp("더 이상 주울 수 없습니다!",Color.red);
                            
                            }
                        }   
                    }
                }
            }   
        }
        else
        {
            Player[] players = FindObjectsOfType<Player>();
            foreach (Player p in players)
            {
                if (p.pv.IsMine)
                {
                    player = p;
                    break;
                }
            }
        }
    }

    public void GetItem(tem item)
    {
        bool isGet = false;
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].ItemName == "")
            {
                isGet = true;
                ItemList[i] = item;
                break;
            }
        }

        if (!isGet)
        {
            PopUpManager.instance.PopUp("더 이상 만들 수 없습니다!", Color.red);
        }
    }
    public tem GetItemArray(int Index)
    {
        tem tem = new tem();
        foreach (tem item in ItemList)
        {
            if (item.index == Index)
            {
                tem = item;
                break;
            }
        }
        if (tem != null)
        {
            return tem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            tem.index = 0;
            return tem;
        }
    }

    public void DiscardItem(int index,bool isDead=false)
    {
        int ind = ItemList[index].index;
        ItemList[index].Clear();
        player.pv.RPC("discardRPC",RpcTarget.All,ind,isDead);
    }
    
    public void Dead()
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if(ItemList[i].index!=0) 
                player.pv.RPC("discardRPC",RpcTarget.All,ItemList[i].index,true);
            ItemList[i].Clear();
        }
    }

    [PunRPC]
    void discardRPC(int TemIndex, bool isDead = false)
    {
        Vector2 pos=new Vector2();
        if (isDead)
        {
            pos=new Vector2(transform.position.x+UnityEngine.Random.Range(-1f,1f),transform.position.y+UnityEngine.Random.Range(-1f,1f));
        }
        else
        {
            pos = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.2f, 0.2f),
                transform.position.y-1 + UnityEngine.Random.Range(-0.3f, 0.3f));

        }
        PhotonNetwork.InstantiateRoomObject("item"+TemIndex,pos , Quaternion.identity);

    }
    
}
