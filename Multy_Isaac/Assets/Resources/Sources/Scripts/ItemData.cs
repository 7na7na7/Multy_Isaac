using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public List<tem> temDatas;

    private void Update()
    {
        print(GetItem(3).ItemName);
    }

    public tem GetItem(int Index)
    {
        for (int i = 0; i < temDatas.Count; i++)
        {
            if (temDatas[i].index == Index)
            {
                tem item = temDatas[i];
                return item;
            }
        }
        print(Index+"(이)라는 인덱스는 없어용!");
        return temDatas[0];
    }
}
