using System;
using System.Collections;
using System.Collections.Generic;
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
        
       
            if (decos.Length != 0)
            {
                int decoCount = Random.Range(minDeco, maxDeco);

                for (int i = 0; i < decoCount; i++)
                {
                    GameObject go=Instantiate(decos[Random.Range(0, decos.Length)], transform);
                    go.transform.position = transform.position + new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4f, 2.5f), 0); //벽이랑 안겹치게 1씩 떨어뜨려줌
                }
            }
    }
}
