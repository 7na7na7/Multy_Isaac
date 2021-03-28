using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public int Count=4;
    public Vector4[] ranges;
    public GameObject StartEntryPrefab;
    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            for (int i = 0; i < Count; i++)
            {
                Instantiate(StartEntryPrefab, new Vector2(
                    Random.Range((int)ranges[i].x, (int)ranges[i].y+1) * 18, Random.Range((int)ranges[i].z, (int)ranges[i].w+1) * 10), quaternion.identity);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < Count; i++)
                {
                    PhotonNetwork.InstantiateRoomObject(StartEntryPrefab.name, new Vector2(
                        Random.Range((int)ranges[i].x, (int)ranges[i].y) * 18+1, Random.Range((int)ranges[i].z, (int)ranges[i].w+1) * 10), quaternion.identity);
                }
            }   
        }
    }
    
}
