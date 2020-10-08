using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed = 5;
    public float DestroyTime = 1;
    private int dir;
    public PhotonView pv;
    void Start()
    {
        Destroy(gameObject,DestroyTime);
    }
    
    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }

   
    
    [PunRPC]
    public void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
