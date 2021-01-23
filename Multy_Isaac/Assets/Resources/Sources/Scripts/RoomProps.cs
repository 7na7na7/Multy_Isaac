using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomProps : MonoBehaviour
{
    public GameObject minimapObj;
    public Vector2 offset;
    public Vector2 BoxSize;

    private void Start()
    {
        if(PhotonNetwork.OfflineMode) 
            Instantiate(minimapObj, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
        else
        {
            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.InstantiateRoomObject(minimapObj.name, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
        }
    }
}
