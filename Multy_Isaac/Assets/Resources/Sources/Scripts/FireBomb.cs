using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    public PhotonView pv;
    public GameObject fire;
    public Ease easeType;
    private Camera cam;
    public float rotateSpeed;
    public void ON()
    {
        if (PhotonNetwork.OfflineMode)
        {
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            moveRPC((Vector2)cam.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            pv.RPC("moveRPC",RpcTarget.All,(Vector2)cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    [PunRPC]
    void moveRPC(Vector2 pos)
    {
        //cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        transform.DOMove(pos, 0.75f).SetEase(easeType).OnComplete(()=>
        {
            if(PhotonNetwork.IsMasterClient) 
                PhotonNetwork.Instantiate(fire.name,transform.position,Quaternion.identity);
            Destroy(gameObject);
        });    
    }
    void Update()
    {
        transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z+rotateSpeed*Time.deltaTime);
    }
}
