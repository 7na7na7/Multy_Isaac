using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
   private List<BoxCollider2D> DaySpawnAreas=new List<BoxCollider2D>();
    private List<BoxCollider2D> NightSpawnAreas=new List<BoxCollider2D>();
    public List<Transform> PlayerTrs;
    public int StartZombieCount;
    public Vector2 randomMin, randomMax;
    public float delay;
    public GameObject regularZombie;
    private BoxCollider2D area;
    private TimeManager time;
    void Start()
    {
        time = FindObjectOfType<TimeManager>();
        for (int i = 0; i < 4; i++)
        {
            DaySpawnAreas.Add(transform.GetChild(i).GetComponent<BoxCollider2D>());
        }
        for (int i = 4; i < 8; i++)
        {
            NightSpawnAreas.Add(transform.GetChild(i).GetComponent<BoxCollider2D>());
        }
        
        StartCoroutine(Spawn());
        Invoke("StartSpawn", 12f);
        
    }

    void StartSpawn()
    {
        Player[] players = FindObjectsOfType<Player>();
            foreach (Player p in players)
            {
                PlayerTrs.Add(p.GetComponent<Transform>());
            }
        
            for (int i = 0; i < StartZombieCount; i++)
            {
                AllMapSpawn();
            }
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(12f);
        while (true)
        {
            yield return new WaitForSeconds(delay);
            foreach (Transform tr in PlayerTrs)
            {
                if(time.isNight)
                    if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient) { }
                    else
                    {
                        NightSpawn(tr);
                    }
                else
                if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient) { }
                else
                {
                    DaySpawn(tr);
                }
            }
        }
    }

    void DaySpawn(Transform PlayerTr)
    {
        area = DaySpawnAreas[Random.Range(0, DaySpawnAreas.Count)];
        if (PhotonNetwork.OfflineMode)
        {
            Instantiate(regularZombie, PlayerTr.position+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(regularZombie.name,  PlayerTr.position+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), Quaternion.identity);   
            }
        }      
    }

    void NightSpawn(Transform PlayerTr)
    {
        area = NightSpawnAreas[Random.Range(0, DaySpawnAreas.Count)];
        if (PhotonNetwork.OfflineMode)
        {
            Instantiate(regularZombie, PlayerTr.position+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), quaternion.identity);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(regularZombie.name,  PlayerTr.position+new Vector3(Random.Range(area.bounds.min.x,area.bounds.max.x),Random.Range(area.bounds.min.y,area.bounds.max.y)), Quaternion.identity);   
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
