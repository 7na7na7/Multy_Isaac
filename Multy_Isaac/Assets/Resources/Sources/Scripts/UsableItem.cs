using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
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

    public void UseItem(int itemIndex)
    {
        player.isFight();
        switch (itemIndex)
        {
            case 44: //붕대
                statMgr.Heal(15);
                break;
            case 53: //좀비고기
                offStat.HungryHeal(10);
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
                break;
            case 58: //삼각김밥
                offStat.HungryHeal(15);
                break;
            case 60: //꿀물
                statMgr.Heal(15);
                break;
            case 61: //포도주스
                statMgr.Heal(30);
                break;
            case 62: //사과주스
                statMgr.Heal(30);
                break;
            case 63: //사과
                offStat.HungryHeal(10);
                break;
            case 64: //포도
                offStat.HungryHeal(10);
                break;
            case 67: //꿀
                statMgr.Heal(10);
                break;
            case 72: //버섯 수프
                offStat.HungryHeal(40);
                statMgr.Heal(20);
                break;
            case 73: //식빵
                offStat.HungryHeal(15);
                break;
            case 74: //비타민주사
                statMgr.Heal(20);
                break;
            case 75: //부목
                statMgr.Heal(45);
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
        }
    }
}
