using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class ShopTem : MonoBehaviour
{
    public GameObject[] shopTems;
    public Transform[] poses;
    void Start()
    {
        for (int i = 0; i < poses.Length; i++)
        {
            if (PhotonNetwork.OfflineMode)
            {
                Instantiate(shopTems[i], poses[i].transform.position, quaternion.identity);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateRoomObject(shopTems[i].name, poses[i].transform.position, quaternion.identity);
                }
            }   
        }
    }

}
