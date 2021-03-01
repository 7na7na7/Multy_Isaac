using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    private StatManager statMgr;
    //기동신
    private int mobile;
    public float mobileTime= 0;
    public int mobilePer=100;
    public float savedMobileTime=5;
    //소음기
    public int Silence = 0;
    
    private void Start()
    {
        statMgr = transform.GetChild(0).GetComponent<StatManager>();
    }

    public void PassiveOn(int itemIndex)
    {
        switch (itemIndex)
        {
            case 43:
                mobilePer += 30;
                break;
            case 81:
                Silence++;
                break;
            case 82:
                statMgr.armor += 10;
                break;
        }
    }
        
    public void PassiveOff(int itemIndex)
    {
        switch (itemIndex)
        {
            case 43:
                mobilePer -= 30;
                break;
            case 81:
                Silence--;
                break;
            case 82:
                statMgr.armor -= 10;
                break;
        }
    }
}
