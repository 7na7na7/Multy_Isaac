using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public GameObject[] temPrefabs;
    public List<tem> temDatas;
    public List<wep> weaponList;

    private void Awake()
    {
        for (int i = 0; i < temPrefabs.Length; i++)
        {
            tem tem = temPrefabs[i].GetComponent<Item>().item.DeepCopy();
            temDatas.Add(tem);

            if (tem.weaponIndex > 0)
            {
                wep weapon = temPrefabs[i].GetComponent<weapon>().weaponObj.DeepCopy();
                weaponList.Add(weapon);
            }
            
        }
    }

    public wep GetWeapon(int Index)
    {
       wep tem = weaponList.Find(data => data.weaponIndex == Index);
       wep copyTem=new wep();
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
