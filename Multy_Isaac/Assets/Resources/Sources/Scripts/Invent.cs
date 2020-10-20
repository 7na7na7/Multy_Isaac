using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    public tem[] completeTemArray;
    private tem elementItem;
    public Sprite NoneSprite;
    public Sprite BoxSprite;
    public Image BigItemImg;
    public Text BigItemName;
    public Image[] completeBoxes; //조합템 테두리
    public Image[] completes; //조합템
    public Text ItemName; //템이름 
    public Text ItemDescription; //템설명

    private void Update()
    {
        for (int i = 0; i < completeTemArray.Length; i++)
        {
            if (completeTemArray[i].index != 0)
            {
                completes[i].sprite = completeTemArray[i].ItemSprite; 
                completeBoxes[i].sprite = BoxSprite;
            }
            else
            {
                completes[i].sprite = NoneSprite;
                completeBoxes[i].sprite = NoneSprite;
            }
            
        }

        if (elementItem != null)
        {
            ItemName.text = elementItem.ItemName;
            ItemDescription.text = elementItem.ItemDescription;
            BigItemImg.sprite = elementItem.ItemSprite;
            BigItemName.text = elementItem.ItemName;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Open(tem item)
    {
        print(item.index+" "+item.ItemDescription+" "+item.ItemName+" "+item.type);
        elementItem = item;
        for (int i = 0; i < item.CompleteItem.Length; i++)
        {
            completeTemArray[i] = item.CompleteItem[i];
            completeTemArray[i].index = 1;
        }

        for (int i = item.CompleteItem.Length; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].index = 0;
        }
    }
}
