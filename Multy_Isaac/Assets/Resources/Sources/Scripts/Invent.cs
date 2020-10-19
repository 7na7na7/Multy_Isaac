using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invent : MonoBehaviour
{
    public List<tem> completeTemList;

    public Sprite NoneSprite;
    public Sprite BoxSprite;
    public Image[] completeBoxes; //조합템 테두리
    public Image[] completes; //조합템
    public Text ItemName; //템이름 
    public Text ItemDescription; //템설명

    private void Update()
    {
        for (int i = 0; i < completes.Length; i++)
        {
            if (completeTemList[i] != null)
            {
                completes[i].sprite = completeBoxes[i].sprite;
                completeBoxes[i].sprite = BoxSprite;
            }
            else
            {
                completes[i].sprite = NoneSprite;
                completeBoxes[i].sprite = NoneSprite;
            }
        } 
    }
}
