using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural : MonoBehaviour
{
    public float count = 0f;

    public float getCount()
    {
        count += 0.02f;
        return count;
    }
}
