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

    int a = 0;
    for (int i = 0; i < transform.childCount; i++)
    {
      if (transform.GetChild(i).GetComponent<RoomSpawner>())
        a++;
    }
   
    if (specialvalue != -1) //특별한 방이 아니면
    {
      if (a >= 4) //클방이면
      {
        for (int i = 0; i < transform.childCount; i++)
        {
          if (transform.GetChild(i).GetComponent<RoomSpawner>())
          {
            if (PhotonNetwork.OfflineMode)
              Instantiate(templates.SpecialRooms[specialvalue], transform.GetChild(i).transform.position, quaternion.identity);
            else
              PhotonNetwork.InstantiateRoomObject(templates.SpecialRooms[specialvalue].name, transform.GetChild(i).transform.position, quaternion.identity);
          }
        } 
      }
      else //작은방이면
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
    }
    else //특별한 방이면
    {
      if (a >= 4) //큰방이면
      {
        for (int i = 0; i < transform.childCount; i++)
        {
          if (transform.GetChild(i).GetComponent<RoomSpawner>())
          {
            if (PhotonNetwork.OfflineMode)
              StartCoroutine(Spawn(templates.RoomProps[Random.Range(0, templates.RoomProps.Length)], transform.GetChild(i).transform.position, quaternion.identity));
            else
              StartCoroutine(Spawn_P(templates.RoomProps[Random.Range(0, templates.RoomProps.Length)].name, transform.GetChild(i).transform.position, quaternion.identity));
          }
        } 
      }
      else //작은방이면
      {
        if (PhotonNetwork.OfflineMode)
        {
          StartCoroutine(Spawn(templates.RoomProps[Random.Range(0, templates.RoomProps.Length)], transform.position, quaternion.identity));
        }
        else
        {
          StartCoroutine(Spawn_P(templates.RoomProps[Random.Range(0, templates.RoomProps.Length)].name, transform.position, quaternion.identity));
        }
      }
    }
  }
  IEnumerator Spawn(GameObject go, Vector3 pos, quaternion q)
  {
    yield return new WaitForSeconds(0.2f);
    Instantiate(go, pos, q);
  }
  
  IEnumerator Spawn_P(string go,Vector3 pos,quaternion q)
  {
    yield return new WaitForSeconds(0.2f);
    PhotonNetwork.Instantiate(go, pos, q);
  }
}
