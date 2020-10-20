using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    private tem elementItem;
    public Sprite NoneSprite;
    public Sprite BoxSprite;
    public Image BigItemImg;
    public Text BigItemName;
    public Image[] completeBoxes; //조합템 테두리
    public element[] completes;
    public Text ItemName; //템이름 
    public Text ItemDescription; //템설명
    private ItemData ItemData;
    private Animator anim;
    private void Start()
    {
        ItemData = FindObjectOfType<ItemData>();
        anim = GetComponent<Animator>();
    }
    
    public void Close()
    {
        anim.Play("InvenClose");
    }
    public void Open(tem item)
    {
        anim.Play("InvenOpen");
        elementItem = item;


//        for (int j = 0; j < completes.Length; j++)
//        {
//            completes[j].tem.Clear();
//        }

       
            for (int i = 0; i < item.CompleteItemIndex.Length; i++)
            {
                foreach (tem tem in ItemData.temDatas)
                {
                    if (tem.index == item.CompleteItemIndex[i])
                    {
                        completes[i].tem = tem;
                        break;
                    }
                }
            }
            for (int i = 0; i < completes.Length; i++)
            {
                if (completes[i].tem.index != 0)
                {
                    completeBoxes[i].sprite = BoxSprite;
                }
                else
                {
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
