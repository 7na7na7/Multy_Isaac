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

    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            setProps();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                setProps();
            }
        }
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
                    GameObject go=Instantiate(decos[Random.Range(0, decos.Length)], transform);
                    go.transform.position = transform.position + new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4f, 2.5f), 0); //벽이랑 안겹치게 1씩 떨어뜨려줌
                }
                else
                {
                    GameObject go=PhotonNetwork.InstantiateRoomObject(decos[Random.Range(0, decos.Length)].name, 
                        transform.position + new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4f, 2.5f), 0),quaternion.identity);
                }
            }
        }
    }
}
