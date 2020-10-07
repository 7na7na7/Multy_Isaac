using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject popup;
    public static PopUpManager instance;
    void Start()
    {
        instance = this;
    }

    public void PopUp(string txt,Color color)
    {
        GameObject p=Instantiate(popup, transform);
        p.GetComponent<Text>().color = color;
        p.GetComponent<Text>().text = txt;
    }
}
