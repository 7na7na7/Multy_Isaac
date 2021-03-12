using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float DestroyTime = 2;
    public GameObject effect;
    private PhotonView pv;
    private SpriteRenderer spr;
    public Sprite none;
    public Player.bulletType type;
    public float nuckBackDistance;
    public string bulletName;
    public int Dmg;
    public float speed;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();
        Invoke("DestroyRPC",DestroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine && !other.GetComponent<Player>().isSuper)
            {
                other.GetComponent<Player>().Hit(Dmg, bulletName, nuckBackDistance, transform.position, type);
                spr.sprite = none;
                Destroy();
            }
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy();
        }
        else if(other.CompareTag("Slash"))
            Destroy();
    }
    
    public void Destroy()
    {
        if(PhotonNetwork.OfflineMode)
            DestroyRPC();
        else
            pv.RPC("DestroyRPC", RpcTarget.All);   
    }
    [PunRPC]
    void DestroyRPC()
    {
        GameObject go=Instantiate(effect, transform.position, Quaternion.identity);
        if(type==Player.bulletType.snow) 
            go.transform.localScale = transform.localScale/2;
        Destroy(gameObject);
    }
}
