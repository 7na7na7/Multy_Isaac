using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    public bool isLobby = false;
    private playerCountSave pc;
    private bool isOpen = false;
    public Image[] categories;
    public GameObject[] scrollViews;
    public GameObject dicBtn;
    public GameObject XBtn;
    public GameObject Dic;
    public GameObject Com;
    public ParticleSystem starEffect;
    public RectTransform panel;
    private Player player;
    private Pause pause;
    public tem[] completeTemArray;
    public Image BigItemImg;
    public Text BigItemName;
    public Image SmallItemImg1, SmallItemImg2, BigItemImg2;
    public Text SmallItemName1, SmallItemName2, BigItemName2;
    private Text SmallType1, SmallType2, BigType2, BigType;
    public Text WhereGet;
    public Image[] completeBoxes; //조합템 테두리
    public Image[] completes; //조합템
    public Text ItemName; //템이름 
    public Text ItemDescription; //템설명
    private TemManager temMgr;
    private Animator anim;
    public GameObject Big, Small;
    public tem element;
    public Button InventBtn;
    private bool CanCombine = false;
    private PlayerItem PlayerItem;
    private void Start()
    {
        pc=playerCountSave.instance;
        if(!isLobby) 
            player = transform.parent.gameObject.transform.parent.gameObject.GetComponent<Player>();
        temMgr = FindObjectOfType<TemManager>();
        pause = FindObjectOfType<Pause>();
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
        
        BigType = BigItemName.transform.GetChild(0).GetComponent<Text>();
        BigType2 = BigItemName2.transform.GetChild(0).GetComponent<Text>();
        SmallType1 = SmallItemName1.transform.GetChild(0).GetComponent<Text>();
            SmallType2 = SmallItemName2.transform.GetChild(0).GetComponent<Text>();
    }

    public void CategoryTem(int index)
    {
        for (int i = 0; i < categories.Length; i++)
        {
            if (i == index)
            {
                Color color = categories[i].color;
                color.a = 0.5f;
                categories[i].color = color;
                scrollViews[i].SetActive(true);
            }
            else
            {
                Color color2 = categories[i].color;
                color2.a = 1f;
                categories[i].color = color2;
                scrollViews[i].SetActive(false);   
            }
        }
    }
    public void DicOpen()
    {
        dicBtn.SetActive(false);
        if(!isLobby) 
            XBtn.SetActive(true);
        Dic.SetActive(true);
        Com.SetActive(false);
    }
    
    public void DicClose()
    {
        if (isLobby)
        {
            dicBtn.SetActive(true);
            Com.SetActive(true);
        }
        else
        {
            dicBtn.SetActive(true);
            XBtn.SetActive(false);
            Com.SetActive(true);
        }
      
    }
    public void Close()
    {
        isOpen = false;
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
            if (isLobby)
                return;
            
            if(Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            if (element.SmallItemIndex.Length != 0)
            {
                if (PlayerItem.GetItemArray(element.SmallItemIndex[0]).index == PlayerItem.GetItemArray(element.SmallItemIndex[1]).index) //둘이 똑같은템 조합이면
                {
                    if (PlayerItem.GetItemArray(element.SmallItemIndex[0]).type == itemType.Usable ||PlayerItem.GetItemArray(element.SmallItemIndex[0]).type == itemType.Item)
                    {
                        if (PlayerItem.GetUsableItemCount(element.SmallItemIndex[0]) >= 2)
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
                        if (PlayerItem.GetItemCount(element.SmallItemIndex[0]) >= 2)
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
                }
                else
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
        tem tempTem = temMgr.GetItemList(element.SmallItemIndex[index]);
        temm = tempTem.DeepCopy();
        Open(temm);
    }

    public void OpenIfCan(tem tam)
    {
        if (tam.ItemName != "")
        {
            if (element != null)
            {
                if(PlayerItem.getCurrentTem().index==element.index && isOpen)
                    Close();
                else
                    Open(tam);
            }
            else
            {
                Open(tam);
            }   
        }
        else
        {
            Close();
        }

    }
    public void Open(tem taaaaam)
    {
        if (!isLobby)
        {
            if (XBtn.activeSelf)
                XBtn.SetActive(false);
            if(player.isPlay) 
                pause.canPause = false;
        }
        if(!dicBtn.activeSelf)
            dicBtn.SetActive(true);
        if(!Com.activeSelf)
            Com.SetActive(true);
        if(Dic.activeSelf) 
            Dic.SetActive(false);
        element = taaaaam;
        isOpen = true;
        anim.Play("InvenOpen");
        tem tem;
        
        for (int i = 0; i < completeTemArray.Length; i++)
        {
            completeTemArray[i].canSee =true;
        }
        
        for (int i = 0; i < element.CompleteItemIndex.Length; i++)
        {
            tem = temMgr.GetItemList(element.CompleteItemIndex[i]);
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
                SmallItemImg1.sprite = temMgr.GetItemList(element.SmallItemIndex[0]).ItemSprite;
                SmallItemImg2.sprite = temMgr.GetItemList(element.SmallItemIndex[1]).ItemSprite;
                if(pc.isKor()) 
                    SmallItemName1.text = temMgr.GetItemList(element.SmallItemIndex[0]).ItemName;
                else
                    SmallItemName1.text = temMgr.GetItemList(element.SmallItemIndex[0]).ItemName2;
                SmallType1.text = getType(temMgr.GetItemList(element.SmallItemIndex[0]).type);
                if(pc.isKor()) 
                    SmallItemName2.text = temMgr.GetItemList(element.SmallItemIndex[1]).ItemName;
                else
                    SmallItemName2.text = temMgr.GetItemList(element.SmallItemIndex[1]).ItemName2;
                SmallType2.text = getType(temMgr.GetItemList(element.SmallItemIndex[1]).type);
                BigItemImg2.sprite = element.ItemSprite;
                if(pc.isKor()) 
                    BigItemName2.text = element.ItemName;
                else
                    BigItemName2.text = element.ItemName2;
                BigType2.text = getType(element.type);
            }
            else
            {
                Big.SetActive(true);
                Small.SetActive(false);
                BigItemImg.sprite = element.ItemSprite;
                if (pc.isKor())
                    BigItemName.text = element.ItemName;
                else
                    BigItemName.text = element.ItemName2;
                BigType.text = getType(element.type);
            }
            if(pc.isKor()) 
                ItemName.text = element.ItemName;
            else
                ItemName.text = element.ItemName2;
            
            if(pc.isKor()) 
                ItemDescription.text = element.ItemDescription;
            else
                ItemDescription.text = element.ItemDescription2;
            if(pc.isKor()) 
                WhereGet.text ="획득 경로 : "+ element.WhereGet;
            else
                WhereGet.text ="Can get at : "+ element.WhereGet2;
        }
    }

    string getType(itemType type)
    {
        switch (type)
        {
            case itemType.Gun:
                if (pc.isKor())
                    return "(원거리 무기)";
                else
                    return "(Gun)";
            case itemType.Item:
                if (pc.isKor())
                return "(재료 아이템)";
                else
                return "(Material)";
            case itemType.Melee:
                if (pc.isKor())
                return "(근접 무기)";
                else
                return "(Melee)";
            case itemType.Passive:
                if (pc.isKor())
                return "(패시브 아이템)";
                else
                return "(Passive)";
            case itemType.Usable:
                if (pc.isKor())
                return "(소모성 아이템)";
                else
                return "(Usable)";
            default:
                return "ERROR";
        }
    }
    public void Combine()
    {
        if (CanCombine)
        {
            tem item=new tem();
            item = element.DeepCopy();
            bool canGet = false;
            if (item.index != 95)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    PlayerItem.DestroyItem(element.SmallItemIndex[0]);
                    PlayerItem.DestroyItem(element.SmallItemIndex[1]);
                    PlayerItem.CombineItem(item.DeepCopy());
                    canGet = true;
                }
                    
            }
            else
            {
                PlayerItem.DestroyItem(element.SmallItemIndex[0]);
                PlayerItem.DestroyItem(element.SmallItemIndex[1]);
                transform.parent.transform.parent.GetComponent<Player>().leftBullet.GetBullet(30);
                canGet = true;
            }

            if (canGet)
            {
                if (player.PlayerIndex == 6)
                {
                    transform.parent.transform.parent.GetComponent<Player>().leftBullet.GetBullet(20);
                }
                Open(item);
                starEffect.Play();
                player.CombineSound();   
            }
        }
    }
    public void CombineClick(tem t)
    {
        tem item=new tem();
            item = t;
            bool canGet = false;
            if (item.index != 95)
            {
                PlayerItem.DestroyItem(t.SmallItemIndex[0]);
                PlayerItem.DestroyItem(t.SmallItemIndex[1]);
                canGet = true;
                for (int i = 0; i < item.Count; i++)
                {
                    PlayerItem.CombineItem(item.DeepCopy());
                }
                    
            }
            else
            {
                PlayerItem.DestroyItem(t.SmallItemIndex[0]);
                PlayerItem.DestroyItem(t.SmallItemIndex[1]);
                transform.parent.transform.parent.GetComponent<Player>().leftBullet.GetBullet(30);
                canGet = true;
            }

            if (canGet)
            {
                if (player.PlayerIndex == 6)
                {
                    transform.parent.transform.parent.GetComponent<Player>().leftBullet.GetBullet(20);
                }
                //Open(item);
                starEffect.Play();
                player.CombineSound();   
            }
        
    }
}
