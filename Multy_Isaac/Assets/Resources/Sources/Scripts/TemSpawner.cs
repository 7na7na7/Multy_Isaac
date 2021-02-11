using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class TemSpawner : MonoBehaviour
{
    private TemManager temMgr;
    public BoxCollider2D[] bound;

    private List<int> temIndexes= new List<int>();

    private void Start()
    {
        temMgr = FindObjectOfType<TemManager>();
    }

    public void Set(int[] temIndex, int[] percentCounts, int minCount, int maxCount)
    {
        bool canGo = true;
        if (!PhotonNetwork.OfflineMode)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                canGo = false;
            }
        }
        if (canGo)
        {
            for (int j = 0; j < percentCounts.Length; j++)
            {
                for (int k = 0; k < percentCounts[j]; k++)
                    temIndexes.Add(temIndex[j]);
            }
        
            int random=Random.Range	(minCount,maxCount+1);
            for (int i = 0; i < random; i++)
            {
                int r = Random.Range(0, bound.Length);
                int index = temIndexes[Random.Range(0, temIndexes.Count)];
                temMgr.setTem(index,new Vector3(Random.Range(bound[r].bounds.min.x,bound[r].bounds.max.x),
                    Random.Range(bound[r].bounds.min.y,bound[r].bounds.max.y))); 
            }   
        }
    }
}
