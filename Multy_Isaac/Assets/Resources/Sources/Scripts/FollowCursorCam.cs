using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursorCam : MonoBehaviour
{
    private void Update()
    {
        Vector2 pointer = Input.mousePosition.normalized;
        print(pointer);
    }
}
