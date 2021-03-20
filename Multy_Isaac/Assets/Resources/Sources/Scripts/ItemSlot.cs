using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int itemCount = 0;
    public Text itemCountTxt;
    public Invent invent;
    PlayerItem player;
    public int index;
    private Image img;
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

    private void Update()
    {
        if (itemCount > 0)
            itemCountTxt.text = itemCount.ToString();
        else
            itemCountTxt.text = "";
    }

    public void Drag()
    {
       img.transform.position = Input.mousePosition;
    }

    public void Down()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.ItemList[index].ItemName != "")
                {
                    Combine();
                }
        }
    }

    public void Combine()
    {
        invent.Open(player.ItemList[index]);
    }
}
