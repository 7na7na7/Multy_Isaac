using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NailMarble : MonoBehaviour
{
    public PhotonView pv;
    private Camera cam;
    // Start is called before the first frame update
    public void ON()
    {
        if (PhotonNetwork.OfflineMode)
        {
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            moveRPC(-getAngle(transform.position.x, transform.position.y, cam.ScreenToWorldPoint(Input.mousePosition).x,cam.ScreenToWorldPoint(Input.mousePosition).y)+90);
        }
        else
        {
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            pv.RPC("moveRPC",RpcTarget.All,-getAngle(transform.position.x, transform.position.y, cam.ScreenToWorldPoint(Input.mousePosition).x,cam.ScreenToWorldPoint(Input.mousePosition).y)+90);
        }
    }
    
    [PunRPC]
    void moveRPC(float Z)
    {
        transform.eulerAngles = new Vector3(0, 0, Z); 
    }

    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }
}
