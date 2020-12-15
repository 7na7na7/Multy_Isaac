﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_Cam : MonoBehaviour
{
    public Camera cam;
    public float max, min;
    public float scrollSpeed;
    private void Update() 
    { 
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed*-1;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + scroll * scrollSpeed*Time.deltaTime, min, max);
    }
    
}