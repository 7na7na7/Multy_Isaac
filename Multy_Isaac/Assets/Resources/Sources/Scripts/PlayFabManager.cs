using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using Unity.Mathematics;
using Hashtable=ExitGames.Client.Photon.Hashtable;

public class PlayFabManager : MonoBehaviourPunCallbacks
{
   public int SendRate;
   public int SerializationRate;
   public Vector2Int startResolution;
   public List<GameObject> Prefabs;

   public GameObject titleImg;
   private Text txt;
   //public string GameVersion = "0.1";
   public InputField EmailInput, PasswordInput, UsernameInput;
   public GameObject LoadingPanel, LoginPanel;
   public string NickName;
   private string EamilKey = "EmailKey";
   private string NameKey = "NameKey";

   [Header("LobbyPanel")]
   public Text pingSpeed;
   public GameObject LobbyPanel;
    public InputField RoomInput;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Text LobbyInfoText;

    [Header("RoomPanel")] 
    public GameObject StartBtn;
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;
    public Scrollbar charBar;
    
    [Header("ETC")]
    public PhotonView PV;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    
   private void Awake()
   {
      PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString(NameKey,"");
      Screen.SetResolution(startResolution.x,startResolution.y, false);
      EmailInput.text = PlayerPrefs.GetString(EamilKey, "");
      UsernameInput.text = PlayerPrefs.GetString(NameKey, "");

      PhotonNetwork.SendRate = SendRate;
      PhotonNetwork.SerializationRate = SerializationRate;
      //동기화 빠르게
   }

   public void offlineMode()
   {
      PhotonNetwork.OfflineMode = true;
      OnConnectedToMaster();
      CreateRoom();
      SceneManager.LoadScene("Play");
   }
   void Start ()
   {
      DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
      if (pool != null && this.Prefabs != null)
      {
         foreach (GameObject prefab in this.Prefabs)
         {
            pool.ResourceCache.Add(prefab.name, prefab);
         }
      }
   }
   
   private void Update()
   {
      if (PhotonNetwork.IsConnected)
      {
         if(PhotonNetwork.GetPing()<50)
            pingSpeed.color=Color.green;
         else if(PhotonNetwork.GetPing()<100)
            pingSpeed.color=Color.yellow;
         else
            pingSpeed.color=Color.red;
         pingSpeed.text = "Ping : "+PhotonNetwork.GetPing().ToString();
      }
      else
      {
         pingSpeed.text = "DisConnected";
      }
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
         
         //엔터키로 로그인
         if(Input.GetKeyDown(KeyCode.Return))
            LogIn();
      }

      if (LobbyPanel.activeSelf)
      {
         if(Input.GetKeyDown(KeyCode.Escape)) 
            Disconnect();
         
         LobbyInfoText.text = ("접속자 "+PhotonNetwork.CountOfPlayers+"명 / 로비 "+ PhotonNetwork.CountOfPlayersInRooms) + "명";
      }

