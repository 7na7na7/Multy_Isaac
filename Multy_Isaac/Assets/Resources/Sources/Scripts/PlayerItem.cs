using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
    
    private void Start()
    {
        player = GetComponent<Player>();
    }

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
        if (player.pv.IsMine)
        {
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

    
    public tem GetItem(int Index)
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

    public void DiscardItem(int index, int itemIndex)
    {
        if (ItemList[index] != null)
        {
            ItemList[index].Clear();
            player.pv.RPC("discardRPC",RpcTarget.All,"item"+itemIndex);   
        }
    }
    
    public void DeadDiscardItem(int index, int itemIndex) //랜덤으로 떨어짐
    {
        if (ItemList[index] != null)
        {
            ItemList[index].Clear();
            player.pv.RPC("DeadDiscardRPC",RpcTarget.All,"item"+itemIndex);   
        }
    }

    public void Dead()
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if(ItemList[i].index!=0) 
                DeadDiscardItem(i,ItemList[i].index);
        }
    }
    [PunRPC]
    void discardRPC(string itemName)
    {
        PhotonNetwork.InstantiateRoomObject(itemName, transform.position, quaternion.identity);
    }
    
    [PunRPC]
    void DeadDiscardRPC(string itemName)
    {
        PhotonNetwork.InstantiateRoomObject(itemName, 
            new Vector3(transform.position.x+UnityEngine.Random.Range(-1f,1f),transform.position.y+UnityEngine.Random.Range(-1f,1f),transform.position.z), quaternion.identity);
    }
}
