using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamAchieveemnt : MonoBehaviour
{
    public static SteamAchieveemnt instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAchievement(string name)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement(name);
            SteamUserStats.StoreStats();
        }
    }
}
