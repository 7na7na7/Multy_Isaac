using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    public tem[] completeTemArray;
    public Image BigItemImg;
    public Text BigItemName;
    public Image SmallItemImg1, SmallItemImg2, BigItemImg2;
    public Text SmallItemName1, SmallItemName2, BigItemName2;
    public Text WhereGet;
    public Image[] completeBoxes; //조합템 테두리
    public Image[] completes; //조합템
    public Text ItemName; //템이름 
    public Text ItemDescription; //템설명
    private ItemData ItemData;
    private Animator anim;
    public GameObject Big, Small;
    private tem element;
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

    public void SmallOpen(int index)
    {
        tem temm=new tem();
        temm.index = ItemData.GetItem(element.SmallItemIndex[index]).index;
        temm.type = ItemData.GetItem(element.SmallItemIndex[index]).type;
        temm.ItemDescription = ItemData.GetItem(element.SmallItemIndex[index]).ItemDescription;
        temm.ItemName= ItemData.GetItem(element.SmallItemIndex[index]).ItemName;
        temm.ItemSprite = ItemData.GetItem(element.SmallItemIndex[index]).ItemSprite;
        temm.CompleteItemIndex = ItemData.GetItem(element.SmallItemIndex[index]).CompleteItemIndex;
        temm.WhereGet = ItemData.GetItem(element.SmallItemIndex[index]).WhereGet;
        temm.SmallItemIndex = ItemData.GetItem(element.SmallItemIndex[index]).SmallItemIndex;
        Open(temm);
    }

    public void Open(tem taaaaam)
    {
        element = taaaaam;
        anim.Play("InvenOpen");
        
        for (int i = 0; i < element.CompleteItemIndex.Length; i++)
        {
            completeTemArray[i].index = ItemData.GetItem(element.CompleteItemIndex[i]).index;
            completeTemArray[i].type = ItemData.GetItem(element.CompleteItemIndex[i]).type;
            completeTemArray[i].ItemDescription = ItemData.GetItem(element.CompleteItemIndex[i]).ItemDescription;
            completeTemArray[i].ItemName = ItemData.GetItem(element.CompleteItemIndex[i]).ItemName;
            completeTemArray[i].ItemSprite = ItemData.GetItem(element.CompleteItemIndex[i]).ItemSprite;
            completeTemArray[i].CompleteItemIndex = ItemData.GetItem(element.CompleteItemIndex[i]).CompleteItemIndex;
            completeTemArray[i].WhereGet = ItemData.GetItem(element.CompleteItemIndex[i]).WhereGet;
            completeTemArray[i].SmallItemIndex = ItemData.GetItem(element.CompleteItemIndex[i]).SmallItemIndex;
        }

        for (int i = element.CompleteItemIndex.Length; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].index = 0;
        }
        
        
        for (int i = 0; i < completeTemArray.Length; i++)
        {
            if (completeTemArray[i].index != 0)
            {
                completeBoxes[i].gameObject.SetActive(true);
                completes[i].gameObject.SetActive(true);
                completes[i].sprite = completeTemArray[i].ItemSprite;
            }
            else
            {
                completes[i].gameObject.SetActive(false);
                completeBoxes[i].gameObject.SetActive(false);
            }
            
        }

        if (element != null)
        {
            if (element.SmallItemIndex.Length >= 2)
            {
                Big.SetActive(false);
                Small.SetActive(true);
                SmallItemImg1.sprite = ItemData.GetItem(element.SmallItemIndex[0]).ItemSprite;
                SmallItemImg2.sprite = ItemData.GetItem(element.SmallItemIndex[1]).ItemSprite;
                SmallItemName1.text = ItemData.GetItem(element.SmallItemIndex[0]).ItemName;
                SmallItemName2.text = ItemData.GetItem(element.SmallItemIndex[1]).ItemName;
                BigItemImg2.sprite = element.ItemSprite;
                BigItemName2.text = element.ItemName;
            }
            else
            {
                Big.SetActive(true);
                Small.SetActive(false);
                BigItemImg.sprite = element.ItemSprite;
                BigItemName.text = element.ItemName;
            }
            ItemName.text = element.ItemName;
            ItemDescription.text = element.ItemDescription;
          
            WhereGet.text ="획득 경로 : "+ element.WhereGet;
        }
    }
}
