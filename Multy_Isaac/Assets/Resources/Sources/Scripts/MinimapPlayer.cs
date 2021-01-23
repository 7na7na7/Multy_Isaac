using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlayer : MonoBehaviour
{
    public GameObject target;
    private Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        transform.position =pos + new Vector3(target.transform.position.x * 0.1f, target.transform.position.y * 0.1f,0);
    }
}
