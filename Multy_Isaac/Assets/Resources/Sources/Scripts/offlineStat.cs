using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class offlineStat : MonoBehaviour
{
    private StatManager stat;
    public float startSpeed;
    public float maxValue;
    public GameObject hpslider;
    public Image hp;
    public Image stomach;
    public Text hpTxt;
    public Text stomachTxt;
    Player player;
    private float one = 0;
    private float hungryLessSpeed;
    private float hungrySpeed;
    private bool canHungry = false;
    public void MaxHpUp(int value)
    {
        maxValue += value;
    }

    public void MaxHpDown(int value)
    {
        maxValue -= value;
    }
    private void Awake()
    {
        stat = GetComponent<StatManager>();
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
        if(SceneManager.GetActiveScene().name=="Play") 
            Invoke("canhun",FindObjectOfType<ZombieSpawner>().FirstDelay);
    }

    void canhun()
    {
        canHungry = true;
    }
    void Update()
    {
        if (player != null)
        {
            hp.fillAmount = hpslider.transform.localScale.x/100;
            hpTxt.text = ((int) (hpslider.transform.localScale.x/100*maxValue)).ToString();
            if (player.isPlay&&canHungry)
            {
                if (stomach.fillAmount > 0)
                {
                    if (player.rb.velocity != Vector2.zero)
                    {
                        if(player.speedValue()>0) 
                            stomach.fillAmount -= (player.speedValue()-startSpeed)*5/1000f * Time.deltaTime;
                    }
                
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
    }

    public void HungryHeal(int value)
    {
        if (value != 1)
        {
            if(player.PlayerIndex==5)
                stat.JustHeal(value/2);   
        }
        stomach.fillAmount += value/100f;
    }

    public void JustHungryHeal(int value)
    {
        stomach.fillAmount += value/100f;
    }
    public int getHungry()
    {
        return ((int) (100f * stomach.fillAmount));
    }
}