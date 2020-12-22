using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    public RectTransform panel;
    
    
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
    public ItemData ItemData;
    private Animator anim;
    public GameObject Big, Small;
    public tem element;
    public Button InventBtn;
    private bool CanCombine = false;
    private PlayerItem PlayerItem;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.pv.IsMine)
            {
                PlayerItem = p.GetComponent<PlayerItem>();
                break;
            }
        }  
    }
    
    public void Close()
    {
        anim.Play("InvenClose");
    }

    public void CompleteOpen(int index)
    {
        Open(completeTemArray[index]);
    }

    private void Update()
    {
        if(element!=null)
        {
            if(Input.GetMouseButtonDown(1) &&RectTransformUtility.RectangleContainsScreenPoint(panel, Input.mousePosition) )
                Close();
            if (element.SmallItemIndex.Length != 0)
            {
                if (PlayerItem.GetItemArray(element.SmallItemIndex[0]).index != 0 &&PlayerItem.GetItemArray(element.SmallItemIndex[1]).index != 0)
                {
                    InventBtn.interactable = true;
                    CanCombine = true;
                }
                else
                {
                    InventBtn.interactable = false;
                    CanCombine = false;
                }      
            }
            else
            {
                InventBtn.interactable = false;
                CanCombine = false;
            }
        }
    }

    public void SmallOpen(int index)
    {
        tem temm=new tem();
        tem tempTem = ItemData.GetItemList(element.SmallItemIndex[index]);
        temm = tempTem.DeepCopy();
        Open(temm);
    }
    
    public void Open(tem taaaaam)
    {
        element = taaaaam;
        anim.Play("InvenOpen");
        tem tem;
        
        for (int i = 0; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].canSee =true;
        }
        
        for (int i = 0; i < element.CompleteItemIndex.Length; i++)
        {
            tem = ItemData.GetItemList(element.CompleteItemIndex[i]);
            completeTemArray[i] = tem.DeepCopy();
        }

        for (int i = element.CompleteItemIndex.Length; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].canSee = false;
        }
        
        
        for (int i = 0; i < completeTemArray.Length; i++)
        {
            if (completeTemArray[i].canSee)
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
                SmallItemImg1.sprite = ItemData.GetItemList(element.SmallItemIndex[0]).ItemSprite;
                SmallItemImg2.sprite = ItemData.GetItemList(element.SmallItemIndex[1]).ItemSprite;
                SmallItemName1.text = ItemData.GetItemList(element.SmallItemIndex[0]).ItemName;
                SmallItemName2.text = ItemData.GetItemList(element.SmallItemIndex[1]).ItemName;
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

    public void Combine()
    {
        if (CanCombine)
        {
            PlayerItem.GetItemArray(element.SmallItemIndex[0]).Clear();
            PlayerItem.GetItemArray(element.SmallItemIndex[1]).Clear();
            tem item=new tem();
            item = element.DeepCopy();
           
            PlayerItem.GetItem(item);
            Open(item);
        }
    }
}
