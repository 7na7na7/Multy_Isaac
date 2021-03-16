using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Dic : MonoBehaviour
{
    private List<tem> tems;
    public GameObject dicElement;
    private void Start()
    {
        Invoke("set",0.2f);
    }

    void set()
    {
        tems = FindObjectOfType<TemManager>().temDatas;
        foreach (tem t in tems)
        {
            GameObject a=Instantiate(dicElement, transform);
            a.GetComponent<DicElement>().tem = t;
        }

        gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
