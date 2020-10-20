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
            completeTemArray[i].SmallItemIndex = ItemData.GetItem(item.CompleteItemIndex[i]).SmallItemIndex;
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
            if (item.SmallItemIndex.Length >= 2)
            {
                Big.SetActive(false);
                Small.SetActive(true);
                SmallItemImg1.sprite = ItemData.GetItem(item.SmallItemIndex[0]).ItemSprite;
                SmallItemImg2.sprite = ItemData.GetItem(item.SmallItemIndex[1]).ItemSprite;
                SmallItemName1.text = ItemData.GetItem(item.SmallItemIndex[0]).ItemName;
                SmallItemName2.text = ItemData.GetItem(item.SmallItemIndex[1]).ItemName;
                BigItemImg2.sprite = item.ItemSprite;
                BigItemName2.text = item.ItemName;
            }
            else
            {
                Big.SetActive(true);
                Small.SetActive(false);
                BigItemImg.sprite = item.ItemSprite;
                BigItemName.text = item.ItemName;
            }
            ItemName.text = item.ItemName;
            ItemDescription.text = item.ItemDescription;
          
            WhereGet.text ="획득 경로 : "+ item.WhereGet;
        }
    }
}
