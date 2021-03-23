using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    private Player player;
    public int armor;

    public Slider hpSlider;

    private void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    public void Heal(int value)
    {
        if (value > 0)
        {
            if (hpSlider.maxValue - hpSlider.value < value) //회복량이 잃은체력보다 크면
                hpSlider.value = hpSlider.maxValue;
            else
                hpSlider.value += value;
        }
        else
        {
            hpSlider.value += value;
        }
        player.hpSync(hpSlider.value);
    }

    public bool Hit(int value)  
    {
        float minusPer = 100 *((float)armor*1.5f / (armor*1.5f + 100f));
        hpSlider.value -= value-(value * minusPer / 100f);
        player.hpSync(hpSlider.value);
        if (hpSlider.value <= 0)
            return true;
        else
            return false;
    }

    public bool LoseHp(int value)
    {
        hpSlider.value -= value;
        player.hpSync(hpSlider.value);
        if (hpSlider.value <= 0)
            return true;
        else
            return false;
    }
    public int GetHp()
    {
        return (int)hpSlider.value;
    }
}
