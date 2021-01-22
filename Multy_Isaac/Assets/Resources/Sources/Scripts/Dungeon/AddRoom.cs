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

  private void Start()
  {
    Invoke("SetRoom",5f);
  }

  public void SetRoom()
  {
    templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    templates.rooms.Add(this.gameObject);
    SetRoomProps(); //생성
  }

  void SetRoomProps()
  {
   
      int randomAreaIndex = Random.Range(0, templates.RoomProps.Length);
      if (PhotonNetwork.OfflineMode)
      {
        Instantiate(templates.RoomProps[randomAreaIndex], transform.position, quaternion.identity);
      }
      else
      {
        PhotonNetwork.InstantiateRoomObject(templates.RoomProps[randomAreaIndex].name, transform.position, quaternion.identity);
      }
    
  }
  void Spawn(GameObject go, Transform tr,Vector3 pos)
  {
    GameObject GO=Instantiate(go, tr);
    GO.transform.position = pos;
  }
  
  void Spawn_P(string go,Transform tr,Vector3 pos)
  {
    GameObject GO=PhotonNetwork.InstantiateRoomObject(go,pos,quaternion.identity);
    GO.transform.SetParent(tr);
  }
}
