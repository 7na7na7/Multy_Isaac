using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class DoorCol : MonoBehaviour
{
    private bool isInstantiate = false;
    
    public GameObject MinimapRoomPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&other.GetComponent<PhotonView>().IsMine)
        {
            Camera.main.transform.position=new Vector3(transform.parent.transform.position.x,transform.parent.transform.position.y,-10);
            Vector2 pos = transform.parent.transform.position;
            print(pos+" "+transform.parent.name.Substring(0, transform.parent.name.IndexOf("("))+"입니당!"); //(Clone) 앞까지 추출
            
            int x = (int)pos.x / 18;
            int y = (int) pos.y / 10;
            Vector3 minimapPos=new Vector3(500+x*0.9f,500+y*0.55f,-10);

            if (isInstantiate==false)
            {
                Instantiate(MinimapRoomPrefab,new Vector3(minimapPos.x,minimapPos.y,0), quaternion.identity);
                isInstantiate = true;
            }

            GameObject.FindGameObjectWithTag("Minimap").transform.position = minimapPos;
        }
    }
}
