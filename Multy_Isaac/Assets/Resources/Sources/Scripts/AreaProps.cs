using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AreaProps : MonoBehaviour
{
    public GameObject[] decos;
    public int minDeco;
    public int maxDeco;

    private void Awake()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            if(!PhotonNetwork.IsMasterClient)
                Destroy(gameObject);
        }
    }
    private void Start()
    {
        setProps();
    }

    void setProps()
    {
        if (decos.Length != 0)
        {
            int decoCount = Random.Range(minDeco, maxDeco);

            for (int i = 0; i < decoCount; i++)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    GameObject go=Instantiate(decos[Random.Range(0, decos.Length)], transform.position + new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4f, 2.5f), 0),quaternion.identity);
                    //go.transform.parent = transform;
                }
                else
                {
                    GameObject go=PhotonNetwork.InstantiateRoomObject(decos[Random.Range(0, decos.Length)].name, transform.position + new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4f, 2.5f), 0),quaternion.identity);
                    //go.transform.parent = transform;
                }
            }
        }
    }
}
