using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private void DestroyF()
    {
        if(PhotonNetwork.OfflineMode)
            RPC();
        else
            GetComponent<PhotonView>().RPC("RPC",RpcTarget.All);
    }

    [PunRPC]
    void RPC()
    {
        Destroy(gameObject);
    }
}
