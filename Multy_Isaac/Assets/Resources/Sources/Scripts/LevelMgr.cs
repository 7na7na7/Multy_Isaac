using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMgr : MonoBehaviour
{
    public int[] expValues;
    public Slider expSlider;
    

    private int currentValue = 0;

    public int Lv = 1;


    public Text currentExpTxt;
    public Text LvText;
    void Start()
    {
        expSlider.maxValue = expValues[Lv-1];
    }
    public void GetExp(int value)
    {
        currentValue += value;
        if (currentValue > expValues[Lv-1]) //레벨업할 exp면
        {
            currentValue -= expValues[Lv-1];
            Lv++;
            expSlider.maxValue = expValues[Lv - 1];
        }
        expSlider.value = currentValue;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            GetExp(10);


        currentExpTxt.text = currentValue + " / " + expSlider.maxValue;
        LvText.text = "Lv." + Lv;
    }
}
