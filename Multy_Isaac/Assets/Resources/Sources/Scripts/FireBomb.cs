using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    public GameObject fire;
    public Ease easeType;
    private Camera cam;
    public float rotateSpeed;
    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        transform.DOMove((Vector2)cam.ScreenToWorldPoint(Input.mousePosition), 0.75f).SetEase(easeType).OnComplete(()=>
            {
                if(PhotonNetwork.OfflineMode) 
                    Instantiate(fire,transform.position,Quaternion.identity);
                else 
                    PhotonNetwork.Instantiate(fire.name,transform.position,Quaternion.identity);
                Destroy(gameObject);
            });   
    }

    void Update()
    {
        transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z+rotateSpeed*Time.deltaTime);
    }
}
