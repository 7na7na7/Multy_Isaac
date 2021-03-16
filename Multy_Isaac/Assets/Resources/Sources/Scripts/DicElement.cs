using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DicElement : MonoBehaviour
{
    public Image image;
    public tem tem;

    public void Click()
    {
        FindObjectOfType<Invent>().DicClose();
        FindObjectOfType<Invent>().Open(tem);
    }

    private void Start()
    {
        image.sprite = tem.ItemSprite;
    }
}
