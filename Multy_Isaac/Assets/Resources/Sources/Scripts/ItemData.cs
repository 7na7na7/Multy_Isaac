using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public List<tem> temDatas;
    
    public tem GetItemList(int Index)
    {
        tem tem = temDatas.Find(data => data.index == Index);
        tem copyTem=new tem();
        if (tem != null)
        {
            copyTem.Copy(tem);
            return copyTem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            return copyTem;
        }
    }
}
