using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    public Vector2 randomMin, randomMax;
    public float delay;
    public GameObject regularZombie;
  
    void Start()
    {
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (PhotonNetwork.OfflineMode)
            {
                Instantiate(regularZombie, new Vector2(Random.Range(randomMin.x,randomMax.x),Random.Range(randomMin.y,randomMax.y)), quaternion.identity);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateRoomObject(regularZombie.name,  new Vector2(Random.Range(randomMin.x,randomMax.x),Random.Range(randomMin.y,randomMax.y)), Quaternion.identity);   
                }
            }   
        }
    }
}
