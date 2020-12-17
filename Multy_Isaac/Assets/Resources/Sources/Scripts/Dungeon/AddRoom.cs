using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddRoom : MonoBehaviour
{
  public Vector2 offset;
  public Vector2 BoxSize;
  //public Sprite minimapRoom;
  private RoomTemplates templates;

  public void SetRoom(int specialvalue = -1)
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
    
    if (specialvalue != -1)
    {
      if (PhotonNetwork.OfflineMode)
      {
        Instantiate(templates.SpecialRooms[specialvalue], transform.position,quaternion.identity);
      }
      else
      {
        PhotonNetwork.InstantiateRoomObject(templates.SpecialRooms[specialvalue].name, transform.position,quaternion.identity);
      }
    }
    else
    {
      if (PhotonNetwork.OfflineMode)
      {
        Instantiate(templates.RoomProps[Random.Range(0,templates.RoomProps.Length)], transform.position,quaternion.identity);
      }
      else
      {
        PhotonNetwork.InstantiateRoomObject(templates.RoomProps[Random.Range(0,templates.RoomProps.Length)].name, transform.position,quaternion.identity);
      }
    }
  }
}
