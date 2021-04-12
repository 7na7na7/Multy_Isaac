using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    public bool isAllZombie = true;
    public int FirstDelay = 15;
    public BoxCollider2D[] DaySpawnAreas;
    public BoxCollider2D[] NightSpawnAreas;
    public int StartZombieCount;
    public Transform randomMin, randomMax;
    public float delay;
    public float[] delays;
    public GameObject[] zombies;
    public int[] Days;
    public int[] Indexes;
    public int[] counts;
    public List<GameObject> zombieList=new List<GameObject>();
    private BoxCollider2D area;
    private TimeManager time;

    public void DaybyDay(int day)
    {
        delay = delays[day - 1];
        for (int i = 0; i < Days.Length; i++)
        {
            if (Days[i] == day)
            {
                for (int j = 0; j < counts[i]; j++)
                {
                    zombieList.Add(zombies[Indexes[i]-1]);
                }
            }
        }
    }
    void Start()
    {
        time = FindObjectOfType<TimeManager>();
        if(PhotonNetwork.OfflineMode)
            StartCoroutine(Spawn());
        else
        {
            StartCoroutine(Spawn());   
        }
        Invoke("StartSpawn", FirstDelay);
    }

    void StartSpawn()
    {
        if (PhotonNetwork.OfflineMode)
        {
            for (int i = 0; i < StartZombieCount; i++)
            {
                AllMapSpawn();
            }   
        }
//        else
//        {
//            for (int i = 0; i < StartZombieCount/2; i++)
//            {
//                AllMapSpawn();
//            }   
//        }
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(FirstDelay);
        while (true)
        {
            Player[] players = FindObjectsOfType<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                if(time.isNight)
                    NightSpawn(players[i].transform.position);
                else
                    DaySpawn(players[i].transform.position);
            }
            
            float rdelay = delay;
            if (!time.isNight)
                rdelay *= 1.75f;
            if (PhotonNetwork.OfflineMode)
                yield return new WaitForSeconds(rdelay);
            else
                yield return new WaitForSeconds(rdelay*players.Length*1.25f);
            
        }
    }

    void DaySpawn(Vector3 PlayerPos)
    {
        area = DaySpawnAreas[Random.Range(0, DaySpawnAreas.Length)];
        if (PhotonNetwork.OfflineMode)
        {
            SpawnZombie(zombieList[Random.Range(0,zombieList.Count)],
                PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x), Random.Range(area.bounds.min.y,area.bounds.max.y))); 
        }
//        else
//        {
//            if (PhotonNetwork.IsMasterClient)
//            {
//                SpawnZombie_N(zombieList[Random.Range(0, zombieList.Count)],
//                    PlayerPos + new Vector3(Random.Range(area.bounds.min.x, area.bounds.max.x), Random.Range(area.bounds.min.y, area.bounds.max.y)));
//            }
//        }      
    }

    void NightSpawn(Vector3 PlayerPos)
    {
        area = NightSpawnAreas[Random.Range(0, DaySpawnAreas.Length)];
        if (PhotonNetwork.OfflineMode)
        {
          SpawnZombie(zombieList[Random.Range(0, zombieList.Count)],PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)));
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnZombie_N(zombieList[Random.Range(0, zombieList.Count)],PlayerPos+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)));            }
        }      
    }
    
    void AllMapSpawn()
    {
        if (PhotonNetwork.OfflineMode)
        {
            SpawnZombie(zombieList[Random.Range(0, zombieList.Count)], new Vector2(Random.Range(randomMin.position.x, randomMax.position.x), Random.Range(randomMin.position.y, randomMax.position.y)));
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnZombie_N(zombieList[Random.Range(0, zombieList.Count)], new Vector2(Random.Range(randomMin.position.x, randomMax.position.x), Random.Range(randomMin.position.y, randomMax.position.y)));            }
        }   
    }

    public void SpawnZombie(GameObject zombie, Vector2 pos)
    {
        if (isAllZombie)
            Instantiate(zombies[Random.Range(0, zombies.Length)], pos, quaternion.identity);
        else
            Instantiate(zombie, pos, quaternion.identity);
    }
    public void SpawnZombie_N(GameObject zombie, Vector2 pos)
    {
        if (isAllZombie)
            PhotonNetwork.InstantiateRoomObject(zombies[Random.Range(0, zombies.Length)].name, pos, quaternion.identity);
        else
            PhotonNetwork.InstantiateRoomObject(zombie.name, pos, quaternion.identity);
    }
}
