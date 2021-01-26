using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject regularZombie;
  
    void Start()
    {
        Invoke("Spawn",7f);
    }

    void Spawn()
    {
        if (PhotonNetwork.OfflineMode)
        {
            Instantiate(regularZombie, transform.position, quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.InstantiateRoomObject(regularZombie.name, transform.position, Quaternion.identity);
        }
    }
}
