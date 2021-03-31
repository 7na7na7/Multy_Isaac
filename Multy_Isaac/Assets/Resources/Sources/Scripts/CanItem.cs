using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanItem : MonoBehaviour
{
    private Invent invet;
    public int index = 0;
    private PlayerItem p;
    private TemManager tem;
    private void Start()
    {
        invet = FindObjectOfType<Invent>();
        p = FindObjectOfType<PlayerItem>();
        tem = FindObjectOfType<TemManager>();
    }

    public void Combine()
    {
        if(index!=0) 
            invet.CombineClick(tem.GetItemList(index));
    }
}
