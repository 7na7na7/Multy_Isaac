using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomProps : MonoBehaviour
{
    private PhotonView pv;
    public Vector2 offset;
    public Vector2 BoxSize;

    private float rr, gg, bb;

    private GameObject mp;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void setMinimap(GameObject minimapObj,float r,float g,float b)
    {
        rr = r;
        gg = g;
        bb = b;
        mp = minimapObj;
        
        if (PhotonNetwork.OfflineMode)
        {
            GameObject go=Instantiate(mp, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
            
            Color color = go.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.r = rr;
            color.g = gg;
            color.b = bb;
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            try
            {
                pv.RPC("set",RpcTarget.AllBuffered);
            }
            catch (Exception e)
            { throw; }
        }
    }


    [PunRPC]
    private void set()
    {
        GameObject go = Instantiate(mp, GameObject.Find("minimapTr").transform.position + new Vector3(transform.position.x * 0.1f, transform.position.y * 0.1f), Quaternion.identity);

        Color color = go.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        color.r = rr;
        color.g = gg;
        color.b = bb;
        go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
    }
}
