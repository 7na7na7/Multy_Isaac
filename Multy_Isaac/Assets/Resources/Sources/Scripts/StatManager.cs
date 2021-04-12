using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    private offlineStat offStat;
    private float maxValue;
    private Player player;
    public int armor;

    public GameObject hpslider;

    private void Start()
    {
        offStat = GetComponent<offlineStat>();
        maxValue = GetComponent<offlineStat>().maxValue;
        player = transform.parent.GetComponent<Player>();
    }
    
    
    public void Heal(int value)
    {
        if(player.PlayerIndex==5)
            offStat.JustHungryHeal(value/2);
        if (value > 0)
        {
            if (maxValue - hpslider.transform.localScale.x/100 * maxValue < value) //회복량이 잃은체력보다 크면
                hpslider.transform.localScale=new Vector3(100,hpslider.transform.localScale.y,hpslider.transform.localScale.z);
            else
                hpslider.transform.localScale=new Vector3(hpslider.transform.localScale.x+value/maxValue*100,hpslider.transform.localScale.y,hpslider.transform.localScale.z);
        }
        else
        {
            hpslider.transform.localScale=new Vector3(hpslider.transform.localScale.x+value/maxValue*100,hpslider.transform.localScale.y,hpslider.transform.localScale.z);
        }
        player.hpSync(hpslider.transform.localScale.x);
    }
    public void JustHeal(int value)
    {
        if (value > 0)
        {
            if (maxValue - hpslider.transform.localScale.x/100 * maxValue < value) //회복량이 잃은체력보다 크면
                hpslider.transform.localScale=new Vector3(100,hpslider.transform.localScale.y,hpslider.transform.localScale.z);
            else
                hpslider.transform.localScale=new Vector3(hpslider.transform.localScale.x+value/maxValue*100,hpslider.transform.localScale.y,hpslider.transform.localScale.z);
        }
        else
        {
            hpslider.transform.localScale=new Vector3(hpslider.transform.localScale.x+value/maxValue*100,hpslider.transform.localScale.y,hpslider.transform.localScale.z);
        }
        player.hpSync(hpslider.transform.localScale.x);
    }
    public bool Hit(int value)  
    {
        float minusPer = 100 *((float)armor*1.5f / (armor*1.5f + 100f));
        float v = (value - (value * minusPer / 100f)) / maxValue;
        hpslider.transform.localScale = new Vector2(hpslider.transform.localScale.x - v*100, hpslider.transform.localScale.y);
        player.hpSync(hpslider.transform.localScale.x );
        if (hpslider.transform.localScale.x <= 0)
        {
            hpslider.transform.localScale=new Vector2(0,hpslider.transform.localScale.y);
            return true;   
        }
        else
            return false;
    }

    public bool LoseHp(int value)
    {
        hpslider.transform.localScale = new Vector2(hpslider.transform.localScale.x - value/maxValue*100, hpslider.transform.localScale.y);
        player.hpSync(hpslider.transform.localScale.x);
        if (hpslider.transform.localScale.x <= 0)
        {
            hpslider.transform.localScale=new Vector2(0,hpslider.transform.localScale.y);
            return true;   
        }
        else
            return false;
    }
    public int GetHp()
    {
        return (int) (maxValue * hpslider.transform.localScale.x);
    }
}
