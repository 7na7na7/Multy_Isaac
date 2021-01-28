using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Random = UnityEngine.Random;

[System.Serializable]
public class prop
{
    public int perValue;
    public int index;
    public Sprite roofSprite;
    public int[] getItemIndex;
    public GameObject[] properties;
}
public class Roof : MonoBehaviour
{
    public prop[] props;
    private prop realProp;
    public float alpha = 0.01f;
    private SpriteRenderer spr;
    private List<int> indexList= new List<int>();
    
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        Set();
    }

    void Set()
    {
        for (int i = 0; i < props.Length; i++)
        {
            for (int j = 0; j < props[i].perValue;j++)
            {
                indexList.Add(props[i].index);   
            }
        }

        int index = indexList[Random.Range(0, indexList.Count)];

        for (int i = 0; i < props.Length; i++)
        {
            if (props[i].index == index)
            {
                realProp = props[i];
                break;
            }
        }

        spr.sprite = realProp.roofSprite;
    }
    void invisible()
    {
        Color color = spr.color;
        color.a = alpha;
        spr.DOColor(color, 1f);
    }

    void unInvisible()
    {
        Color color = spr.color;
        color.a = 1f;
        spr.DOColor(color, 1f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<PhotonView>().IsMine) 
                invisible();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<PhotonView>().IsMine) 
                unInvisible();
        }
    }
}
