using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public Animator happy;
    public float happyTime;
    private float time = 0;
    private Player player;
    private StatManager statMgr;
    private offlineStat offStat;

    public GameObject Bomb;
    public GameObject BonFire;
    public GameObject Light;
    public GameObject FireBomb;
    
    private void Start()
    {
        player = GetComponent<Player>();
        statMgr = transform.GetChild(0).GetComponent<StatManager>();
        offStat= transform.GetChild(0).GetComponent<offlineStat>();
    }

    void setHappy()
    {
        happy.Play("Open");
        time = happyTime;
    }
    public void UseItem(int itemIndex)
    {
        player.isFight();
        switch (itemIndex)
        {
            case 44: //붕대
                statMgr.Heal(15);
                setHappy();
                break;
            case 53: //좀비고기
                offStat.HungryHeal(10);
                setHappy();
                if(statMgr.LoseHp(10))
                    player.Die("식중독");   
                break;
            case 55: //폭탄
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Bomb, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Bomb.name, transform.position, Quaternion.identity);
                break;
            case 57: //컵라면
                offStat.HungryHeal(20);
                setHappy();
                break;
            case 58: //삼각김밥
                offStat.HungryHeal(15);
                setHappy();
                break;
            case 60: //꿀물
                statMgr.Heal(15);
                setHappy();
                break;
            case 61: //포도주스
                statMgr.Heal(30);
                setHappy();
                break;
            case 62: //사과주스
                statMgr.Heal(30);
                setHappy();
                break;
            case 63: //사과
                offStat.HungryHeal(10);
                setHappy();
                break;
            case 64: //포도
                offStat.HungryHeal(10);
                setHappy();
                break;
            case 67: //꿀
                statMgr.Heal(10);
                setHappy();
                break;
            case 72: //버섯 수프
                offStat.HungryHeal(60);
                statMgr.Heal(30);
                setHappy();
                break;
            case 73: //식빵
                offStat.HungryHeal(15);
                setHappy();
                break;
            case 74: //비타민주사
                statMgr.Heal(20);
                setHappy();
                break;
            case 75: //부목
                statMgr.Heal(35);
                setHappy();
                break;
            case 76: //모닥불
                if (PhotonNetwork.OfflineMode)
                    Instantiate(BonFire, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(BonFire.name, transform.position, Quaternion.identity);
                break;
            case 78: //전등
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Light, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Light.name, transform.position, Quaternion.identity);
                break;
            case 79: //화염병
                if (PhotonNetwork.OfflineMode)
                    Instantiate(FireBomb, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(FireBomb.name, transform.position, Quaternion.identity);
                break;
            case 84: //피자
                offStat.HungryHeal(15);
                setHappy();
                break;
            case 86: //치즈가 늘어나는 피자
                offStat.HungryHeal(50);
                statMgr.Heal(50);
                setHappy();
                break;
        }
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if(time<=0)
                happy.Play("Close");
        }
    }
}
