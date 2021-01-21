using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delay;

    void Start()
    {
        if (PhotonNetwork.OfflineMode)
           destroyRPC();
        else
        {
            GetComponent<PhotonView>().RPC("destroyRPC",RpcTarget.All);
        }
    }

    [PunRPC]
    void destroyRPC()
    {
        Destroy(gameObject,delay);
    }
}
