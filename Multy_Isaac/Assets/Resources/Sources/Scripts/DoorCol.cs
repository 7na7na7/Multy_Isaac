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
        }
    }
}
