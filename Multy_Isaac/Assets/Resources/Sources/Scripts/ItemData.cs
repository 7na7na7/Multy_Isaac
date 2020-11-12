﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public GameObject[] temPrefabs;
    public List<tem> temDatas;
    public List<weapon> weaponList;

    private void Awake()
    {
        foreach (GameObject item in temPrefabs)
        {
            temDatas.Add(item.GetComponent<Item>().item.DeepCopy());
            if(item.GetComponent<Item>().item.type==itemType.Weapon) 
                weaponList.Add(item.GetComponent<weapon>());
        }
    }

    public weapon GetWeapon(int Index)
    {
       weapon tem = weaponList.Find(data => data.weaponIndex == Index);
       weapon copyTem=new weapon();
        if (tem != null)
        {
            copyTem = tem.DeepCopy();
            return copyTem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            return copyTem;
        }
    }
    public tem GetItemList(int Index)
    {
        tem tem = temDatas.Find(data => data.index == Index);
        tem copyTem=new tem();
        if (tem != null)
        {
            copyTem = tem.DeepCopy();
            return copyTem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            return copyTem;
        }
    }
}
