using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public bool isRPCSound = true;
    public string myName;
    public string myName2;
    public float nuckBackDistance;
    public int damage;
    public float delay;
    // Start is called before the first frame update
    private void Awake()
    {
    }

    void Start()
    {
        if(GetComponent<SoundManager>()!=null) 
            GetComponent<SoundManager>().Play(0,isRPCSound,0.5f);
        Destroy(gameObject,delay);
    }
    
}
