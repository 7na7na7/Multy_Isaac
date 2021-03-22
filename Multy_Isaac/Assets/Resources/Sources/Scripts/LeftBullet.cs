using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftBullet : MonoBehaviour
{
    private Player player;
    public int StartBulletCount = 30;
    public int[] leftBullets;
    int GetedBullet = 0;
    public Text GetedBulletText;
    
    public int bulletCount = 0;
    public int maxBulletCount = 0;
    public GameObject[] bullets;
    public GameObject[] siluettes;
    public float reLoadTime;

    public GameObject parentGO;

    private void Start()
    {
        player = transform.parent.gameObject.transform.parent.GetComponent<Player>();
        leftBullets=new int[8];
        GetedBullet += StartBulletCount;
        GetedBulletText.text = "X " + GetedBullet;
    }

    public int getBulletCount()
    {
        return bulletCount;
    }
    
    public void GetBullet(int value)
    {
        player.getBulletSound();
        GetedBullet += value;
        GetedBulletText.text = "X " + GetedBullet;
    }
    
    public void SetFalse()
    {
        parentGO.SetActive(false);
    }
    public bool canReload()
    {
        if (bulletCount < maxBulletCount&& GetedBullet>0)
            return true;
        else
            return false;
    }
    public void Reload(int selectedIndex)
    {
        int leftBullet = maxBulletCount - bulletCount; //재장전해야 하는 총알의 수
        int value1 = bulletCount + leftBullet;
        if (GetedBullet >= leftBullet) //자신에게 있는 총알의 개수가 재장전해야 하는 총알의 수보다 많으면 재장전 
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (i < value1)
                    bullets[i].SetActive(true);
                else
                    bullets[i].SetActive(false);
            }
            bulletCount = maxBulletCount;
            GetedBullet -= leftBullet;
        }
        else //총알이 적으면 그 수만큼 재장전
        {
            int value2 = bulletCount + GetedBullet;
            for (int i = 0; i < bullets.Length; i++)
            {
                if (i < value2)
                    bullets[i].SetActive(true);
                else
                    bullets[i].SetActive(false);
            }

            bulletCount = value2;
            GetedBullet = 0;
        }
        GetedBulletText.text = "X " + GetedBullet;
        leftBullets[selectedIndex]=bulletCount;
    }
    
    public void SetBullet(int maxBullet, int selectedIndex, bool isFirst)
    {
        parentGO.SetActive(true);
        maxBulletCount = maxBullet;
        if (isFirst)
        {
            bulletCount = 0;
            leftBullets[selectedIndex] = 0;
        }
        else
            bulletCount = leftBullets[selectedIndex];
        
        for (int i = 0; i < bullets.Length; i++)
        {
            if (i < maxBullet)
            {
                siluettes[i].SetActive(true);
            }
            else
            {
                siluettes[i].SetActive(false);
            }

            if (i < bulletCount)
            {
                bullets[i].SetActive(true);
            }
            else
            {
                bullets[i].SetActive(false);
            }
        }
    }
    public bool MinusBullet(int selectedIndex,int consumeBullet)
    {
        if (bulletCount <= 0)
        {
            return false;
        }
        else
        {
            bulletCount-=consumeBullet;
            for (int i = 0; i < bullets.Length; i++)
            {
                if (i <bulletCount)
                {
                    bullets[i].SetActive(true);
                }
                else
                {
                    bullets[i].SetActive(false);
                }
            }
            leftBullets[selectedIndex] = bulletCount;
            return true;
        }
    }

    public bool isBulletMax()
    {
        if (bulletCount < maxBulletCount)
            return false;
        else
            return true;
    }
}
