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

    private void Start()
    {
        ItemData = FindObjectOfType<ItemData>();
        completeTemArray[0] = ItemData.temDatas[2]; //오류 하나 내줘서 첨에 제대로 안나오는거 고침 ㅋ
    }

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
        elementItem = item;
        for (int i = 0; i < item.CompleteItemIndex.Length; i++)
        {
            completeTemArray[i] = ItemData.temDatas[item.CompleteItemIndex[i]];
        }

        for (int i = item.CompleteItemIndex.Length; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].index = 0;
        }
    }
}
