using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
  //public Sprite minimapRoom;
  private RoomTemplates templates;

  private void Start()
  {
    templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    if(templates.GetComponent<PhotonView>().IsMine) 
      templates.rooms.Add(this.gameObject);
  }
}
