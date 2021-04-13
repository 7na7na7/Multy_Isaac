using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gas : MonoBehaviour
{
    private SpriteRenderer spr;
    public float min, max;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        float r = Random.Range(min, max);
        transform.localScale=new Vector2(r,r);
        Color color=new Color(0,Random.Range(0.5f,1f),0);
        color.a = Random.Range(0.5f, 1f);
        spr.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
