using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    private PhotonView pv;

    public int office = 0;
    //가시갑옥
    public int spike = 0;
    //레이더
    public float laderRad = 10;
    private IEnumerator laderCor;
    public ParticleSystem laderParticle;
    private int laderCount = 0;
    
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
    //기계다리
    public int machineLegCount = 0;
    private offlineStat offStat;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        laderCor = LaderCor();
        statMgr = transform.GetChild(0).GetComponent<StatManager>();
        light=transform.GetChild(1).GetComponent<PlayerLight>();
        offStat=transform.GetChild(0).GetComponent<offlineStat>();
    }

    [PunRPC]
    void spikeRPC(int v)
    {
        spike = v;
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
                offStat.MaxHpUp(30);
                break;
            case 107: //횃불
                light.lightValue += 0.05f;
                light.torchOn();
                break;
            case 119: //안전모
                statMgr.armor += 10;
                break;
            case 120: //목발
                Speed += 12;
                break;
            case 122: //스웨터
                statMgr.armor += 15;
                offStat.MaxHpUp(15);
                break;
            case 123: //방탄모
                statMgr.armor += 35;
                break;
            case 125: //목도리
                statMgr.armor += 20;
                break;
            case 126: //레이더
                StartCoroutine(laderCor);
                laderCount++;
                break;
            case 127: //기계다리
                statMgr.armor += 10;
                Speed += 15;
                machineLegCount++;
                break;
            case 133: //패딩
                statMgr.armor += 35;
                offStat.MaxHpUp(35);
                break;
            case 134: //기름신발
                statMgr.armor += 5;
                Speed += 14;
                break;
            case 135: //셔츠
                statMgr.armor += 20;
                break;
            case 136: //슈트
                statMgr.armor += 60;
                offStat.MaxHpUp(30);
                break;
            case 139: //사슬갑옷
                statMgr.armor += 40;
                break;
            case 146: //체인레깅스
                statMgr.armor += 20;
                Speed += 20;
                break;
            case 147: //가시갑옷
                statMgr.armor += 50;
                spike++;
                if(!PhotonNetwork.OfflineMode)
                    pv.RPC("spikeRPC",RpcTarget.All,spike);
                break;
            case 149: //찢어진스타킹
                Speed += 27;
                break;
            case 150: //경찰조끼
                statMgr.armor += 15;
                break;
            case 151: //방탄조끼
                statMgr.armor += 30;
                offStat.MaxHpUp(15);
                break;
            case 152: //인형
                offStat.MaxHpUp(20);
                break;
            case 153: //다키마쿠라
                offStat.MaxHpUp(50);
                break;
            case 158: //풀바디 아머
                statMgr.armor += 100;
                break;
            case 162: //다이아갑옷
                statMgr.armor += 40;
                offStat.MaxHpUp(40);
                Speed += 12;
                break;
            case 164: //오핏스룩
                offStat.MaxHpUp(35);
                Speed += 20;
                office++;
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
                offStat.MaxHpDown(30);
                break;
            case 107: //횃불
                light.lightValue -= 0.05f;
                light.torchOff();
                break;
            case 119: //안전모
                statMgr.armor -= 10;
                break;
            case 120: //목발
                Speed -= 12;
                break;
            case 122: //스웨터
                statMgr.armor -= 15;
                offStat.MaxHpDown(15);
                break;
            case 123: //방탄모
                statMgr.armor -= 35;
                break;
            case 125: //목도리
                statMgr.armor -= 20;
                break;
            case 126: //레이더
                laderCount--;
                if(laderCount<=0) 
                    StopCoroutine(laderCor);
                break;
            case 127: //기계다리
                statMgr.armor -= 10;
                Speed -= 15;
                machineLegCount--;
                break;
            case 133: //패딩
                statMgr.armor -= 35;
                offStat.MaxHpDown(35);
                break;
            case 134: //기름신발
                statMgr.armor -= 5;
                Speed -= 14;
                break;
            case 135: //셔츠
                statMgr.armor -= 20;
                break;
            case 136: //슈트
                statMgr.armor -= 60;
                offStat.MaxHpDown(30);
                break;
            case 139: //사슬갑옷
                statMgr.armor -= 40;
                break;
            case 146: //체인레깅스
                statMgr.armor -= 20;
                Speed -= 20;
                break;
            case 147: //가시갑옷
                statMgr.armor -= 50;
                spike--;
                if(!PhotonNetwork.OfflineMode)
                    pv.RPC("spikeRPC",RpcTarget.All,spike);
                break;
            case 149: //찢어진스타킹
                Speed -= 27;
                break;
            case 150: //경찰조끼
                statMgr.armor -= 15;
                break;
            case 151: //방탄조끼
                statMgr.armor -= 30;
                offStat.MaxHpDown(15);
                break;
            case 152: //인형
                offStat.MaxHpDown(20);
                break;
            case 153: //다키마쿠라
                offStat.MaxHpDown(50);
                break;
            case 158: //풀바디 아머
                statMgr.armor -= 100;
                break;
            case 162: //다이아갑옷
                statMgr.armor -= 40;
                Speed -= 12; 
                offStat.MaxHpDown(40);
                break;
            case 164: //오핏스룩
                offStat.MaxHpDown(35);
                Speed -= 20;
                office--;
                break;
        }
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
