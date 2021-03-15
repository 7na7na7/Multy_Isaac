using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject TurretBullet;
    public float shotDelay = 2;
    private void Start()
    {
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        while (true)
        {
            yield return new WaitForSeconds(shotDelay);
            if (PhotonNetwork.OfflineMode)
            {
                Instantiate(TurretBullet, transform.position, Quaternion.identity);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateRoomObject(TurretBullet.name, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
