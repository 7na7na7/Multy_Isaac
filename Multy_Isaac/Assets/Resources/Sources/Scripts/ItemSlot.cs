using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Invent invent;
    PlayerItem player;
    public int index;
    private Image img;
    public GameObject Btns;
    private void Start()
    {
        img = GetComponent<Image>();
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.pv.IsMine)
            {
                player = p.GetComponent<PlayerItem>();
                break;
            }
        }  
    }

    public void Drag()
    {
       img.transform.position = Input.mousePosition;
//       print(img.transform.position);
    }

    public void DragEnd()
    {
        print(img.transform.position);
    }

    public void Down()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (player.ItemList[index].ItemName != "")
                {
                    if(!Btns.activeSelf)
                        player.OtherBtnSetFalse(index);
                    Btns.SetActive(!Btns.activeSelf);   
                }
        }
    }

    public void Discard()
    {
       
            player.DiscardItem(index,player.ItemList[index].index);
            Btns.SetActive(false);   
       
    }

    public void Combine()
    {
        invent.Open(player.ItemList[index].index);
        Btns.SetActive(false);   
        
    }
    public void Up()
    {
        
    }
}
