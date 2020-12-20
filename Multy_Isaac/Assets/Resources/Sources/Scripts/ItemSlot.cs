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
    }

    public void Down()
    {
        if (Input.GetMouseButtonDown(1))
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
