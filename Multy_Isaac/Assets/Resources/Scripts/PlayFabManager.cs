﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviourPunCallbacks
{
   private Text txt;
   
   public InputField EmailInput, PasswordInput, UsernameInput;
   public GameObject LoadingPanel, LoginPanel;
   public string NickName;


   [Header("LobbyPanel")] 
   public GameObject LobbyPanel;
    public InputField RoomInput;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Text LobbyInfoText;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;
    public Transform chatTr;
    public Scrollbar charBar;
    [Header("ETC")]
    public PhotonView PV;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    
   private void Awake()
   {
      Screen.SetResolution(960, 540, false);
   }
   
   private void Update()
   {
      if (LoginPanel.activeSelf)
      {
         //탭키 전환
         if (Input.GetKeyDown(KeyCode.Tab))
         {
            if(EmailInput.isFocused)
            {
               PasswordInput.ActivateInputField();
            }
            if(PasswordInput.isFocused)
            {
               UsernameInput.ActivateInputField();
            }
            if(UsernameInput.isFocused)
            {
               EmailInput.ActivateInputField();
            }
         }  
      }

      if (LobbyPanel.activeSelf)
      {
         LobbyInfoText.text = ("접속자 "+PhotonNetwork.CountOfPlayers+"명 / 로비 "+ PhotonNetwork.CountOfPlayersInRooms) + "명";
      }

      if (RoomPanel.activeSelf)
      {
         if (Input.GetKeyDown(KeyCode.Return))
         {
            if (!ChatInput.isFocused)
            {
               ChatInput.Select();
               Send();
            }
         }
      }
   }
   

   #region PlayFab로그인/회원가입

   public void LogIn()
   {
      var request = new LoginWithEmailAddressRequest {Email = EmailInput.text, Password = PasswordInput.text};
      PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
      if(UsernameInput!=null) 
         NickName = UsernameInput.text;
      
      LoginPanel.SetActive(false);
      LoadingPanel.SetActive(true);
   }

   public void Register()
   {
      var request = new RegisterPlayFabUserRequest
         {Email = EmailInput.text, Password = PasswordInput.text, Username = UsernameInput.text};
      PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
      
      LoginPanel.SetActive(false);
      LoadingPanel.SetActive(true);
   }

   #endregion

   #region 로그인/회원가입 성공/실패 처리

   private void OnLoginSuccess(LoginResult result)
   {
      print("로그인 성공! (팝업띄우기)");
      
      EmailInput.text = null;
      PasswordInput.text = null;
      UsernameInput.text = null;
      Connect();
   }

   private void OnLoginFailure(PlayFabError error)
   {
      print("로그인 실패... (팝업띄우기)");
      
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
   }

   private void OnRegisterSuccess(RegisterPlayFabUserResult result)
   {
      print("회원가입 성공! (팝업띄우기)");
      
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
      
      EmailInput.text = null;
      PasswordInput.text = null;
      UsernameInput.text = null;
   }

   private void OnRegisterFailure(PlayFabError error)
   {
      print("회원가입 실패... (팝업띄우기)");
      
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
   }

   #endregion
   
   #region 연결

   public void Connect() //연결
   {
      PhotonNetwork.ConnectUsingSettings();  
   }

   public override void OnConnectedToMaster() //연결되었을때 
   {
      print("연결 성공!");
      PhotonNetwork.JoinLobby();
   }

   public override void OnJoinedLobby() //로비 들어왔을때
   {
      print("로비 들어옴!");
      LoadingPanel.SetActive(false);
      LobbyPanel.SetActive(true);
      RoomPanel.SetActive(false);
      PhotonNetwork.LocalPlayer.NickName = NickName;
      myList.Clear();
   }

   public void Disconnect() //연결 끊기
   {
      PhotonNetwork.Disconnect();  
   }

   public override void OnDisconnected(DisconnectCause cause) //연결 끊어졌을 때
   {
      print("연결 끊어짐...");
      LobbyPanel.SetActive(false);
      RoomPanel.SetActive(false);
   }
   #endregion

   #region 방리스트 갱신
   // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
   public void MyListClick(int num)
   {
      if (num == -2) --currentPage;
      else if (num == -1) ++currentPage;
      else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
      MyListRenewal();
   }

   void MyListRenewal()
   {
      // 최대페이지
      maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

      // 이전, 다음버튼
      PreviousBtn.interactable = (currentPage <= 1) ? false : true;
      NextBtn.interactable = (currentPage >= maxPage) ? false : true;

      // 페이지에 맞는 리스트 대입
      multiple = (currentPage - 1) * CellBtn.Length;
      for (int i = 0; i < CellBtn.Length; i++)
      {
         CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
         CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
         CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
      }
   }

   public override void OnRoomListUpdate(List<RoomInfo> roomList)
   {
      int roomCount = roomList.Count;
      for (int i = 0; i < roomCount; i++)
      {
         if (!roomList[i].RemovedFromList)
         {
            if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
            else myList[myList.IndexOf(roomList[i])] = roomList[i];
         }
         else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
      }
      MyListRenewal();
   }
   #endregion
   
   
    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + UnityEngine.Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 8 }); //방만들기

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom(); //랜덤룸 입장

    public void LeaveRoom() => PhotonNetwork.LeaveRoom(); //방떠나기

    public override void OnJoinedRoom()
    {
       LobbyPanel.SetActive(false);
       RoomPanel.SetActive(true);
       RoomRenewal();
       ChatInput.text = "";
       for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); } 

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       RoomRenewal();
       PV.RPC("ChatRPC", RpcTarget.All, "<color=green>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       RoomRenewal();
       PV.RPC("ChatRPC", RpcTarget.All, "<color=red>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }


    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " - " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    #endregion

    #region 채팅
    public void Send()
    {
       if (ChatInput.text == "" || ChatInput.text == null)
       {
          ChatInput.Select();
       }
       else
       {
          string msg = PhotonNetwork.NickName + " : " + ChatInput.text;
          PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
          ChatInput.text = "";
          ChatInput.ActivateInputField();  
       }
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
       bool isInput = false;
       for (int i = 0; i < ChatText.Length; i++)
          if (ChatText[i].text == "")
          {
             isInput = true;
             ChatText[i].text = msg;
             break;
          }
       if (!isInput) // 꽉차면 한칸씩 위로 올림
       {
          for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
          ChatText[ChatText.Length - 1].text = msg;
       }
    }
    #endregion
}
