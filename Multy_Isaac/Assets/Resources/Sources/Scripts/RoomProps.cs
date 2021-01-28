using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomProps : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 BoxSize;

   public void setMinimap(GameObject minimapObj,float r,float g,float b)
    {
        if (PhotonNetwork.OfflineMode)
        {
            GameObject go=Instantiate(minimapObj, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
            
            Color color = go.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.r = r;
            color.g = g;
            color.b = b;
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject go=PhotonNetwork.InstantiateRoomObject(minimapObj.name, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);

                Color color = go.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                color.r = r;
                color.g = g;
                color.b = b;
                go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
