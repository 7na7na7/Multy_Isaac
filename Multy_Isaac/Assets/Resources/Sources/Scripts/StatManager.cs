using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public int startHealth;
    public int startArmor;
    public int[] healths;
    public int[] armors;

    public int armor;

    public Slider hpSlider;
    public Text hpText;
    public float MpHealSpeed = 5;
    public float sleepHealSpeed = 1;

    public bool canMove = false;

    public float tempHp = 0;
    private void Update()
    {
        hpText.text = hpSlider.value + " / " + hpSlider.maxValue;
        //if(canMove) 
            //
//        if (isSleeping)
//        {
//            tempHp += Time.deltaTime * sleepHealSpeed;
//            hpSlider.value = hpSlider.value+Mathf.CeilToInt(tempHp);
//        }
    }

    public void SetLevel(int i)
    {
        //체력
        int healValue;
        if (i == 0)
            healValue = healths[i] - startHealth;
        else
            healValue=healths[i] - healths[i - 1];
        hpSlider.maxValue += healValue;
        hpSlider.value += healValue;
        
        
        int armorValue;
        if (i == 0)
            armorValue = armors[i] - startArmor;
        else
            armorValue = armors[i] -armors[i-1];
        armor += armorValue;
    }
    
    public void Heal(int value)
    {
        if (hpSlider.maxValue - hpSlider.value < value) //회복량이 잃은체력보다 크면
            hpSlider.value = hpSlider.maxValue;
        else
            hpSlider.value += value;
    }

    public bool LoseHp(int value)
    {
        hpSlider.value -= value-(value*(armor*0.01f));
        if (hpSlider.value <= 0)
            return true;
        else
            return false;
    }
}
