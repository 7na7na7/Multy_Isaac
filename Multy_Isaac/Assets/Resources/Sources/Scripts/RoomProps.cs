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
    private GameObject minimapTr;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        minimapTr=GameObject.Find("minimapTr");
    }

    public void setMinimap(GameObject minimapObj,float r,float g,float b)
    {
        rr = r;
        gg = g;
        bb = b;
        mp = minimapObj;
        
        if (PhotonNetwork.OfflineMode)
        {
            GameObject go=Instantiate(mp, minimapTr.transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
            
            if(rr==1&&gg==1&&bb==1)
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
            if (rr == 1 && gg == 1 && bb == 1)
            {
                try
                {
                    pv.RPC("set",RpcTarget.AllBuffered,false);
                }
                catch (Exception e)
                { throw; }
            }
            else
            {
                try
                {
                    pv.RPC("set",RpcTarget.AllBuffered,true);
                }
                catch (Exception e)
                { throw; }
            }
        }
    }


    [PunRPC]
    private void set(bool isRandomColor)
    {
        GameObject go = Instantiate(mp, minimapTr.transform.position + new Vector3(transform.position.x * 0.1f, transform.position.y * 0.1f), Quaternion.identity);

        if (isRandomColor)
        {
            Color color = go.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.r = rr;
            color.g = gg;
            color.b = bb;
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;   
        }
    }
}
