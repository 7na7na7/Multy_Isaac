using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomProps : MonoBehaviour
{
    public bool startPlay = false;
    public GameObject minimapObj;
    private PhotonView pv;
    public Vector2 offset;
    public Vector2 BoxSize;

    private float rr, gg, bb;

    private GameObject mp;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if(startPlay)
            setMinimap(minimapObj,0,0,0);
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
            
            if(rr==0&&gg==0&&bb==0)
            {}
            else
            {
                Color color = go.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                color.r = rr;
                color.g = gg;
                color.b = bb;
                go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;   
            }
        }
        else
        {
            if(rr==0&&gg==0&&bb==0)
            {}
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
