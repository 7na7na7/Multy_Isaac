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
    
    SetRoomProps(specialvalue);
  }

  void SetRoomProps(int specialvalue)
  {

    if (specialvalue != -1) //특별한 방이 아니면
    {
     
        for (int i = 0; i < transform.childCount; i++)
        {
          if (transform.GetChild(i).GetComponent<RoomSpawner>())
          {
            if (transform.GetChild(i).GetComponent<RoomSpawner>().spawned)
            {
              if (PhotonNetwork.OfflineMode)
                Spawn(templates.SpecialRooms[specialvalue], transform.GetChild(i).transform.position, quaternion.identity);
              else
                Spawn_P(templates.SpecialRooms[specialvalue].name, transform.GetChild(i).transform.position, quaternion.identity); 
            }
          }
        } 
   
    }
    else //특별한 방이면
    {
    
        for (int i = 0; i < transform.childCount; i++)
        {
          if (transform.GetChild(i).GetComponent<RoomSpawner>())
          {
            if (transform.GetChild(i).GetComponent<RoomSpawner>().spawned)
            {
              if (PhotonNetwork.OfflineMode)
                Spawn(templates.RoomProps[Random.Range(0, templates.RoomProps.Length)],
                  transform.GetChild(i).transform.position, quaternion.identity);
              else
                Spawn_P(templates.RoomProps[Random.Range(0, templates.RoomProps.Length)].name,
                  transform.GetChild(i).transform.position, quaternion.identity);
            }
          }
        }
    }
  }
  void Spawn(GameObject go, Vector3 pos, quaternion q)
  {
    Instantiate(go, pos, q);
  }
  
  void Spawn_P(string go,Vector3 pos,quaternion q)
  {
    PhotonNetwork.Instantiate(go, pos, q);
  }
}
