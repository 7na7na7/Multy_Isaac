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
                Instantiate(decos[Random.Range(0, decos.Length)],
                    transform.position+new Vector3(Random.Range(-9f, 9f), Random.Range(-5f,4.8f), 0), quaternion.identity); //벽이랑 안겹치게 0.2f떨어뜨려줌
            }
        }
    }
}
