using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public bool canPause = true;
    public GameObject pausePanel;
    public Animator anim;
    private bool isOpen = false;
    private void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOpen)
                {
                    isOpen = false;
                    anim.Play("Close");
                }
                else
                {
                    isOpen = true;
                    anim.Play("Open");
                }
                
            }
        }
    }
    
    
}
