using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Photon.Pun;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
  public bool isBig = false;
  //public Sprite minimapRoom;
  private RoomTemplates templates;

  private void Start()
  {
    templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

    if (PhotonNetwork.OfflineMode)
    {
      templates.rooms.Add(this.gameObject);
    }
    else
    {
      if(templates.GetComponent<PhotonView>().IsMine) 
        templates.rooms.Add(this.gameObject); 
    }
  }
}
