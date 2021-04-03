using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public bool isSound = false;
    public string myName;
    public float nuckBackDistance;
    public int damage;
    public float delay;
    public int playerDmg;
    // Start is called before the first frame update
    private void Awake()
    {
        playerDmg = damage;
    }

    void Start()
    {
        if(isSound) 
            GetComponent<SoundManager>().Play(0,true,0.5f);
        Destroy(gameObject,delay);
    }
    
}
