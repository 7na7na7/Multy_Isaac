using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float shotRadious;
    public GameObject TurretBullet;
    public float shotDelay = 2;
    private Vector2 position;
    private void Start()
    {
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        while (true)
        {
            yield return new WaitForSeconds(shotDelay);
            RaycastHit2D[] zombieCol = Physics2D.CircleCastAll(transform.position, shotRadious, Vector2.up,0);

            bool canFind = false;
            foreach (RaycastHit2D col in zombieCol)
            {
                if (col.collider.CompareTag("Enemy"))
                {
                    position = col.collider.transform.position;
                    canFind = true;
                    break;
                }
            }

            if (canFind)
            {
                Vector3 angle=new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, position.x, position.y)+90);
                if (PhotonNetwork.OfflineMode)
                {
                    setAngleRPC(angle);
                    Instantiate(TurretBullet, transform.position, Quaternion.Euler(angle));
                }
                else
                {
                    GetComponent<PhotonView>().RPC("setAngleRPC",RpcTarget.All);
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.InstantiateRoomObject(TurretBullet.name, transform.position, Quaternion.Euler(angle));
                    }
                }   
            }
            
        }
    }

    [PunRPC]
    void setAngleRPC(Vector3 angle)
    {
        transform.eulerAngles = angle;
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
    
