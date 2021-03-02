using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelMgr : MonoBehaviour
{
    public ParticleSystem LvEffect;
    public int[] expValues;
    public Slider expSlider;
    

    private int currentValue = 0;

    public int Lv = 1;


    public Text currentExpTxt;
    public Text LvText;

    private StatManager statMgr;
    void Start()
    {
        statMgr = GetComponent<StatManager>();
        expSlider.maxValue = expValues[Lv-1];
    }
    public void GetExp(int value)
    {
        currentValue += value;
        if (currentValue >= expSlider.maxValue) //레벨업할 exp면
        {
            LvEffect.Play();
            currentValue -= (int)expSlider.maxValue;
            Lv++;
            statMgr.SetLevel(Lv-2);
            expSlider.maxValue = expValues[Lv - 1];
        }
        expSlider.value = currentValue;
    }

    private void Update()
    {
        currentExpTxt.text = currentValue + " / " + expSlider.maxValue;
        LvText.text = "Lv." + Lv;
    }
}
