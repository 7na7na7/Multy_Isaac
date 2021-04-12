using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerItem : MonoBehaviour
{
    private playerCountSave pc;
    public Sprite noneSprite;
    public int[] indexes;
    private tem t = null;
    private bool cango = true;
    public Image[] canItems;
    public Invent invent;
    public GameObject[] startTem;
    public int[] startTemCount;
    
    private float time = 0f;
    
    //아이템
    public float itemRadious;
    public LayerMask itemLayer;
    public tem[] ItemList;
    public Image[] ItemBoxes;
    public Player player;
    public Sprite NullSprite;
    public ItemSlot[] slots;
    public GameObject[] Selected;
    private TemManager temMgr;
    public UsableItem usable;
    public int selectedIndex = 0;

    private void Awake()
    {
        indexes=new int[canItems.Length];
        temMgr = FindObjectOfType<TemManager>();
        if (PhotonNetwork.OfflineMode)
        {
            if(SceneManager.GetActiveScene().name=="Play") 
                Invoke("StartTem",FindObjectOfType<ZombieSpawner>().FirstDelay);   
        }
        else
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                if(SceneManager.GetActiveScene().name=="Play")
                    Invoke("StartTem",FindObjectOfType<ZombieSpawner>().FirstDelay);
            }   
        }
        pc=playerCountSave.instance;
    }

    public tem getCurrentTem()
    {
        return ItemList[selectedIndex];
    }
    void StartTem()
    {
        for (int j = 0; j < startTem.Length;j++) 
        {
            for (int k = 0; k < startTemCount[j]; k++)
            {
                tem item=new tem();
                item = startTem[j].GetComponent<Item>().item.DeepCopy();
                GetItem(item);
            }
        }
    }

    public void ranTem()
    {
        DestroyItem(ItemList[0].index);
        List<tem> weapons=new List<tem>();
        foreach (tem tt in temMgr.temDatas)
        {
            if(tt.weaponIndex!=0)
                weapons.Add(tt.DeepCopy());
        }
        tem t = weapons[UnityEngine.Random.Range(0, weapons.Count)].DeepCopy();
        GetItem(t);
    }
    private void Update()
    {
        if (player != null)
        {
            if (player.pv.IsMine)
            {
                if (player.isPlay)
                {
                      float scroll = Input.GetAxis("Mouse ScrollWheel");
                      if (scroll != 0)
                                 {
                                     if (scroll < 0)
                                     {
                                         if (selectedIndex >= Selected.Length-1)
                                             selectedIndex = 0;
                                         else
                                             selectedIndex++;
                                    
                                     
                                         player.ChangeWeaponSound();
                                         for (int i = 0; i < Selected.Length; i++) //현재 인텍스에만 선택창 달아줌
                                         {
                                             if (i == selectedIndex)
                                                 Selected[i].SetActive(true);
                                             else
                                                 Selected[i].SetActive(false); 
                                         }
                                         check(selectedIndex,false);   
                                     
                                     }
                                     else
                                     {
                                         if (selectedIndex <= 0)
                                             selectedIndex = Selected.Length-1;
                                         else
                                             selectedIndex--;
                                    
                                     
                                         player.ChangeWeaponSound();
                                         for (int i = 0; i < Selected.Length; i++) //현재 인텍스에만 선택창 달아줌
                                         {
                                             if (i == selectedIndex)
                                                 Selected[i].SetActive(true);
                                             else
                                                 Selected[i].SetActive(false); 
                                         }
                                         check(selectedIndex,false);   
                                     }   
                                 } //스크롤 템전환   
                }

                for (int i = 0; i < ItemList.Length; i++) //아이템이미지가 존재한다면 매 프레임마다 박스에 이미지 갱신
                {
                    if(ItemList[i].ItemSprite!=null) 
                        ItemBoxes[i].sprite = ItemList[i].ItemSprite;
                    else
                        ItemBoxes[i].sprite = NullSprite;
                }

                if (Input.GetMouseButtonDown(1)) //우클릭으로 템조합하기
                {
                    invent.OpenIfCan(ItemList[selectedIndex]);
                }
                if (player.canMove) //움직일 수 있는 상태에서만 입력 가능
                {
                    if (Input.GetKeyDown(KeyCode.E)) //아이템 사용
                    {
                        if (ItemList[selectedIndex].type == itemType.Usable)
                        {
                            if (usable.UseItem(ItemList[selectedIndex].index))
                            {
                                slots[selectedIndex].itemCount--;
                                if(slots[selectedIndex].itemCount<=0)
                                    ItemList[selectedIndex].Clear();      
                            }
                            else
                            {
                               if(pc.isKor()) 
                                   PopUpManager.instance.PopUp("회복아이템은 "+usable.getCool()+"초 후 재사용 가능", Color.cyan);
                               else
                                   PopUpManager.instance.PopUp("Can use food "+usable.getCool()+"sec after", Color.cyan);
                            }
                        }
                    }
                    
                    if (Input.GetKeyDown(KeyCode.Space)) //스페이스바로 줍기
                    {
                        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, itemRadious, itemLayer);

                        try
                        {
                            if (items[0] != null)
                            {
                                items.OrderBy(c => c.transform.position - transform.position);
                            
                                Collider2D item = items[0];
                                Item it = item.GetComponent<Item>();
                                bool canGo = false;
                                if (it.isShopTem)
                                {
                                    if (player.leftBullet.GetedBulletCount() >= it.price)
                                    {
                                        canGo = true;
                                        player.leftBullet.GetBulletMinus(it.price);
                                        player.purchaseSound();
                                    }
                                    else
                                    {
                                        if(pc.isKor()) 
                                            PopUpManager.instance.PopUp("총알이 부족합니다!",Color.red);
                                        else
                                            PopUpManager.instance.PopUp("Lack of Bullet!",Color.red);
                                    }
                                }
                                else
                                {
                                    canGo = true;
                                }

                                if (canGo)
                                {
                                                                     bool isGet = false;

                                if (it.item.type == itemType.Usable||it.item.type == itemType.Item) //소비템이면
                                {
                                    bool isHaveUsable = false;
                                    
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == it.item.ItemName) //이름이 같은 소비템이면
                                        {
                                            isHaveUsable = true;
                                            isGet = true;
                                            slots[i].itemCount++;
                                        
                                            check(i,false);

                                            if (!it.isShopTem)
                                            {
                                                temMgr.delTem(it.Index); 
                                                Destroy(item);   
                                            }
                                            else
                                            {
                                                it.Del();
                                            }
                                            break;
                                        }
                                    }

                                    if (!isHaveUsable) //소비템이 없으면
                                    {
                                        for (int i = 0; i < ItemList.Length; i++) 
                                        {
                                            if (ItemList[i].ItemName == "") //빈곳에 템넣어줌
                                            {
                                                isGet = true;
                                                ItemList[i]=it.item;
                                                slots[i].itemCount++;
                                        
                                                check(i,true);
                                        
                                                if (!it.isShopTem)
                                                {
                                                    temMgr.delTem(it.Index);
                                                    Destroy(item);  
                                                }
                                                else
                                                {
                                                   it.Del();
                                                }
                                               
                                                break;
                                            }
                                        }      
                                    }
                                }
                                else//소비템이 아니면 
                                {
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == "") //빈곳에 템넣어줌
                                        {
                                            isGet = true;
                                            ItemList[i]=it.item;
                                            
                                            if(it.item.type==itemType.Passive) //패시브템이면
                                                player.PassiveOn(it.item.index); //패시브 ON

                                            check(i,true);
                                        
                                            if (!it.isShopTem)
                                            {
                                                temMgr.delTem(it.Index);
                                                Destroy(item);  
                                            }
                                            else
                                            {
                                                it.Del();
                                            }
                                            break;
                                        }
                                    }   
                                }

                                if (!isGet)
                                {
                                 if(pc.isKor())
                                     PopUpManager.instance.PopUp("더 이상 주울 수 없습니다!",Color.red);
                                 else
                                     PopUpManager.instance.PopUp("Can't pick up item any more!'",Color.red);
                                }
                                else
                                    player.GetSound();   
                                }
                            }   
                        }
                        catch (Exception e)
                        { }
                    }

                    if (player.isPlay)
                    {
                        //1부터 6으로 아이템 선택 가능
                        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) ||
                            Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) ||
                            Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6)
                            || Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Alpha8)
                            || Input.GetKeyDown(KeyCode.Alpha9))
                        {
                            int contactIndex = 0;
                            if (Input.GetKeyDown(KeyCode.Alpha1)) //1
                                contactIndex = 0;
                            else if (Input.GetKeyDown(KeyCode.Alpha2)) //2
                                contactIndex = 1;
                            else if (Input.GetKeyDown(KeyCode.Alpha3)) //3
                                contactIndex = 2;
                            else if (Input.GetKeyDown(KeyCode.Alpha4)) //4
                                contactIndex = 3;
                            else if (Input.GetKeyDown(KeyCode.Alpha5)) //5
                                contactIndex = 4;
                            else if (Input.GetKeyDown(KeyCode.Alpha6)) //6
                                contactIndex = 5;
                            else if (Input.GetKeyDown(KeyCode.Alpha7)) //7
                                contactIndex = 6;
                            else if (Input.GetKeyDown(KeyCode.Alpha8)) //8
                            {
                                contactIndex = 7;
                            }
                            else if (Input.GetKeyDown(KeyCode.Alpha9)) //9
                            {
                                if (player.PlayerIndex == 3)
                                {
                                    contactIndex = 8;
                                }
                            }

                            if (contactIndex != selectedIndex)
                            {
                                player.ChangeWeaponSound();
                                selectedIndex = contactIndex;
                                for (int i = 0; i < Selected.Length; i++) //현재 인텍스에만 선택창 달아줌
                                {
                                    if (i == selectedIndex)
                                        Selected[i].SetActive(true);
                                    else
                                        Selected[i].SetActive(false);
                                }

                                check(selectedIndex, false);
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Q) && player.isPlay) //템 버리기
                    {
                        DiscardItem(false);
                    }
                }

                if(player.isPlay) 
                    canTem();
            }   
        }
        else
        {
            Player[] players = FindObjectsOfType<Player>();
            foreach (Player p in players)
            {
                if (p.pv.IsMine)
                {
                    player = p;
                    break;
                }
            }
        }
    }

    public void Swap(int index1, int index2)
    {
        if (index1 != index2)
        {
            tem tempTem = ItemList[index1].DeepCopy();
            ItemList[index1] = ItemList[index2].DeepCopy();
            ItemList[index2] = tempTem;
            int tempCount = slots[index1].itemCount;
            slots[index1].itemCount = slots[index2].itemCount;
            slots[index2].itemCount = tempCount;
            check(selectedIndex,true);
        }
    }
    void canTem()
    {
                      for (int q = 0; q < indexes.Length; q++)
                    indexes[q] = 0;
                
                for (int i = 0; i < ItemList.Length; i++)
                {
                    if (ItemList[i].ItemSprite != null&&ItemList[i].CompleteItemIndex.Length > 0)
                    {
                        for (int j = 0; j < ItemList[i].CompleteItemIndex.Length; j++)
                        {
                            t = temMgr.GetItemList(ItemList[i].CompleteItemIndex[j]);
                            cango = true;
                            if (temMgr.GetItemList(t.SmallItemIndex[0]).index ==
                                temMgr.GetItemList(t.SmallItemIndex[1]).index) //하위템 두개가 같은재료면
                            {
                                if (temMgr.GetItemList(t.SmallItemIndex[0]).type == itemType.Item || temMgr.GetItemList(t.SmallItemIndex[0]).type == itemType.Usable)
                                {
                                    if (slots[returnCurrentTem(temMgr.GetItemList(t.SmallItemIndex[0]).index)].itemCount >= 2) //2개이상 있으면
                                    {
                                        foreach (int w in indexes)
                                        {
                                            if (w == t.index)
                                            {
                                                cango = false;
                                            }
                                        }
                                    
                                        if (cango)
                                        {
                                            for (int k = 0; k < indexes.Length; k++)
                                            {
                                                if (indexes[k] == 0)
                                                {
                                                    indexes[k] = t.index;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int a = 0;
                                 
                                    foreach (tem temm in ItemList)
                                    {
                                        if(temm.index==t.SmallItemIndex[0]) 
                                            a++;
                                    }
                                
                                    if (a>=2) //2개이상 있으면
                                    {
                                        foreach (int w in indexes)
                                        {
                                            if (w == t.index)
                                            {
                                                cango = false;
                                            }
                                        }
                                    
                                        if (cango)
                                        {
                                            for (int k = 0; k < indexes.Length; k++)
                                            {
                                                if (indexes[k] == 0)
                                                {
                                                    indexes[k] = t.index;
                                                    break;
                                                }
                                            }
                                        }
                                    }   
                                }
                            }
                            else
                            {
                                if (GetItemArray(t.SmallItemIndex[0]).index != 0 && GetItemArray(t.SmallItemIndex[1]).index != 0)//하위템 두개다 갖고있으면
                                {
                                    foreach (int w in indexes)
                                    {
                                        if (w == t.index)
                                        {
                                            cango = false;
                                            break;
                                        }
                                    }
                                   
                                    if (cango)
                                    {
                                        for (int k = 0; k < indexes.Length; k++)
                                        {
                                            if (indexes[k] == 0)
                                            {
                                                indexes[k] = t.index;
                                                break;
                                            }
                                        }
                                    }
                                }   
                            }
                        }   
                    }

                  
                    for (int a = 0; a < canItems.Length; a++)
                    {
                        if (indexes[a] != 0)
                        {
                            canItems[a].sprite = temMgr.GetItemList(indexes[a]).ItemSprite;
                            canItems[a].GetComponent<CanItem>().index = indexes[a];
                        }
                        else
                        {
                            canItems[a].sprite = noneSprite;
                            canItems[a].GetComponent<CanItem>().index = 0;
                        }
                    }
                        
                }
    }
    public void GetItem(tem item)
    {
        bool isGet = false;
          int select = 0;
                                if (item.type == itemType.Usable||item.type==itemType.Item) //소비템 또는 재료템이면
                                {
                                    bool isHaveUsable = false;
                                    
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == item.ItemName) //이름이 같으면
                                        {
                                            select = i;
                                            isHaveUsable = true;
                                            isGet = true;
                                            slots[i].itemCount++;
                                        
                                            check(i,false);
                                            
                                            break;
                                        }
                                    }

                                    if (!isHaveUsable) //소비템이 없으면
                                    {
                                        for (int i = 0; i < ItemList.Length; i++)
                                        {
                                            if (ItemList[i].ItemName == "") //빈곳에 템넣어줌
                                            {
                                                select = i;
                                                isGet = true;
                                                ItemList[i]=item;
                                                slots[i].itemCount++;
                                        
                                                check(i,true);
                                                
                                                break;
                                            }
                                        }      
                                    }
                                }
                                else//소비템도 재료템도 아니면 
                                {
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == "") //빈곳에 템넣어줌
                                        {
                                            select = i;
                                            isGet = true;
                                            ItemList[i]=item;
                                            
                                            if(item.type==itemType.Passive) //패시브템이면
                                                player.PassiveOn(item.index); //패시브 ON

                                            check(i,true);
                                            
                                            break;
                                        }
                                    }   
                                }

                                if (!isGet)
                                {
                                    if(pc.isKor()) 
                                        PopUpManager.instance.PopUp("더 이상 제작할 수 없습니다!",Color.red);
                                    else
                                        PopUpManager.instance.PopUp("Can't Invent any more!",Color.red);
                                }
                                else
                                {
                                    selectedIndex = select;
                                    for (int i = 0; i < Selected.Length; i++) //현재 인텍스에만 선택창 달아줌
                                    {
                                        if (i == selectedIndex)
                                            Selected[i].SetActive(true);
                                        else
                                            Selected[i].SetActive(false); 
                                    }
                                    check(selectedIndex,false);
                                }
    }
     public void CombineItem(tem item)
    {
        bool isGet = false;
          int select = 0;
                                if (item.type == itemType.Usable||item.type==itemType.Item) //소비템 또는 재료템이면
                                {
                                    bool isHaveUsable = false;
                                    
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == item.ItemName) //이름이 같으면
                                        {
                                            select = i;
                                            isHaveUsable = true;
                                            isGet = true;
                                            slots[i].itemCount++;
                                        
                                            check(i,false);
                                            
                                            break;
                                        }
                                    }

                                    if (!isHaveUsable) //소비템이 없으면
                                    {
                                        for (int i = 0; i < ItemList.Length; i++)
                                        {
                                            if (ItemList[i].ItemName == "") //빈곳에 템넣어줌
                                            {
                                                select = i;
                                                isGet = true;
                                                ItemList[i]=item;
                                                slots[i].itemCount++;
                                        
                                                check(i,true);
                                                
                                                break;
                                            }
                                        }      
                                    }
                                }
                                else//소비템도 재료템도 아니면 
                                {
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == "") //빈곳에 템넣어줌
                                        {
                                            select = i;
                                            isGet = true;
                                            ItemList[i]=item;
                                            
                                            if(item.type==itemType.Passive) //패시브템이면
                                                player.PassiveOn(item.index); //패시브 ON

                                            check(i,true);
                                            
                                            break;
                                        }
                                    }   
                                }

                                if (!isGet)
                                {
                                    discardRPC(item.index);
                                }
                                else
                                {
                                    selectedIndex = select;
                                    for (int i = 0; i < Selected.Length; i++) //현재 인텍스에만 선택창 달아줌
                                    {
                                        if (i == selectedIndex)
                                            Selected[i].SetActive(true);
                                        else
                                            Selected[i].SetActive(false); 
                                    }
                                    check(selectedIndex,false);
                                }
    }

    public void check(int i, bool isFirst)
    {
        player.KillReload();
        if (ItemList[i].type == itemType.Gun || ItemList[i].type == itemType.Melee) //아이템타입이 총이나 무기면 무기들려줌
        {
            if (ItemList[i].weaponIndex>0 && selectedIndex==i) 
            {
                player.changeWeapon(temMgr.GetWeapon(ItemList[selectedIndex].weaponIndex),isFirst); 
            }
            else
            {
                if(selectedIndex==i) 
                    player.gunSetfalse();
            }
        }
        else //아니면 무기없앰
        {
            if(selectedIndex==i) 
                player.gunSetfalse();
        }
    }

    public int GetUsableItemCount(int Index)
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].index == Index)
                return slots[i].itemCount;
        }

        return 0;
    }

    public int GetItemCount(int Index)
    {
        int count = 0;
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].index == Index)
                count++;
        }

        return count;
    }
    public tem GetItemArray(int Index)
    {
        tem tem = new tem();
        foreach (tem item in ItemList)
        {
            if (item.index == Index)
            {
                tem = item.DeepCopy();
                break;
            }
        }
        if (tem != null)
        {
            return tem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            tem.index = 0;
            return tem;
        }
    }
    public int returnCurrentTem(int Index)
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].index == Index)
            {
                return i;
            }
        }
        return 0;
    }
    public void DestroyItem(int index)
    {
        int inx = -1;
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].index == index)
            {
                inx = i;
                break;
            }
        }

        if (inx != -1)
        {
            if (ItemList[inx].type == itemType.Usable||ItemList[inx].type == itemType.Item) //소비템이면
            {
                slots[inx].itemCount--;
                if (slots[inx].itemCount <= 0)
                    ItemList[inx].Clear();
            }
            else //소비템 아니면
            {
                if (ItemList[inx].weaponIndex > 0) //무기를 버렸으면
                {
                    player.leftBullet.GetBullet(player.leftBullet.leftBullets[inx]);
                    player.gunSetfalse();
                }

                if (ItemList[inx].type == itemType.Passive) //패시브 아이템을 버렸으면
                    player.PassiveOff(ItemList[inx].index); //패시브 비활성화
                ItemList[inx].Clear();
            }   
        }
    }
    public void DiscardItem(bool isDead=false)
    {
        if (ItemList[selectedIndex].ItemSprite != null&&ItemList[selectedIndex].ItemSprite != NullSprite) //비어있지않다면
        {
            player.discardSound();
            int ind = ItemList[selectedIndex].index;

            if (ItemList[selectedIndex].type == itemType.Usable||ItemList[selectedIndex].type ==itemType.Item) //소비템이면
            {
                slots[selectedIndex].itemCount--;
                if(slots[selectedIndex].itemCount<=0)
                    ItemList[selectedIndex].Clear();   
            }
            else //소비템 아니면
            {
                if (ItemList[selectedIndex].weaponIndex > 0) //무기를 버렸으면
                {
                    player.leftBullet.GetBullet(player.leftBullet.leftBullets[selectedIndex]);
                    player.gunSetfalse();   
                }
                if(ItemList[selectedIndex].type==itemType.Passive) //패시브 아이템을 버렸으면
                    player.PassiveOff(ItemList[selectedIndex].index); //패시브 비활성화
                ItemList[selectedIndex].Clear();   
            }
            
            discardRPC(ind,isDead);
        }
    }
    
    public void Dead(bool ismain=false)
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (!ismain)
            {
                if (ItemList[i].index != 0)
                {
                    discardRPC(ItemList[i].index,true);
                }
            }
            ItemList[i].Clear();
        }
    }
    
    void discardRPC(int TemIndex, bool isDead = false)
    {
        Vector2 pos=new Vector2();
        if (isDead)
        {
            pos=new Vector2(transform.position.x+UnityEngine.Random.Range(-1f,1f),transform.position.y+UnityEngine.Random.Range(-1f,1f));
        }
        else
        {
            pos = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.2f, 0.2f),
                transform.position.y + UnityEngine.Random.Range(-0.3f, 0.3f));

        } 
        
        temMgr.setTem(TemIndex,pos);
       
    }
}
