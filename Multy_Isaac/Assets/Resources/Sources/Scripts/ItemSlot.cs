using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    public int itemCount = 0;
    public Text itemCountTxt;
    PlayerItem player;
    public int index;
    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.pv.IsMine)
            {
                player = p.GetComponent<PlayerItem>();
                break;
            }
        }  
        Invoke("setStartPos",5f);
    }

    void setStartPos()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (itemCount > 0)
            itemCountTxt.text = itemCount.ToString();
        else
            itemCountTxt.text = "";
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        img.transform.position = Input.mousePosition; //드래그하는 동안 커서 따라가게하기
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPos; //놓으면 템 제자리로 가기
        int otherIndex = 0; //바꿀템 인덱스
        float distance = 1000; 
        Vector3 mousePos = Input.mousePosition;
        for (int i = 0; i < player.slots.Length; i++)
        {
            float dis = Vector3.Distance(mousePos, player.slots[i].transform.position);
            if (dis <= distance) //제일 가까운칸 찾아서 거기인덱스 가져오기
            {
                distance = dis;
                otherIndex = player.slots[i].index;
            }
        }
        player.Swap(index,otherIndex); //자기인덱스랑 바꿀거 인덱스를 인자로 바꾸는 함수호출
    }
}
