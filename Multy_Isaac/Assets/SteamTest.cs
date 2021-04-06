using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamTest : MonoBehaviour
{ 
    void Start()
    {
        if (!SteamManager.Initialized) { return; }
//만약에 스팀이 열려있으면 스팀 사용자 이름 받아와서 출력!
        string name = SteamFriends.GetPersonaName();
        print(name);
    }
}
