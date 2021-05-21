using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public int dakimakura=0;
    public int padding = 0;
    private PhotonView pv;

    public int office = 0;

    //가시갑옥
    public int spike = 0;

    //레이더
    public float laderRad = 10;
    private IEnumerator laderCor;
    public ParticleSystem laderParticle;
    public int laderCount = 0;

    private StatManager statMgr;

    private PlayerLight light;

    //기동신
    private int mobile;
    public float mobileTime = 0;
    public int mobilePer = 100;

    public float savedMobileTime = 5;

    //소음기
    public int Silence = 0;

    //신발등 이동속도
    public int Speed = 0;

    //기계다리
    public int machineLegCount = 0;
    private offlineStat offStat;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        laderCor = LaderCor();
        statMgr = transform.GetChild(0).GetComponent<StatManager>();
        light = transform.GetChild(1).GetComponent<PlayerLight>();
        offStat = transform.GetChild(0).GetComponent<offlineStat>();
    }

    [PunRPC]
    void spikeRPC(int v)
    {
        spike = v;
    }

    public void Passive(int index,bool isOn)
    {
        if (index == 43) //기동신
        {
            mobilePer += isOn ? 50 : -50;
        }
        else if (index == 48) //양말
        {
            Speed += isOn ? 7 : -7;
        }
        else if (index == 49) //스타킹
        {
            Speed += isOn ? 17 : -17;
        }
        else if (index == 81) //소음기
        {
            Silence += isOn ? 1 : -1;
        }
        else if (index == 82) //티셔츠
        {
            offStat.MaxHpUp(isOn?10:-10);
        }
        else if (index == 83) //신발
        {
            Speed += isOn ? 8 : -8;
        }
        else if (index == 90) //멋진티셔츠
        {
            offStat.MaxHpUp(isOn ? 30 : -30);
        }
        else if (index == 107) //횃불
        {
            light.lightValue += isOn ? 0.075f : -0.075f;
            if(isOn)
                light.torchOn();
            else
                light.torchOff();
        }
        else if (index == 119) //안전모
        {
            offStat.MaxHpUp(isOn ? 8 : -8);
        }
        else if (index == 120) //목발
        {
            Speed += isOn ? 12 : -12;
        }
        else if (index == 122) //스웨터
        {
            offStat.MaxHpUp(isOn ? 25 : -25);
        }
        else if (index == 123) //방탄모
        {
            offStat.MaxHpUp(isOn ? 40 : -40);
        }
        else if (index == 125) //목도리
        {
            offStat.MaxHpUp(isOn ? 25 : -25);
        }
        else if (index == 126) //레이더
        {
            if (isOn)
            {
                StartCoroutine(laderCor);
                laderCount++;
            }
            else
            {
                laderCount--;
                if(laderCount<=0) 
                    StopCoroutine(laderCor);
            }
        }
        else if (index == 127) //기계다리
        {
            offStat.MaxHpUp(isOn?15:-15);
            Speed += isOn ? 15 : -15;
            machineLegCount += isOn ? 1 : -1;
        }
        else if (index == 133) //패딩
        {
            offStat.MaxHpUp(isOn ? 55 : -55);
            padding += isOn ? 1 : -1;
        }
        else if (index == 134) //기름신발
        {
            Speed += isOn ? 16 : -16;
        }
        else if (index == 135) //셔츠
        {
            offStat.MaxHpUp(isOn?30:-30);
        }
        else if (index == 136) //슈트
        {
            offStat.MaxHpUp(isOn?75:-75);
        }
        else if (index == 139) //사슬갑옷
        {
            offStat.MaxHpUp(isOn?45:-45);
        }
        else if (index == 146) //체인레깅스
        {
            offStat.MaxHpUp(isOn?25:-25);
            Speed += isOn ? 20 : -20;
        }
        else if (index == 147) //가시갑옷
        {
            offStat.MaxHpUp(isOn?45:-45);
            spike += isOn ? 1 : -1;
            if(!PhotonNetwork.OfflineMode)
                pv.RPC("spikeRPC",RpcTarget.All,spike);
        }
        else if (index==149) //찢어진스타킹
        {
            Speed += isOn ? 27 : -27;
        }
        else if (index == 150) //경찰조끼
        {
            offStat.MaxHpUp(isOn?15:-15);
        }
        else if (index == 151) //방탄조끼
        {
            offStat.MaxHpUp(isOn?40:-40);
        }
        else if (index == 152) //인형
        {
            offStat.MaxHpUp(isOn?25:-25);
        }
        else if (index == 153) //다키마쿠라
        {
            offStat.MaxHpUp(isOn?35:-35);
            dakimakura += isOn ? 1 : -1; 
        }
        else if (index == 158) //풀바디아머
        {
            offStat.MaxHpUp(isOn?150:-150);
            Speed += isOn ? -15 : 15;
        }
        else if (index == 162) //다이아갑옷
        {
            offStat.MaxHpUp(isOn?55:-55);
            Speed += isOn ? 12 : -12;
        }
        else if (index == 164) //오피스룩
        {
            offStat.MaxHpUp(isOn?40:-40);
            Speed += isOn ? 20 : -20;
            office += isOn ? 1 : -1;
        }
    }
    
    public void PassiveOn(int itemIndex)
    { 
        Passive(itemIndex,true);
    }
        
    public void PassiveOff(int itemIndex)
    {
        Passive(itemIndex,false);
    }

    public void StopLader()
    {
        StopCoroutine(laderCor);
    }
    IEnumerator LaderCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            laderParticle.Play();
            
            RaycastHit2D[] zombies = Physics2D.CircleCastAll(transform.position, laderRad, Vector2.up,0);
            foreach (RaycastHit2D col in zombies)
            {
                if (col.collider.CompareTag("Enemy"))
                {
                    col.collider.gameObject.GetComponent<FlashWhite>().Lader();
                }
            }   
        }
    }
}
