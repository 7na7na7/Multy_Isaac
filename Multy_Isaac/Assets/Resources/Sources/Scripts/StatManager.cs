using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public int startHealth;
    public int startMana;
    public int startArmor;
    public int[] healths;
    public int[] manas;
    public int[] armors;
    public float[] manaRegens;

    public int armor;

    public Slider hpSlider;
    public Slider manaSlider;
    public Text hpText;
    public float MpHealSpeed = 5;
    public float sleepHealSpeed = 1;

    public bool canMove = false;
    public bool isSleeping = false;
    
    private void Update()
    {
        hpText.text = hpSlider.value + " / " + hpSlider.maxValue;
        if(canMove) 
            manaSlider.value += Time.deltaTime * MpHealSpeed;
        if(isSleeping)
            hpSlider.value=(int)(hpSlider.value+Time.deltaTime * sleepHealSpeed);
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
        
        //마나
        int healValueMana;
        if (i == 0)
            healValueMana = manas[i] - startMana;
        else
            healValueMana= manas[i] - (int)manaSlider.maxValue;

        manaSlider.maxValue += healValueMana;
        manaSlider.value += healValueMana;

        int armorValue;
        if (i == 0)
            armorValue = armors[i] - startArmor;
        else
            armorValue = armors[i] -armors[i-1];
        armor += armorValue;
    }
    
    public bool LoseMp(int value) //마나 잃음
    {
        if (manaSlider.value >= value)
        {
            manaSlider.value -= value;
            return true;
        }
        else
        {
            return false;
        }
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
