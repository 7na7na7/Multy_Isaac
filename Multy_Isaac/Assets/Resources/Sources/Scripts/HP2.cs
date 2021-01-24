using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP2 : MonoBehaviour
{
    public Slider hp, mp;
    Player player;

    private void Awake()
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

    void Update()
    {
        if (player != null)
        {
            hp.value = player.hp.value;
            hp.maxValue = player.hp.maxValue;
        }
    }
}
