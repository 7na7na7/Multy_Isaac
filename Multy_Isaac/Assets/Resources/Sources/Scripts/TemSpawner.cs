using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class TemSpawner : MonoBehaviour
{
    public bool StartSpawn = false;
    
    public BoxCollider2D bound;
    public GameObject[] tems;
    public int[] percentCounts;
    public int minCount;
    public int maxCount;

    private List<GameObject> temArray= new List<GameObject>();

    private void Start()
    {
        if(StartSpawn)
            Set(tems,percentCounts,minCount,maxCount);
    }

    public void Set(GameObject[] tems, int[] percentCounts, int minCount, int maxCount)
    {
        for (int j = 0; j < percentCounts.Length; j++)
        {
            for (int k = 0; k < percentCounts[j]; k++)
                temArray.Add(tems[j]);
        }
        
        int random=Random.Range	(minCount,maxCount+1);
        
        for (int i = 0; i < random; i++)
        {
            if (PhotonNetwork.OfflineMode)
            {
                Instantiate(temArray[Random.Range(0, temArray.Count)],new Vector3(Random.Range(bound.bounds.min.x,bound.bounds.max.x),Random.Range(bound.bounds.min.y,bound.bounds.max.y)),Quaternion.identity);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateRoomObject(temArray[Random.Range(0, temArray.Count)].name,new Vector3(Random.Range(bound.bounds.min.x,bound.bounds.max.x),Random.Range(bound.bounds.min.y,bound.bounds.max.y)),Quaternion.identity);
                }
            }
        }
    }
}
