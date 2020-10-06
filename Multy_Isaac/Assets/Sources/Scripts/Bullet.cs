using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed = 5;
    private int dir;
    public PhotonView pv;
    void Start()
    {
        Destroy(gameObject,1.0f);
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
