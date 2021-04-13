using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGas : MonoBehaviour
{
    public GameObject gas;
    public int count;
    private BoxCollider2D area;
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider2D>();
        for (int i = 0; i < count; i++)
        {
            Instantiate(gas, new Vector3(Random.Range(area.bounds.min.x, area.bounds.max.x),
                                 Random.Range(area.bounds.min.y, area.bounds.max.y)), Quaternion.identity);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
