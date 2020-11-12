using System;
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
    public GameObject[] Selected;
    
    private int selectedIndex = 1;
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

    private void Start()
    {
        Selected[0].SetActive(true);
    }
    
    private void Update()
    {
        if (player != null)
        {
            if (player.pv.IsMine)
            {
                //print(ItemList[0].index+" "+ItemList[1].index+" "+ItemList[2].index+" "+ItemList[3].index+" "+ItemList[4].index+" "+ItemList[5].index);
            
                for (int i = 0; i < ItemList.Length; i++) //아이템이미지가 존재한다면 매 프레임마다 박스에 이미지 갱신
                {
                    if(ItemList[i].ItemSprite!=null) 
                        ItemBoxes[i].sprite = ItemList[i].ItemSprite;
                    else
                        ItemBoxes[i].sprite = NullSprite;
                }
                
                if (player.canMove) //움직일 수 있는 상태에서만 입력 가능
                {
                    if (Input.GetKeyDown(KeyCode.Space)) //스페이스바로 줍기
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
                    
                    //1부터 6으로 아이템 선택 가능
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) ||
                        Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) ||
                        Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha1)) //1
                                selectedIndex = 1;
                            else if (Input.GetKeyDown(KeyCode.Alpha2)) //2
                                selectedIndex = 2;
                            else if (Input.GetKeyDown(KeyCode.Alpha3)) //3
                                selectedIndex = 3;
                            else if (Input.GetKeyDown(KeyCode.Alpha4)) //4
                                selectedIndex = 4;
                            else if (Input.GetKeyDown(KeyCode.Alpha5)) //5
                                selectedIndex = 5;
                            else if (Input.GetKeyDown(KeyCode.Alpha6)) //6
                                selectedIndex = 6;

                            for (int i = 0; i < Selected.Length; i++) //현재 인텍스에만 선택창 달아줌
                            {
                                if (i == selectedIndex - 1)
                                    Selected[i].SetActive(true);
                                else
                                    Selected[i].SetActive(false); 
                            }

                            if (ItemList[selectedIndex - 1].ItemSprite != NullSprite)
                            {
                                if (ItemList[selectedIndex-1].type == itemType.Weapon) 
                                { 
                                    player.changeWeapon(itemData.GetWeapon(ItemList[selectedIndex-1].weaponIndex)); 
                                }
                                else
                                {
                                    player.gunSetfalse();
                                }
                            }
//                            else
//                            {
//                                player.gunSetfalse();
//                            }
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
