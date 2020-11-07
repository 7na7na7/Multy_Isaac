using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Tab : MonoBehaviour
{
    public Animator anim;
    public Camera cam;
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
        cam.orthographicSize = 5;
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
        cam.orthographicSize = 2;
    }
}
