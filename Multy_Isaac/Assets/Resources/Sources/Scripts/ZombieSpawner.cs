using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    public int FirstDelay = 15;
    public BoxCollider2D[] DaySpawnAreas;
    public BoxCollider2D[] NightSpawnAreas;
    public int StartZombieCount;
    public Vector2 randomMin, randomMax;
    public float delay;
    public GameObject regularZombie;
    private BoxCollider2D area;
    private TimeManager time;
    void Start()
    {
        time = FindObjectOfType<TimeManager>();
        if(PhotonNetwork.OfflineMode)
            StartCoroutine(Spawn());
        else
        {
            if(PhotonNetwork.IsMasterClient) 
                StartCoroutine(Spawn());   
        }
        Invoke("StartSpawn", FirstDelay);
    }

    void StartSpawn()
    {
        for (int i = 0; i < StartZombieCount; i++)
            {
                AllMapSpawn();
            }
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(FirstDelay);
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Player[] players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                if(time.isNight)
                    NightSpawn(players[i].transform.position);
                else
                    DaySpawn(players[i].transform.position);
            }
        }
    }

    void DaySpawn(Vector3 PlayerPos)
    {
        area = DaySpawnAreas[Random.Range(0, DaySpawnAreas.Length)];
        if (PhotonNetwork.OfflineMode)
        {
            Instantiate(regularZombie, PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(regularZombie.name,  PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), Quaternion.identity);   
            }
        }      
    }

    void NightSpawn(Vector3 PlayerPos)
    {
        area = NightSpawnAreas[Random.Range(0, DaySpawnAreas.Length)];
        if (PhotonNetwork.OfflineMode)
        {
            Instantiate(regularZombie, PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(regularZombie.name,  PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), Quaternion.identity);   
            }
        }      
    }
    
    void AllMapSpawn()
    {
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
