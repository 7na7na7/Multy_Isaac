using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public int startHealth;

    public int armor;

    public Slider hpSlider;
    private void Update()
    {
        //if(canMove) 
            //
//        if (isSleeping)
//        {
//            tempHp += Time.deltaTime * sleepHealSpeed;
//            hpSlider.value = hpSlider.value+Mathf.CeilToInt(tempHp);
//        }
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
    }

    public bool Hit(int value)  
    {
        float minusPer = 100 *((float)armor*2 / (armor*2 + 100f));
        hpSlider.value -= value-(value * minusPer / 100f);
        if (hpSlider.value <= 0)
            return true;
        else
            return false;
    }

    public bool LoseHp(int value)
    {
        hpSlider.value -= value;
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