      if (RoomPanel.activeSelf)
      {
         RoomRenewal();
         if (PhotonNetwork.IsMasterClient) //방주인이면
            StartBtn.SetActive(true);
         else
            StartBtn.SetActive(false);
         
         if (Input.GetKeyDown(KeyCode.Escape)) //방에있을때 esc누르면 방에서나감
            PhotonNetwork.LeaveRoom();
         
         if (ChatInput.isFocused)
         {
            Player[] players = FindObjectsOfType<Player>();
            foreach (Player p in players)
            {
               if (p.pv.IsMine)
               {
                  p.canMove = false;
                  break;
               }
            }  
         }

         if (Input.GetKeyDown(KeyCode.Return))
         {
            if (!ChatInput.gameObject.activeSelf)
            {
               ChatInput.gameObject.SetActive(true);
               ChatInput.ActivateInputField();
            }
            else
            {
               Send();  
               Player[] players = FindObjectsOfType<Player>();
               foreach (Player p in players)
               {
                  if (p.pv.IsMine)
                  {
                     p.canMove = true;
                     break;
                  }
               }
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
      
      PlayerPrefs.SetString(NameKey,NickName);

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
      PopUpManager.instance.PopUp("Login Succeed", Color.green);
      
      PlayerPrefs.SetString(EamilKey,EmailInput.text);
      PlayerPrefs.SetString(NameKey,UsernameInput.text);
      EmailInput.text = null;
      PasswordInput.text = null;
      UsernameInput.text = null;
      
      Connect();
   }

   private void OnLoginFailure(PlayFabError error)
   {
      PopUpManager.instance.PopUp(error.ToString(), Color.red);
      
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
   }

   private void OnRegisterSuccess(RegisterPlayFabUserResult result)
   {
      PopUpManager.instance.PopUp("Register Succeed", Color.green);
      
      PlayerPrefs.SetString(EamilKey,EmailInput.text);
      PlayerPrefs.SetString(NameKey,UsernameInput.text);
      
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
      
      EmailInput.text = null;
      PasswordInput.text = null;
      UsernameInput.text = null;
   }

   private void OnRegisterFailure(PlayFabError error)
   {
      PopUpManager.instance.PopUp(error.ToString(), Color.red);
      
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
   }

   #endregion
   
   #region 연결

   public void Connect() //연결
   {
      //PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "editor";
      //PhotonNetwork.GameVersion = GameVersion;
      PhotonNetwork.AutomaticallySyncScene = true; //씬동기화
      PhotonNetwork.ConnectUsingSettings();  
   }

   public override void OnConnectedToMaster() //연결되었을때 
   {
      PopUpManager.instance.PopUp("서버 연결됨", Color.green);
      PhotonNetwork.JoinLobby();
   }

   public override void OnJoinedLobby() //로비 들어왔을때
   {
      LoginPanel.SetActive(false);
      LoadingPanel.SetActive(false);
      LobbyPanel.SetActive(true);
      RoomPanel.SetActive(false);
      titleImg.SetActive(true);
      PhotonNetwork.LocalPlayer.NickName = NickName;
      myList.Clear();
   }
   public void Disconnect() //연결 끊기
   {
      PhotonNetwork.Disconnect();  
   }

   public override void OnDisconnected(DisconnectCause cause) //연결 끊어졌을 때
   {
      PopUpManager.instance.PopUp("연결 끊어짐", Color.red);
      LobbyPanel.SetActive(false);
      RoomPanel.SetActive(false);
      LoadingPanel.SetActive(false);
      LoginPanel.SetActive(true);
      EmailInput.text = PlayerPrefs.GetString(EamilKey, "");
      UsernameInput.text = PlayerPrefs.GetString(NameKey, "");
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
    public void CreateRoom()
    {
       RoomOptions roomOpton=new RoomOptions();
       roomOpton.MaxPlayers = 10;
       PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + UnityEngine.Random.Range(0, 100) : RoomInput.text, roomOpton); //방만들기     
    }

    public void JoinRandomRoom()
    {
       PhotonNetwork.JoinRandomRoom(); //랜덤룸 입장
    }

    public void LeaveRoom() => PhotonNetwork.LeaveRoom(); //방떠나기

    public override void OnJoinedRoom()
    {
       StartCoroutine(delayDestroy());
       Spawn();
       LobbyPanel.SetActive(false);
       RoomPanel.SetActive(true);
       titleImg.SetActive(false);
       //RoomRenewal();
       ChatInput.text = "";
       for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    IEnumerator delayDestroy()
    {
       yield return new WaitForSeconds(0.2f);
       foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Bullet")) GO.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.All);
    }
   
    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }
   
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
       //RoomRenewal();
       ChatRPC("<color=green>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
       //RoomRenewal();
       ChatRPC("<color=red>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + (PhotonNetwork.PlayerList[i].IsMasterClient==true ? "(방장)" :"")+" - "+PhotonNetwork.GetPing()+"ms"+"\n";
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " - " + PhotonNetwork.CurrentRoom.PlayerCount+ " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    #endregion

    #region 채팅
    public void Send()
    {
       if (ChatInput.text == "" || ChatInput.text == null)
       {
          ChatInput.Select();
          ChatInput.gameObject.SetActive(false);
       }
       else
       {
          string msg = PhotonNetwork.NickName + " : " + ChatInput.text;
          PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
          Player[] players = FindObjectsOfType<Player>();
          foreach (Player p in players)
          {
             if (p.pv.IsMine)
             {
                p.pv.RPC("ChatBaloonRPC",RpcTarget.All,ChatInput.text);
                break;
             }
          }  
          ChatInput.text = "";
          //ChatInput.ActivateInputField();
          StartCoroutine(delayScrollDown());
          ChatInput.gameObject.SetActive(false);
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

    IEnumerator delayScrollDown()
    {
       yield return new WaitForSeconds(0.01f);
       charBar.value = 0;
    }
    public void Spawn()
    {
       PhotonNetwork.Instantiate("Player"+FindObjectOfType<playerCountSave>().PlayerIndex, Vector3.zero, quaternion.identity);
    }

    public void GameStart()
    {
       if (PhotonNetwork.IsMasterClient) //방주인이면
       {
          PhotonNetwork.CurrentRoom.IsOpen = false; //더이상 플레이어 못들어오게함
          PhotonNetwork.CurrentRoom.IsVisible = false; //방목록에서 안보이게함

          Player[] players = FindObjectsOfType<Player>();

          PhotonNetwork.LoadLevel(1); //Build Settng에서 1번째 인덱스의 씬 호출  
       }
    }
   
}
