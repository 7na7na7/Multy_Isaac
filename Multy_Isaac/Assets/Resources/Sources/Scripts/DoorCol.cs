using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class DoorCol : MonoBehaviour
{
    private bool isInstantiate = false;
    private bool isInstantiate_2 = false;
    
    public GameObject MinimapRoomPrefab;
    public GameObject MinimapRoomPrefab_2;  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            DOTween.KillAll();
            Camera.main.transform.DOMove(
                new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, -10), 0.3f);
            Vector2 pos = transform.parent.transform.position;
            print(pos + " " + transform.parent.name.Substring(0, transform.parent.name.IndexOf("(")) +
                  "입니당!"); //(Clone) 앞까지 추출

            int x = (int) pos.x / 18;
            int y = (int) pos.y / 10;
            Vector3 minimapPos = new Vector3(500 + x * 0.9f, 500 + y * 0.55f, -10);

            if (isInstantiate == false)
            {
                Instantiate(MinimapRoomPrefab, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
                isInstantiate = true;
            }

            if (isInstantiate_2 == false)
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                   if(transform.parent.GetChild(i).CompareTag("WallSpawner"))
                   {
                       int c = transform.parent.GetChild(i).GetComponent<WallSpawner>().dir; 
                       switch (c) 
                       { 
                           case 3: 
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x+0.9f, minimapPos.y, 0), quaternion.identity); 
                               break; 
                           case 4: 
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x-0.9f, minimapPos.y, 0), quaternion.identity); 
                               break;
                           case 1: 
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y+0.55f, 0), quaternion.identity); 
                               break;
                           case 2 :
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y-0.55f, 0), quaternion.identity); 
                               break; 
                       }
                   }
                }
            }

        GameObject.FindGameObjectWithTag("Minimap").transform.DOMove(minimapPos,0.1f);
            GameObject.FindGameObjectWithTag("MinimapHead").transform.DOMove(new Vector3(minimapPos.x,minimapPos.y,0), 0.1f);
        }
    }

    public void StartEntry()
    {
      Camera.main.transform.DOMove(
                new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, -10), 0.3f);
            Vector2 pos = transform.parent.transform.position;
            print(pos + " " + transform.parent.name.Substring(0, transform.parent.name.IndexOf("(")) +
                  "입니당!"); //(Clone) 앞까지 추출

            int x = (int) pos.x / 18;
            int y = (int) pos.y / 10;
            Vector3 minimapPos = new Vector3(500 + x * 0.9f, 500 + y * 0.55f, -10);

            if (isInstantiate == false)
            {
                Instantiate(MinimapRoomPrefab, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
                isInstantiate = true;
            }

            if (isInstantiate_2 == false)
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                   if(transform.parent.GetChild(i).CompareTag("WallSpawner"))
                   {
                       int c = transform.parent.GetChild(i).GetComponent<WallSpawner>().dir; 
                       switch (c) 
                       { 
                           case 3: 
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x+0.9f, minimapPos.y, 0), quaternion.identity); 
                               break; 
                           case 4: 
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x-0.9f, minimapPos.y, 0), quaternion.identity); 
                               break;
                           case 1: 
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y+0.55f, 0), quaternion.identity); 
                               break;
                           case 2 :
                               Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y-0.55f, 0), quaternion.identity); 
                               break; 
                       }
                   }
                }
            }

        GameObject.FindGameObjectWithTag("Minimap").transform.DOMove(minimapPos,0.1f);
            GameObject.FindGameObjectWithTag("MinimapHead").transform.DOMove(new Vector3(minimapPos.x,minimapPos.y,0), 0.1f);
    }
}
