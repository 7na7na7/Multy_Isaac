using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Player player=null;
    public bool canPause = true;
    public GameObject pausePanel;
    public Animator anim;
    private bool isOpen = false;

    private void Start()
    {
        isOpen = false;
    }

    private void Update()
    {
        if (player != null)
        {
            if (!player.isDead)
            {
                if (canPause)
                {
                    if (Input.GetKeyUp(KeyCode.Escape))
                    {
                        if (isOpen)
                        {
                            isOpen = false;
                            anim.Play("Close");
                            if(PhotonNetwork.OfflineMode) 
                                Time.timeScale = 1;
                        }
                        else
                        {
                            isOpen = true;
                            anim.Play("Open");
                            if(PhotonNetwork.OfflineMode) 
                                Time.timeScale = 0;
                        }

                    }
                }
                else
                {
                    if (Input.GetKeyUp(KeyCode.Escape))
                    {
                        canPause = true;
                    }
                }      
            }
            else
            {
                if (isOpen)
                {
                    isOpen = false;
                    anim.Play("Close");
                    if(PhotonNetwork.OfflineMode) 
                        Time.timeScale = 1;
                }

                if(canPause) 
                    canPause = false;
            }
        }
    }
    
    
}
