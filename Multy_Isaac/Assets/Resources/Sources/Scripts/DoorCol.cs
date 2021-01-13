using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class DoorCol : MonoBehaviour
{
    public Vector2 doorValue;
    private TweenParams parms = new TweenParams();
    public GameObject r, l, t, b;
    private bool isInstantiate = false;

    public GameObject MinimapRoomPrefab;
    public GameObject MinimapRoomPrefab_2;
    
    private Camera cam;
    private void Start()
    {
        cam=Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        { 
            Minimap();
        }
    }
    bool isright=false, isleft=false, istop=false, isbottom=false;
    
    
    void mini(int i, Vector3 minimapPos)
    {
        GameObject a = null;
        if (i == 1)
        {
            istop = true;
            a = b;
        }
        else if (i == 2)
        {
            isbottom = true;
            a = t;
        }
        else if (i == 3)
        {
            isright = true;
            a = l;
        }
        else
        {
            isleft = true;
            a = r;
        }
       
            Instantiate(MinimapRoomPrefab_2, minimapPos, quaternion.identity);
            Instantiate(a, minimapPos, quaternion.identity);

        istop = true;
    }
    public void Minimap()
    {
        cam.GetComponent<CameraManager>().canMove = false;
        
        DOTween.Kill(parms);
        cam.transform.DOMove(
                new Vector3(transform.position.x+doorValue.x, transform.position.y+doorValue.y, -10), 0.3f).SetAs(parms).OnComplete(()=>
                {
                    if (transform.parent.GetChild(0).name == "Bound")
                    {
                        transform.parent.GetChild(0).gameObject.SetActive(true);
                        cam.GetComponent<CameraManager>().SetBound(transform.parent.GetChild(0).GetComponent<BoxCollider2D>().bounds.min,transform.parent.GetChild(0).GetComponent<BoxCollider2D>().bounds.max);   
                        cam.GetComponent<CameraManager>().canMove = true;
                    }
                    else
                    {
                        cam.GetComponent<CameraManager>().canMove = false;
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

                        Vector2 wallSpawnerPos = transform.parent.GetChild(i).transform.localPosition;
                        
                        int xx = (int) wallSpawnerPos.x / 18;
                        int yy = (int) wallSpawnerPos.y / 10;
                        
                        Vector2 newPos=new Vector2(minimapPos.x+xx*0.9f,minimapPos.y+yy*0.55f);
                        
                        mini(c,newPos);
                    }
                } 
                
//                if (istop)
//                    Instantiate(t, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
//               if(isbottom)
//                   Instantiate(b, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
//               if(isright)
//                   Instantiate(r, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
//               if(isleft)
//                   Instantiate(l, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
               // int dx = (int) doorValue.x / 18;
                //int dy = (int) doorValue.y / 10;

                Instantiate(MinimapRoomPrefab, new Vector3(minimapPos.x, minimapPos.y, 0), quaternion.identity);
            }

        GameObject.FindGameObjectWithTag("Minimap").transform.DOMove(minimapPos,0.1f);
            GameObject.FindGameObjectWithTag("MinimapHead").transform.DOMove(new Vector3(minimapPos.x,minimapPos.y,0), 0.1f);
    }
}
