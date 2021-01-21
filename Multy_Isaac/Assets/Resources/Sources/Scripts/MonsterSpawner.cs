using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsters;

    public int minCount = 5;
    public int maxCount = 8;

    public int Count;
    
    private Vector2 first, second;
    public float minSpawnTime = 4;
    public float maxSpawnTime = 8;

    public bool isStartCor = false;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(first,second);
    }
    void Start()
    {
        //if (transform.parent.gameObject.tag != "Entry")
        if (PhotonNetwork.OfflineMode)
        {
            startSpawn();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startSpawn();
            }
        }
    }

    void startSpawn()
    {
        if (!transform.parent.CompareTag("Entry"))
        {
            Count = Random.Range(minCount, maxCount);
            for (int i = 0; i < Count; i++)
            {
                Count++;
                Spawn();
            }
        }
    }
    void Spawn()
    {
        Vector3 randomPos = Vector3.zero;
        bool canSpawn = false;
        //Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        while (true)
        {
            randomPos = transform.position + new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4f, 2.5f), 0);
            RaycastHit2D[] hit = Physics2D.BoxCastAll(randomPos,new Vector2(0.5f,0.5f), 0,Vector2.down,0);

            bool can = false;
            foreach (RaycastHit2D c in hit)
            {
                if (c.collider.CompareTag("Wall")) //벽과 닿으면 생성못함
                {
                    first = randomPos;
                    second = new Vector2(0.5f,0.5f);
                    can = true;
                    break;
                }
            }

            if (can == false)
                break;
        }

        int randomMon = Random.Range(0, monsters.Length);
        if (PhotonNetwork.OfflineMode)
        {
            GameObject mon=Instantiate(monsters[randomMon],transform);
            mon.transform.position = randomPos;
        }
        else
        {
           PhotonNetwork.InstantiateRoomObject(monsters[randomMon].name, randomPos, Quaternion.identity);
        }

        Count--;
    }
    IEnumerator spawnCor()
    {
        isStartCor = true;
        
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime,maxSpawnTime));
            if(Count>=1) //Count가 1이상이면
                Spawn();
        }
    }

    public void StartSpawnCor()
    {
        if(!isStartCor) 
            StartCoroutine(spawnCor());
    }
}
