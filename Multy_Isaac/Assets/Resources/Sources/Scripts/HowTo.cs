using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowTo : MonoBehaviour
{
    public GameObject TUTO;
    public GameObject[] tutorials;
    public GameObject prevBtn;
    public GameObject nextBtn;
    public int Index = 0;


    public void On()
    {
        if (!TUTO.activeSelf)
        {
            TUTO.SetActive(true);
            set();   
        }
        else
        {
            TUTO.SetActive(false);
        }
    }
    
    public void Prev()
    {
        Index--;
        if (Index == 0)
            prevBtn.SetActive(false);
        else
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }
        set();
    }
    
    public void Next()
    {
        Index++;
        if (Index == tutorials.Length-1)
            nextBtn.SetActive(false);
        else
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }
        set();
    }

    void set()
    {
        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i==Index) 
                tutorials[i].SetActive(true);
            else
                tutorials[i].SetActive(false);
        }
      
    }
}