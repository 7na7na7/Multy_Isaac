using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class ShopTem : MonoBehaviour
{
    List<Item> tems=new List<Item>();
    public GameObject[] shopTems;
    public Transform[] poses;
    void Start()
    {
        for (int i = 0; i < poses.Length; i++)
        {
            if (PhotonNetwork.OfflineMode)
            {
               GameObject g=Instantiate(shopTems[i], poses[i].transform.position, quaternion.identity);
               tems.Add(g.GetComponent<Item>());
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject g=PhotonNetwork.InstantiateRoomObject(shopTems[i].name, poses[i].transform.position, quaternion.identity);
                    tems.Add(g.GetComponent<Item>());
                }
            }   
        }
    }

    public void Change()
    {
        foreach (Item t in tems)
        {
            t.Del();
        }
    }
}
