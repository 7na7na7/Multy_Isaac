using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider sound;
    public Dropdown dropdown;
    public Animator Anim;
    private bool isOpen = false;
    public GameObject FullScreenToggle;
    private playerCountSave p;

    public void SetSound()
    {
        p.SetSound(sound.value);
    }
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

    public void Kor()
    {
        p.SetLang(true);
    }

    public void Eng()
    {
        p.SetLang(false);
    }
    public void Close()
    {
        if (isOpen)
        {
            Anim.Play("Close");
            isOpen = false;
        } 
    }
    private void Start()
    {
        p = playerCountSave.instance;
        sound.value = p.soundValue;
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
