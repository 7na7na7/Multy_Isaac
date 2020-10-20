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
    private ItemData ItemData;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ItemData = FindObjectOfType<ItemData>();
    }
    
    public void Close()
    {
        anim.Play("InvenClose");
    }
    public void Open(tem item)
    {
        anim.Play("InvenOpen");
        print(item.index+" "+item.ItemDescription+" "+item.ItemName+" "+item.type);
        elementItem = item;
        for (int i = 0; i < item.CompleteItemIndex.Length; i++)
        {
            print(ItemData.GetItem(i).ItemName);
            completeTemArray[i] = ItemData.GetItem(item.CompleteItemIndex[i]);
            //completeTemArray[i].index = 1;
        }

        for (int i = item.CompleteItemIndex.Length; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].index = 0;
        }
        
        
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
}
