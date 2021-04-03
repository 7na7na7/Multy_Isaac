using System;
using System.Collections;
using System.Collections.Generic;
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

public class InGameNetwork : MonoBehaviourPunCallbacks
{
   public int dayScore;
   public int zombieScore;
   public int killedZombies=0;
   public Text score;
   public Text killedZombie;
   public Text Day;
   public GameObject GameOverPanel;
   public GameObject GameOverPanel2;
   public Text LoseOrWin;
   public Text rank;
   public Player p;
   public GameObject[] playerPrefabs;
   public bool isFullScreen;
   public int ResX, RexY;
   public bool isOffline;
   
   public static InGameNetwork instance; //싱글톤
   private bool isDisconnecting = false;
   [Header("Chat")]
   public Text[] ChatText;
   public InputField ChatInput;
   public Scrollbar charBar;

   [Header("ETC")]
   public Text pingSpeed;
   public PhotonView PV;

   public void GameOver()
   {
      GameOverPanel.SetActive(true);
      int d = FindObjectOfType<TimeManager>().day;
      Day.text = d+" "+(d == 1 ? "Day":"Days");
      killedZombie.text = killedZombies.ToString();
      score.text = (dayScore * d + zombieScore).ToString();
   }

   public void GameOver2()
   {
      GameOverPanel2.SetActive(true);
      if (p.rank == 1)
      {
         LoseOrWin.text = "Win!";
         LoseOrWin.color=Color.yellow;
      }
      else
      {
         LoseOrWin.text = "Lose...";  
         LoseOrWin.color=Color.red;
      }
      rank.text = p.rank.ToString();
   }
   private void Awake()
   {
      StartCoroutine(delayDestroy());
      
      Screen.SetResolution(ResX, RexY, isFullScreen);
      instance = this;
      if (isOffline)
      {
         PhotonNetwork.OfflineMode = true;
         OnConnectedToMaster();
         Spawn();
      }
      else
      {
         Spawn();
      }
      
   }
   public void Suicide()
   {
      p.Die(PhotonNetwork.NickName);
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

      if (ChatInput.isFocused)
      {
         Player[] players = FindObjectsOfType<Player>();
         foreach (Player p in players)
         {
            if (p.pv.IsMine)
            {
               //p.canMove = false;
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
                  //p.canMove = true;
                  break;
               }
            }
         }
      }
      
   }
   
   
   #region 연결
   public void Disconnect() //연결 끊기
   {
    
         if (p.isDead)
         {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Main");
         }
         else
         {
            Suicide();
         }
      
   }

   public void Die()
   {
      
   }
   public override void OnDisconnected(DisconnectCause cause) //연결 끊어졌을 때
   {
      PopUpManager.instance.PopUp("연결 끊어짐", Color.red);
      SceneManager.LoadScene("Main");
   }

   #endregion


   #region 방

   public override void OnJoinedLobby()
   {
      SceneManager.LoadScene("Main");
   }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
       //RoomRenewal();
       ChatRPC("<color=green>" + newPlayer.NickName + "님이 게임에 참가했습니다.</color>");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
       //RoomRenewal();
       ChatRPC("<color=red>" + otherPlayer.NickName + "님이 게임에서 나갔습니다.</color>");
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
                p.pv.RPC("ChatBaloonRPC",RpcTarget.AllBuffered,ChatInput.text);
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
    IEnumerator delayDestroy()
    {
       yield return new WaitForSeconds(0.2f);
       foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Bullet")) GO.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.All);
    }
    IEnumerator delayScrollDown()
    {
       yield return new WaitForSeconds(0.01f);
       charBar.value = 0;
    }
    public void Spawn()
    {
       if (isOffline)
       {
          Instantiate(playerPrefabs[FindObjectOfType<playerCountSave>().PlayerIndex], Vector3.zero, quaternion.identity);  
       }
       else
       {
          PhotonNetwork.Instantiate("Player"+FindObjectOfType<playerCountSave>().PlayerIndex, Vector3.zero, quaternion.identity);  
       }
    }

}
