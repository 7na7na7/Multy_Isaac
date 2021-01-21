using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject StartEntryPrefab;
    private void Awake()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            if(!PhotonNetwork.IsMasterClient)
                Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
          Instantiate(StartEntryPrefab, transform.position, quaternion.identity);
        }
        else
        {
            PhotonNetwork.InstantiateRoomObject("StartEntry", transform.position, quaternion.identity);
        }
    }
    
}
