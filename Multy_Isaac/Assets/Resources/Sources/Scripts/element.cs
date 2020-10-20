using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class element : MonoBehaviour
{
    public Invent invent;
    public Sprite None;
    public tem tem;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (tem.index != 0)
        {
            img.sprite = tem.ItemSprite;
        }
        else
        {
            img.sprite = None;
        }
    }

    public void Open()
    {
        invent.Open(tem);
    }
}
