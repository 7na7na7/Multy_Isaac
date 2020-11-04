using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DoorCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&other.GetComponent<PhotonView>().IsMine)
        {
            Camera.main.transform.position=new Vector3(transform.parent.transform.position.x,transform.parent.transform.position.y,-10);
            Vector2 pos = transform.parent.transform.position;
            print(pos+" "+transform.parent.name.Substring(0, transform.parent.name.IndexOf("("))+"입니당!"); //(Clone) 앞까지 추출
        }
    }
}
