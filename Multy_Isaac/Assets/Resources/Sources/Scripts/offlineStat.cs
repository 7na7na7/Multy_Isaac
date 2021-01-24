using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offlineStat : MonoBehaviour
{
    public Image hp;
    public Image stomach;
    public Text hpTxt;
    public Text stomachTxt;
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
            hp.fillAmount = player.hp.value/player.hp.maxValue;
            hpTxt.text = ((int)player.hp.value).ToString();

            if (stomach.fillAmount > 0)
            {
                stomach.fillAmount -= player.hungrySpeed/100f * Time.deltaTime;
                stomachTxt.text = ((int) (100f * stomach.fillAmount)).ToString();   
            }
        }
    }

}
