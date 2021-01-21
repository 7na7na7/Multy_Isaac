using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public GameObject Effect;
    public Vector3 pos;
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
        if(PhotonNetwork.OfflineMode) 
            Instantiate(Effect, transform.position+pos,quaternion.identity);
        else
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.InstantiateRoomObject(Effect.name, transform.position+pos, quaternion.identity);
        }
    }
}
