using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    //아이템
    public float itemRadious;
    public LayerMask itemLayer;
    private List<Item> ItemList = new List<Item>();
    public Image[] ItemBoxes;
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.pv.IsMine)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if(ItemList[i].ItemSprite!=null) 
                    ItemBoxes[i].sprite = ItemList[i].ItemSprite;
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
                            if (ItemList.Count < 6)
                            {
                                GetItem(item.GetComponent<Item>());
                                item.GetComponent<Item>().Destroy();   
                            }
                            else
                            {
                                PopUpManager.instance.PopUp("더 이상 주울 수 없습니다!",Color.red);
                            }
                        }
                    }   
                }
            }
        }
    }
    
    public bool GetItem(Item item)
    {
        if (ItemList.Count < 6)
        {
            ItemList.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }
}
