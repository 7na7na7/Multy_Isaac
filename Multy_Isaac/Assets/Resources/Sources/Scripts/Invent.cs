using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    public tem[] completeTemArray;
    public Sprite NoneSprite;
    public Sprite BoxSprite;
    public Image BigItemImg;
    public Text BigItemName;
    public Text WhereGet;
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

    public void CompleteOpen(int index)
    {
        Open(completeTemArray[index]);
    }
    public void Open(tem item)
    {
        anim.Play("InvenOpen");
        
        for (int i = 0; i < item.CompleteItemIndex.Length; i++)
        {
            completeTemArray[i].index = ItemData.GetItem(item.CompleteItemIndex[i]).index;
            completeTemArray[i].type = ItemData.GetItem(item.CompleteItemIndex[i]).type;
            completeTemArray[i].ItemDescription = ItemData.GetItem(item.CompleteItemIndex[i]).ItemDescription;
            completeTemArray[i].ItemName = ItemData.GetItem(item.CompleteItemIndex[i]).ItemName;
            completeTemArray[i].ItemSprite = ItemData.GetItem(item.CompleteItemIndex[i]).ItemSprite;
            completeTemArray[i].CompleteItemIndex = ItemData.GetItem(item.CompleteItemIndex[i]).CompleteItemIndex;
            completeTemArray[i].WhereGet = ItemData.GetItem(item.CompleteItemIndex[i]).WhereGet;
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

        if (item != null)
        {
            ItemName.text = item.ItemName;
            ItemDescription.text = item.ItemDescription;
            BigItemImg.sprite = item.ItemSprite;
            BigItemName.text = item.ItemName;
            WhereGet.text ="획득 경로 : "+ item.WhereGet;
        }
    }
}
