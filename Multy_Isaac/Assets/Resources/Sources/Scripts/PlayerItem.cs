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
    public GameObject[] startTem;
    public int[] startTemCount;
    
    public float discardTime = 1f;
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
    
    public int selectedIndex = 0;

    private void Start()
    {
        temMgr = FindObjectOfType<TemManager>();
        if(SceneManager.GetActiveScene().name=="Play") 
            Invoke("StartTem",FindObjectOfType<ZombieSpawner>().FirstDelay);
    }
    
    void StartTem()
    {
        for (int j = 0; j < startTem.Length;j++) 
        {
            for (int k = 0; k < startTemCount[j]; k++)
            {
                if (startTem[j].GetComponent<Item>().item.type == itemType.Usable) //소비템이면
            {
                bool isHaveUsable = false;
                for (int i = 0; i < ItemList.Length; i++) 
                {
                    if (ItemList[i].ItemName ==startTem[j].GetComponent<Item>().item.ItemName) //이름이 같은 소비템이면
                    {
                        isHaveUsable = true;
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
                                                ItemList[i]=startTem[j].GetComponent<Item>().item;
                                                slots[i].itemCount++;
                                        
                                                check(i,true);
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
                                            ItemList[i]=startTem[j].GetComponent<Item>().item;
                                            
                                            if(startTem[j].GetComponent<Item>().item.type==itemType.Passive) //패시브템이면
                                                player.PassiveOn(startTem[j].GetComponent<Item>().item.index); //패시브 ON

                                            check(i,true);
                                            break;
                                        }
                                    }   
                                }      
            }
        }
    }
    private void Update()
    {
        if (player != null)
        {
            if (player.pv.IsMine)
            {
                //print(ItemList[0].index+" "+ItemList[1].index+" "+ItemList[2].index+" "+ItemList[3].index+" "+ItemList[4].index+" "+ItemList[5].index);
            
                for (int i = 0; i < ItemList.Length; i++) //아이템이미지가 존재한다면 매 프레임마다 박스에 이미지 갱신
                {
                    if(ItemList[i].ItemSprite!=null) 
                        ItemBoxes[i].sprite = ItemList[i].ItemSprite;
                    else
                        ItemBoxes[i].sprite = NullSprite;
                }
                
                if (player.canMove) //움직일 수 있는 상태에서만 입력 가능
                {
                    if (Input.GetKeyDown(KeyCode.E)) //아이템 사용
                    {
                        if (ItemList[selectedIndex].type == itemType.Usable)
                        {
                            player.UseItem(ItemList[selectedIndex].index);
                            slots[selectedIndex].itemCount--;
                            if(slots[selectedIndex].itemCount<=0)
                                ItemList[selectedIndex].Clear();   
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

                            
                                bool isGet = false;

                                if (item.GetComponent<Item>().item.type == itemType.Usable) //소비템이면
                                {
                                    bool isHaveUsable = false;
                                    
                                    for (int i = 0; i < ItemList.Length; i++) 
                                    {
                                        if (ItemList[i].ItemName == item.GetComponent<Item>().item.ItemName) //이름이 같은 소비템이면
                                        {
                                            isHaveUsable = true;
                                            isGet = true;
                                            slots[i].itemCount++;
                                        
                                            check(i,false);
                                        
                                            temMgr.delTem(item.GetComponent<Item>().Index); 
                                            Destroy(item);
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
                                                ItemList[i]=item.GetComponent<Item>().item;
                                                slots[i].itemCount++;
                                        
                                                check(i,true);
                                        
                                                temMgr.delTem(item.GetComponent<Item>().Index);
                                                Destroy(item);
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
                                            ItemList[i]=item.GetComponent<Item>().item;
                                            
                                            if(item.GetComponent<Item>().item.type==itemType.Passive) //패시브템이면
                                                player.PassiveOn(item.GetComponent<Item>().item.index); //패시브 ON

                                            check(i,true);
                                        
                                            temMgr.delTem(item.GetComponent<Item>().Index);
                                            Destroy(item);
                                            break;
                                        }
                                    }   
                                }

                                if(!isGet) 
                                    PopUpManager.instance.PopUp("더 이상 주울 수 없습니다!",Color.red);
                            }   
                        }
                        catch (Exception e)
                        { }
                    }
                    
                    //1부터 6으로 아이템 선택 가능
                    if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) ||
                        Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) ||
                        Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6)
                        || Input.GetKeyDown(KeyCode.Alpha7)|| Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        int contactIndex = 0;
                        if (Input.GetKeyDown(KeyCode.Alpha1)) //1
                            contactIndex = 0;
                        else if (Input.GetKeyDown(KeyCode.Alpha2)) //2
                            contactIndex = 1;
                        else if (Input.GetKeyDown(KeyCode.Alpha3)) //3
                            contactIndex= 2;
                        else if (Input.GetKeyDown(KeyCode.Alpha4)) //4
                            contactIndex = 3;
                        else if (Input.GetKeyDown(KeyCode.Alpha5)) //5
                            contactIndex= 4;
                        else if (Input.GetKeyDown(KeyCode.Alpha6)) //6
                            contactIndex = 5;
                        else if (Input.GetKeyDown(KeyCode.Alpha7)) //7
                            contactIndex = 6;
                        else if (Input.GetKeyDown(KeyCode.Alpha8)) //8
                            contactIndex = 7;
                        

                        if (contactIndex != selectedIndex)
                        {
                            selectedIndex = contactIndex;
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

                    if (Input.GetKeyDown(KeyCode.Q)) //F키를 길게 눌러 템 버리기
                    {
                        DiscardItem(false);
//                        if (time >= discardTime)
//                        {
//                            DiscardItem(false);
//                            time = 0;
//                        }
//                        else
//                        {
//                            time += Time.deltaTime;   
//                        }
                    }

//                    if (Input.GetKeyUp(KeyCode.F)) //떼면 시간 초기화
//                    {
//                        time = 0;
//                    }
                }
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

    public void GetItem(tem item)
    {
          bool isGet = false;
          int select = 0;
                                if (item.type == itemType.Usable) //소비템 또는 재료템이면
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
                                
                                if(!isGet) 
                                    PopUpManager.instance.PopUp("더 이상 제작할 수 없습니다!",Color.red);
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
                tem = item;
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

    public void DestroyItem(int index)
    {
        int inx = 0;
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].index == index)
            {
                inx = i;
                break;
            }
        }
        
            if (ItemList[inx].type == itemType.Usable) //소비템이면
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
    public void DiscardItem(bool isDead=false)
    {
        if (ItemList[selectedIndex].ItemSprite != null&&ItemList[selectedIndex].ItemSprite != NullSprite) //비어있지않다면
        {
            int ind = ItemList[selectedIndex].index;

            if (ItemList[selectedIndex].type == itemType.Usable) //소비템이면
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
    
    public void Dead()
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (ItemList[i].index != 0)
            {
                discardRPC(ItemList[i].index,true);
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
