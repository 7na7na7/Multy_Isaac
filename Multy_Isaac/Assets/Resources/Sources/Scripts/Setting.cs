using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Dropdown dropdown;
    public Animator Anim;
    private bool isOpen = false;
    public GameObject FullScreenToggle;
    private playerCountSave p;

    public void On()
    {
        if (isOpen)
        {
            Anim.Play("Close");
            isOpen = false;
        }
        else
        {
            Anim.Play("Open");
            isOpen = true;
        }
    }
    private void Start()
    {
        p = playerCountSave.instance;
        if (p.isFullScreen == 0)
            FullScreenToggle.SetActive(false);
        dropdown.value = p.resolutionIndex;
    }

    public void HandleInputData(int v)
    { 
        p.SetRes(v);
    }

    public void SetFullScreen()
    {
        if (FullScreenToggle.activeSelf)
        {
            FullScreenToggle.SetActive(false);
            p.SetFullScreen(false);
        }
        else
        {
            FullScreenToggle.SetActive(true);
            p.SetFullScreen(true);
        }
    }
}
