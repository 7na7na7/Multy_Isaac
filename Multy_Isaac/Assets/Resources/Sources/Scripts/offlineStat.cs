using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class offlineStat : MonoBehaviour
{
    public float maxValue;
    public GameObject hpslider;
    public Image hp;
    public Image stomach;
    public Text hpTxt;
    public Text stomachTxt;
    Player player;
    private float one = 0;
    private int hungryLessSpeed;
    private int hungrySpeed;
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
        hungrySpeed = player.hungrySpeed;
        hungryLessSpeed = player.hungryLessHpSpeed;
    }

    void Update()
    {
        if (player != null)
        {
            hp.fillAmount = hpslider.transform.localScale.x/100;
            hpTxt.text = ((int) (hpslider.transform.localScale.x/100*maxValue)).ToString();

            if (stomach.fillAmount > 0)
            {
                stomach.fillAmount -= hungrySpeed/1000f * Time.deltaTime;
                stomachTxt.text = Mathf.CeilToInt(100f * stomach.fillAmount).ToString();   
            }
            else
            {
                one+= hungryLessSpeed/10f * Time.deltaTime;
                if (one >= 1f)
                {
                    one = 0;
                    player.loseHP();   
                }
            }
        }
    }

    public void HungryHeal(int value)
    {
        stomach.fillAmount += value/100f;
    }

    public int getHungry()
    {
        return ((int) (100f * stomach.fillAmount));
    }
}