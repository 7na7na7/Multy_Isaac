using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural : MonoBehaviour
{
    public float delay = 0.02f;
    public float count = 0f;

    public float getCount()
    {
        count += delay;
        return count;
    }
}
