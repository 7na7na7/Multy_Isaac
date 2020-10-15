using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public PlayerItem player;
    public int index;
    private Image img;
    public GameObject Btns;
    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void Drag()
    {
       img.transform.position = Input.mousePosition;
//       print(img.transform.position);
    }

    public void DragEnd()
    {
        print(img.transform.position);
    }

    public void Down()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(!Btns.activeSelf)
                player.OtherBtnSetFalse(index);
            Btns.SetActive(!Btns.activeSelf);
        }
    }

    public void Discard()
    {
        player.DiscardItem(index);
        Btns.SetActive(false);
    }
    public void Up()
    {
        
    }
}
