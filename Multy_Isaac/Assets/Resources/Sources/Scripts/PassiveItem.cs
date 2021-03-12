using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    private StatManager statMgr;
    private PlayerLight light;
    //기동신
    private int mobile;
    public float mobileTime= 0;
    public int mobilePer=100;
    public float savedMobileTime=5;
    //소음기
    public int Silence = 0;
    //신발등 이동속도
    public int Speed = 0;
    private void Start()
    {
        statMgr = transform.GetChild(0).GetComponent<StatManager>();
        light=transform.GetChild(1).GetComponent<PlayerLight>();
    }

    public void PassiveOn(int itemIndex)
    {
        switch (itemIndex)
        {
            case 43: //기동신
                mobilePer += 30;
                break;
            case 48: //양말
                Speed += 7;
                break;
            case 49: //스타킹
                Speed += 17;
                break;
            case 81: //소음기
                Silence++;
                break;
            case 82://티셔츠
                statMgr.armor += 10;
                break;
            case 83: //신발
                statMgr.armor += 5;
                Speed += 7;
                break;
            case 90: //멋진티셔츠
                statMgr.armor += 15;
                break;
            case 107: //횃불
                light.lightValue += 0.02f;
                light.torchOn();
                break;
            case 119: //안전모
                statMgr.armor += 7;
                break;
            case 120: //목발
                Speed += 12;
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
            case 48:
                Speed -= 7;
                break;
            case 49:
                Speed -= 17;
                break;
            case 81:
                Silence--;
                break;
            case 82:
                statMgr.armor -= 10;
                break;
            case 83:
                statMgr.armor -= 5;
                Speed -= 7;
                break;
            case 90:
                statMgr.armor -= 15;
                break;
            case 107: //횃불
                light.lightValue -= 0.02f;
                light.torchOff();
                break;
            case 119: //안전모
                statMgr.armor -= 7;
                break;
            case 120: //목발
                Speed -= 12;
                break;
        }
    }
}
