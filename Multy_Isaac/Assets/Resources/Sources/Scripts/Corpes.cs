using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Corpes : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject,60);
    }

    void OnEnable()
    {
        Color c;
        c.a = 0.75f;
        c.r = 0.5f;
        c.g = 0.5f;
        c.b = 0.5f;
        GetComponent<SpriteRenderer>().DOColor(c, 2f);
    }
    
}
