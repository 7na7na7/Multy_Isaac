using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public string myName;
    public float nuckBackDistance;
    public int damage;
    public float delay;
    public int playerDmg;
    // Start is called before the first frame update
    void Start()
    {
        playerDmg = damage;
        Destroy(gameObject,delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
