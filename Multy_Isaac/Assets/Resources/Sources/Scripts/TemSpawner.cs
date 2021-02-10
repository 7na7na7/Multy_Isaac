using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class TemSpawner : MonoBehaviour
{

    public BoxCollider2D[] bound;

    private List<GameObject> temArray= new List<GameObject>();

    public void Set(GameObject[] tems, int[] percentCounts, int minCount, int maxCount)
    {
        for (int j = 0; j < percentCounts.Length; j++)
        {
            for (int k = 0; k < percentCounts[j]; k++)
                temArray.Add(tems[j]);
        }
        
        int random=Random.Range	(minCount,maxCount+1);
//        if (random > 3)
//            random = 3;
        for (int i = 0; i < random; i++)
        {
            int r = Random.Range(0, bound.Length);
            
            if (PhotonNetwork.OfflineMode)
            {
                Instantiate(temArray[Random.Range(0, temArray.Count)],new Vector3(Random.Range(bound[r].bounds.min.x,bound[r].bounds.max.x),Random.Range(bound[r].bounds.min.y,bound[r].bounds.max.y)),Quaternion.identity);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(delaySpawn(r));
                }
            }
        }
    }

    IEnumerator delaySpawn(int r)
    {
        yield return new WaitForSeconds(10);
        PhotonNetwork.InstantiateRoomObject(temArray[Random.Range(0, temArray.Count)].name,new Vector3(Random.Range(bound[r].bounds.min.x,bound[r].bounds.max.x),Random.Range(bound[r].bounds.min.y,bound[r].bounds.max.y)),Quaternion.identity);
    }
}
