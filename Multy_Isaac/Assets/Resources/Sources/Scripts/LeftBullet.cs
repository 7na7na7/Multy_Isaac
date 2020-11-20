using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftBullet : MonoBehaviour
{
    int GetedBullet = 0;
    public Text GetedBulletText;
    
    public int bulletCount = 0;
    public int maxBulletCount = 0;
    public GameObject[] bullets;
    public GameObject[] siluettes;
    public float reLoadTime;

    public void GetBullet(int value)
    {
        GetedBullet += value;
    }
    public void SetBullet(int maxBullet)
    {
        maxBulletCount = maxBullet;
        bulletCount = maxBulletCount;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (i < maxBullet)
            {
                siluettes[i].SetActive(true);
                bullets[i].SetActive(true);
            }
            else
            {
                siluettes[i].SetActive(false);
                bullets[i].SetActive(false);
            }
        }
    }
    public bool MinusBullet()
    {
        if (bulletCount <= 0)
        {
            return false;
        }
        else
        {
            bulletCount--;
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
    private void Update()
    {
        GetedBulletText.text = "X " + GetedBullet;
    }
}
