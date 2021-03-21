using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dic : MonoBehaviour
{
    public itemType type;
    private List<tem> tems;
    public GameObject dicElement;
    private void Start()
    {
        if(SceneManager.GetActiveScene().name=="Play") 
            Invoke("set",3f);
    }

    void set()
    {
        tems = FindObjectOfType<TemManager>().temDatas;
        foreach (tem t in tems)
        {
            if (t.type == type)
            {
                GameObject a=Instantiate(dicElement, transform);
                a.GetComponent<DicElement>().tem = t;   
            }
        }

        if(type!=itemType.Item)
            gameObject.transform.parent.gameObject.SetActive(false);
    }
}
