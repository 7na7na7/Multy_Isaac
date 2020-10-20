using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public List<tem> temDatas;
    
    public tem GetItem(int Index)
    {
        tem tem = temDatas.Find(data => data.index == Index);
        if (tem != null)
        {
            return tem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            return temDatas[0];
        }
//        for (int i = 0; i < temDatas.Count; i++)
//        {
//            if (temDatas[i].index == Index)
//            {
//                tem item = temDatas[i];
//               
//            }
//        }
//       
//        return temDatas[0];
    }
}
