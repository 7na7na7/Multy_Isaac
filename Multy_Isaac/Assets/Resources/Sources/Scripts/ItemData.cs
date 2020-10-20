using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public List<tem> temDatas;

    public tem GetItem(int Index)
    {
        print(Index);
        for (int i = 0; i < temDatas.Count; i++)
        {
            if (temDatas[i].index == Index)
            {
                return temDatas[i];
            }
        }
        print("해당 인덱스는 없어용!");
        return temDatas[0];
    }
}
