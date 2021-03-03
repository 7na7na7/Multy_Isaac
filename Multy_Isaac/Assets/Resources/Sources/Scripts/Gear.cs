using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public float rotateSpeed = 5f;
    void Update()
    {
        transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z+Time.deltaTime*rotateSpeed);
    }
}
