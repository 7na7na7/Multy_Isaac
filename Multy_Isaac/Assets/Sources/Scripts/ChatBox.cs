using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    private float time = 0;
    public float AppearTime;

    public void SetTime()
    {
        time = AppearTime;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
            gameObject.SetActive(false);
    }
}
