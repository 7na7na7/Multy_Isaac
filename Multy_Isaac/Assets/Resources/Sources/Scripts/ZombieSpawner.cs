using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    public Vector2 randomMin, randomMax;
    public int Count;
    public GameObject regularZombie;
  
    void Start()
    {
        Invoke("Spawn",7f);
    }

    void Spawn()
    {
        if (PhotonNetwork.OfflineMode)
        {
            for(int i=0;i<Count;i++)
                Instantiate(regularZombie, new Vector2(Random.Range(randomMin.x,randomMax.x),Random.Range(randomMin.y,randomMax.y)), quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.InstantiateRoomObject(regularZombie.name, transform.position, Quaternion.identity);
        }
    }
}
