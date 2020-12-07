using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class DoorCol : MonoBehaviour
{
    private TweenParams parms = new TweenParams();
    public GameObject r, l, t, b;
    private bool isInstantiate = false;

    public GameObject MinimapRoomPrefab;
    public GameObject MinimapRoomPrefab_2;

    private CameraManager camera;
    private Camera cam;
    private void Start()
    {
        cam=Camera.main;
        camera=cam.GetComponent<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        { 
            Minimap();
        }
    }
    bool isright=false, isleft=false, istop=false, isbottom=false;
    
    
    void top(int i, Vector3 minimapPos)
    {
        if (transform.parent.GetChild(i).transform.position.y - transform.position.y >=10)
        {
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y+1.1f, 0), quaternion.identity);
            Instantiate(b, new Vector3(minimapPos.x, minimapPos.y+1.1f, 0), quaternion.identity);
        }
        else
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y+0.55f, 0), quaternion.identity);
        istop = true;
    }

    void bottom(int i, Vector3 minimapPos)
    {
        if (transform.parent.GetChild(i).transform.position.y - transform.position.y <=-10)
        {
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y-1.1f, 0), quaternion.identity);
            Instantiate(t, new Vector3(minimapPos.x, minimapPos.y-1.1f, 0), quaternion.identity);
        }
        else
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x, minimapPos.y-0.55f, 0), quaternion.identity);
        isbottom = true;
    }

    void right(int i, Vector3 minimapPos)
    {
        if (transform.parent.GetChild(i).transform.position.x - transform.position.x >=18)
        {
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x+1.8f, minimapPos.y, 0), quaternion.identity);
            Instantiate(l, new Vector3(minimapPos.x+1.8f, minimapPos.y, 0), quaternion.identity);
        }
        else
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x+0.9f, minimapPos.y, 0), quaternion.identity);
        isright = true;
    }

    void left(int i, Vector3 minimapPos)
    {
        if (transform.parent.GetChild(i).transform.position.x - transform.position.x <=-18)
        {
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x-1.8f, minimapPos.y, 0), quaternion.identity);
            Instantiate(r, new Vector3(minimapPos.x-1.8f, minimapPos.y, 0), quaternion.identity);
        }
        else
            Instantiate(MinimapRoomPrefab_2, new Vector3(minimapPos.x-0.9f, minimapPos.y, 0), quaternion.identity);
        isleft = true;
    }
    public void Minimap()
    {
        camera.canMove = false;
        
        DOTween.Kill(parms);
        cam.transform.DOMove(
                new Vector3(transform.position.x, transform.position.y, -10), 0.3f).SetAs(parms).OnComplete(()=>
                {
                    if (transform.parent.GetChild(0).name == "Bound")
                    {
                        camera.SetBound(transform.parent.GetChild(0).GetComponent<BoxCollider2D>());   
                        camera.canMove = true;
                    }
                    else
                    {
                        camera.canMove = false;
                    }
                });;
            
            Vector2 pos = transform.position;
           // print(pos + " " + transform.parent.name.Substring(0, transform.parent.name.IndexOf("(")) + "입니당!"); //(Clone) 앞까지 추출

            int x = (int) pos.x / 18;
            int y = (int) pos.y / 10;
            Vector3 minimapPos = new Vector3(500 + x * 0.9f, 500 + y * 0.55f, -10);
            
            
            if (isInstantiate == false)
            {
                isInstantiate = true;

                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    if (transform.parent.GetChild(i).CompareTag("WallSpawner"))
                    {
                        int c = transform.parent.GetChild(i).GetComponent<WallSpawner>().dir;


                        if (c == 1) //위
                        {
                            print(transform.parent.GetChild(i).transform.position.y +" "+ transform.position.y);
                           top(i,minimapPos);
                        }
                        else if (c == 2) //아래
                        {
                            print(transform.parent.GetChild(i).transform.position.y +" "+ transform.position.y);
                           bottom(i,minimapPos);
                        }
                        else if (c == 3) //오른쪽
                        {
                            print(transform.parent.GetChild(i).transform.position.x +" "+ transform.position.x);
                           right(i,minimapPos);
                        }
                        else //왼쪽
                        {
                            print(transform.parent.GetChild(i).transform.position.x +" "+ transform.position.x);
                          left(i,minimapPos);
                        }
                    }
                } 
                
                if (istop)
                    Instantiate(t, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
               if(isbottom)
                   Instantiate(b, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
               if(isright)
                   Instantiate(r, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
               if(isleft)
                   Instantiate(l, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
                Instantiate(MinimapRoomPrefab, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
            }

        GameObject.FindGameObjectWithTag("Minimap").transform.DOMove(minimapPos,0.1f);
            GameObject.FindGameObjectWithTag("MinimapHead").transform.DOMove(new Vector3(minimapPos.x,minimapPos.y,0), 0.1f);
    }
}
