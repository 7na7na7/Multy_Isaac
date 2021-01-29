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
    public bool isRandomColor = false;
    public int perValue;
    public int index;
    public Sprite roofSprite;
    public GameObject minimapObj;
    public int[] getItemIndex;
    public GameObject[] properties;
}
public class Roof : MonoBehaviour
{
    private PhotonView pv;
    public prop[] props;
    private prop realProp;
    private SpriteRenderer spr;
    private List<int> indexList= new List<int>();
    
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        spr = GetComponent<SpriteRenderer>();

        if (PhotonNetwork.OfflineMode)
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
                    index = i;
                    break;
                }
            }
            
            Set(index,Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));   
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
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
                        index = i;
                        break;
                    }
                }

                int a = Random.Range(0, 6);
                float min = 0.3f;
                float max = 0.8f;
                float r=1, g=1, b=1;
                switch (a)
                {
                    case 0:
                        r = Random.Range(min,max);
                        break;
                    case 1:
                        g = Random.Range(min,max);
                        break;
                    case 2:
                        b = Random.Range(min,max);
                        break;
                    case 3:
                        r = Random.Range(min,max);
                        g =Random.Range(min,max);
                        break;
                    case 4:
                        r = Random.Range(min,max);
                        b =Random.Range(min,max);
                        break;
                    case 5:
                        g =Random.Range(min,max);
                        b = Random.Range(min,max);
                        break;
                    case 6:
                        r =Random.Range(min,max);
                        g =Random.Range(min,max);
                        b = Random.Range(min,max);
                        break;
                }
                pv.RPC("Set",RpcTarget.AllBuffered,index,r,g,b);
            }
        }
    }

    [PunRPC]
    void Set(int realIndex, float r, float g, float b)
    {
        realProp = props[realIndex];
        if(!realProp.isRandomColor) 
            transform.parent.GetComponent<RoomProps>().setMinimap(props[realIndex].minimapObj,1,1,1);
        else
            transform.parent.GetComponent<RoomProps>().setMinimap(props[realIndex].minimapObj,r,g,b);
        
        
        spr.sprite = realProp.roofSprite;

        if (realProp.isRandomColor)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.r = r;
            c.g = g;
            c.b = b;
            GetComponent<SpriteRenderer>().color = c;   
        }
    }
    void invisible()
    {
        Color color = spr.color;
        color.a = 0f;
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
