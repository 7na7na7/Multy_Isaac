using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public int range1, range2, range3, range4;
    public GameObject StartEntryPrefab;
    private void Start()
    {
        int rx = Random.Range(range1, range2);
        int ry = Random.Range(range3, range4);
        transform.position = new Vector3(rx * 18, ry * 10);
            
        if (PhotonNetwork.OfflineMode)
        {
          Instantiate(StartEntryPrefab, transform.position, quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject("StartEntry", transform.position, quaternion.identity);
            }   
        }
    }
    
}
