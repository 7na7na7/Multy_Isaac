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
        if (PhotonNetwork.OfflineMode)
        {
          Instantiate(StartEntryPrefab, new Vector3(Random.Range(range1, range2)*18,Random.Range(range3, range4)*10,transform.position.z), quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < playerCountSave.instance.playerCount; i++)
                {
                    PhotonNetwork.InstantiateRoomObject("StartEntry",   new Vector3(Random.Range(range1, range2)*18,Random.Range(range3, range4)*10,transform.position.z), quaternion.identity);   
                }
            }   
        }
    }
    
}
