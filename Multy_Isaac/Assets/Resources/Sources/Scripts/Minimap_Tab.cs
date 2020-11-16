using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Tab : MonoBehaviour
{
    public Animator anim;
    public Camera cam;
    public float bigSize;
    public float smallSize;
    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Tab))
          Open();
      if(Input.GetKeyUp(KeyCode.Tab))
          Close();
    }

    
    void Open()
    {
        anim.Play("Open");
        cam.orthographicSize = bigSize;
    }

    public void OpenSize()
    {
        
    }
    void Close()
    {
        anim.Play("Close");
    }

    public void CloseSize()
    {
        cam.orthographicSize = smallSize;
    }
}
