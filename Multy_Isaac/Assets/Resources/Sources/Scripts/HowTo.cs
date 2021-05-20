using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowTo : MonoBehaviour
{
    private string howtoKey = "howtoKey";
    private bool isOpen = false;
    public Animator TUTO;
    public GameObject[] tutorials;
    public GameObject prevBtn;
    public GameObject nextBtn;
    public int Index = 0;

    private void Start()
    {
      Invoke("ss",0.5f);
    }

    void ss()
    {
        if (PlayerPrefs.GetInt(howtoKey, 0) == 0)
        {
            On();
            PlayerPrefs.SetInt(howtoKey,1);
        }
    }
    public void On()
    {
        PhotonNetwork.OfflineMode = true;
        FindObjectOfType<PlayFabManager>().OnConnectedToMaster();
        FindObjectOfType<PlayFabManager>().CreateRoom();
        SceneManager.LoadScene("Tutorial");
        if (isOpen)
        {
            TUTO.Play("Close");
            isOpen = false;
        }
        else
        {
            TUTO.Play("Open");
            isOpen = true;
        }
    }
    
    public void Prev()
    {
        Index--;
        if (Index == 0)
            prevBtn.SetActive(false);
        else
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }
        set();
    }
    
    public void Next()
    {
        Index++;
        if (Index == tutorials.Length-1)
            nextBtn.SetActive(false);
        else
        {
            prevBtn.SetActive(true);
            nextBtn.SetActive(true);
        }
        set();
    }

    void set()
    {
        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i==Index) 
                tutorials[i].SetActive(true);
            else
                tutorials[i].SetActive(false);
        }
      
    }
}
