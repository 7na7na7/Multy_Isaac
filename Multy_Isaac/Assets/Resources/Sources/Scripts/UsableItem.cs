using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    private playerCountSave pc;
    private int eatCoolTime = 1;
    private int cool = 0;
    private bool canEat = true;
    public Animator happy;
    public float happyTime;
    private float time = 0;
    private Player player;
    private StatManager statMgr;
    private offlineStat offStat;

    public GameObject Turret;
    public GameObject Mine;
    public GameObject Bomb;
    public GameObject BonFire;
    public GameObject Light;
    public GameObject FireBomb;
    public GameObject Pan1, Pan2;
    public GameObject NailMarble;
    public GameObject WireTrap;
    private void Start()
    {
        player = GetComponent<Player>();
        statMgr = transform.GetChild(0).GetComponent<StatManager>();
        offStat= transform.GetChild(0).GetComponent<offlineStat>();
        pc=playerCountSave.instance;
    }

    void setHappy()
    {
        if(PhotonNetwork.OfflineMode)
            happyRPC("Open");
        else
            GetComponent<PhotonView>().RPC("happyRPC",RpcTarget.All,"Open");
    }
    public bool UseItem(int itemIndex)
    {
        player.isFight();
        switch (itemIndex)
        {
            case 44: //붕대
                if (eat())
                {
                statMgr.Heal(15);
                setHappy();
                return true;
                }
                break;
            case 53: //좀비고기
                if (eat())
                {
                offStat.HungryHeal(9);
                setHappy();
                player.hitSound();
                if (statMgr.LoseHp(10))
                {
                    if(pc.isKor()) 
                        player.Die("식중독");      
                    else
                        player.Die("Food poisoning");      
                }
                return true;
                }
                break;
            case 55: //폭탄
                player.PutBombSound();
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Bomb, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Bomb.name, transform.position, Quaternion.identity);
                return true;
                break;
            case 57: //컵라면
                if (eat())
                {
                offStat.HungryHeal(15);
                setHappy();
                return true;
                }
                break;
            case 58: //삼각김밥
                if (eat())
                {
                offStat.HungryHeal(15);
                setHappy();
                return true;
                }
                break;
            case 59: //물
                if (eat())
                {
                    statMgr.Heal(15);
                    setHappy();
                    return true;
                }
                break;
            case 60: //꿀물
                if (eat())
                {
                statMgr.Heal(20);
                setHappy();
                return true;
                }
                break;
            case 61: //포도주스
                if (eat())
                {
                    statMgr.Heal(35);
                    setHappy(); 
                    return true;
                }
                break;
            case 62: //사과주스
                if (eat())
                {
                    statMgr.Heal(35);
                setHappy();
                return true;
                }
                break;
            case 63: //사과
                if (eat())
                {
                offStat.HungryHeal(8);
                setHappy();
                return true;
                }
                break;
            case 64: //포도
                if (eat())
                {
                offStat.HungryHeal(8);
                setHappy();
                return true;
                }
                break;
            case 67: //꿀
                if (eat())
                {
                statMgr.Heal(15);
                setHappy();
                return true;
                }
                break;
            case 72: //버섯 수프
                if (eat())
                {
                offStat.HungryHeal(60);
                statMgr.Heal(30);
                setHappy();
                return true;
                }
                break;
            case 73: //식빵
                if (eat())
                {
                offStat.HungryHeal(15);
                setHappy();
                return true;
                }
                break;
            case 74: //비타민주사
            if (eat())
            {
                statMgr.Heal(23);
                setHappy();
                return true;
            }
                break;
            case 75: //부목
                if (eat())
                {
                statMgr.Heal(35);
                setHappy();
                return true;
                }
                break;
            case 76: //모닥불
                if (PhotonNetwork.OfflineMode)
                    Instantiate(BonFire, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(BonFire.name, transform.position, Quaternion.identity);
                return true;
            case 78: //전등
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Light, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Light.name, transform.position, Quaternion.identity);
                return true;
            case 79: //화염병
                player.fireSound();
                if (PhotonNetwork.OfflineMode)
                {
                   GameObject g= Instantiate(FireBomb, transform.position, Quaternion.identity);
                   g.GetComponent<FireBomb>().ON();
                }
                else
            {
                GameObject g=PhotonNetwork.Instantiate(FireBomb.name, transform.position, Quaternion.identity);
                g.GetComponent<FireBomb>().ON();
            }
                   
                return true;
            case 80: //사과식초
                if (eat())
                {
                    statMgr.Heal(20);
                    setHappy();
                    return true;
                }
                break;
            case 84: //피자
                if (eat())
                {
                offStat.HungryHeal(15);
                setHappy();
                return true;
                }
                break;
            case 86: //치즈가 늘어나는 피자
                if (eat())
                {
                    offStat.HungryHeal(40);
                    setHappy();
                    return true;
                }
                break;
            case 96: //아드레날린
                if (eat())
                {
                    statMgr.Heal(100);
                    setHappy();
                    StartCoroutine(Adrenalin());
                    return true;
                }
                break;
            case 97: //지뢰
                player.PutBombSound();
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Mine, transform.position, Quaternion.identity);
                else
                {
                    GameObject a;
                    a=PhotonNetwork.Instantiate(Mine.name, transform.position, Quaternion.identity);   
                }
                return true;
                break;
            case 100: //과일세트
                if (eat())
                {
                    offStat.HungryHeal(25);
                    setHappy();
                    return true;
                }
                break;
            case 101: //샐러드
                if (eat())
                {
                    offStat.HungryHeal(20);
                    statMgr.Heal(20);
                    setHappy();
                    return true;
                }
                break;
            case 102: //껍질없는식빵
                if (eat())
                {
                    offStat.HungryHeal(40);
                    setHappy();
                    return true;
                }
                break;
            case 103: //오이
                if (eat())
                {
                    offStat.HungryHeal(8);
                    setHappy();
                    return true;
                }
                break;
            case 104: //피클
                if (eat())
                {
                    offStat.HungryHeal(8);
                    setHappy();
                    return true;
                }
                break;
            case 108: //버섯볶음
                if (eat())
                {
                    offStat.HungryHeal(30);
                    statMgr.Heal(30);
                    setHappy();
                    return true;
                }
                break;
            case 109: //과일빙수
                if (eat())
                {
                    offStat.HungryHeal(25);
                    statMgr.Heal(35);
                    setHappy();
                    return true;
                }
                break;
            case 110: //좀비요리
                if (eat())
                {
                    offStat.HungryHeal(15);
                    setHappy();
                    return true;
                }
                break;
            case 113: //감자
                if (eat())
                {
                    offStat.HungryHeal(7);
                    setHappy();
                    return true;
                }
                break;
            case 114: //감자맛탕
                if (eat())
                {
                    offStat.HungryHeal(10);
                    statMgr.Heal(10);
                    setHappy();
                    return true;
                }
                break;
            case 115: //꿀단지
                if (eat())
                {
                    statMgr.Heal(25);
                    setHappy();
                    return true;
                }
                break;
            case 116: //감자칩
                if (eat())
                {
                    offStat.HungryHeal(20);
                    setHappy();
                    return true;
                }
                break;
            case 117: //나무판
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Pan1, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Pan1.name, transform.position, Quaternion.identity);
                return true;
                break;
            case 118: //철판
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Pan2, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Pan2.name, transform.position, Quaternion.identity);
                return true;
                break;
            case 128: //터렛
                if (PhotonNetwork.OfflineMode)
                    Instantiate(Turret, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(Turret.name, transform.position, Quaternion.identity);
                return true;
                break;
            case 131: //오이냉국
                if (eat())
                {
                    offStat.HungryHeal(60);
                    setHappy();
                    return true;
                }
                break;
            case 137: //옥수수
                if (eat())
                {
                    offStat.HungryHeal(8);
                    setHappy();
                    return true;
                }
                break;
            case 138: //콘스프
                if (eat())
                {
                    offStat.HungryHeal(30);
                    statMgr.Heal(30);
                    setHappy();
                    return true;
                }
                break;
            case 140: //옥수수식빵
                if (eat())
                {
                    offStat.HungryHeal(20);
                    setHappy();
                    return true;
                }
                break;
            case 141: //팝콘
                if (eat())
                {
                    offStat.HungryHeal(5);
                    setHappy();
                    return true;
                }
                break;
            case 156: //가시공
                player.fireSound();
                if (PhotonNetwork.OfflineMode)
                {
                    GameObject g= Instantiate(NailMarble, transform.position, Quaternion.identity);
                    g.GetComponent<NailMarble>().ON();
                }
                else
                {
                    GameObject g=PhotonNetwork.Instantiate(NailMarble.name, transform.position, Quaternion.identity);
                    g.GetComponent<NailMarble>().ON();
                }
                return true;
            case 163: //가시공
                //player.fireSound();
                if (PhotonNetwork.OfflineMode)
                    Instantiate(WireTrap, transform.position, Quaternion.identity);
                else
                    PhotonNetwork.Instantiate(WireTrap.name, transform.position, Quaternion.identity);
                return true;
        }

        return false;
    }

    IEnumerator Adrenalin()
    {
        GetComponent<PassiveItem>().Speed += 60;
        yield return new WaitForSeconds(20f);
        GetComponent<PassiveItem>().Speed -= 60;
    }
    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
               if(PhotonNetwork.OfflineMode)
                   happyRPC("Close");
               else
                   GetComponent<PhotonView>().RPC("happyRPC",RpcTarget.All,"Close");
            }
        }
    }

    [PunRPC]
    void happyRPC(string anim)
    {
        if(anim=="Open")
            time = happyTime;
        happy.Play(anim);
    }
    bool eat()
    {
        if (canEat)
        {
            player.eatSound();
            StartCoroutine(eatCool());
            return true;
            
        }
        else
        {
            return false;
        }
    }
    IEnumerator eatCool()
    {
        canEat = false;
        cool = eatCoolTime;
        for (int i = 0; i < eatCoolTime; i++)
        {
            yield return new WaitForSeconds(1);
            cool--;
        }
        canEat = true;
    }

    public int getCool()
    {
        return cool;
    }
}
