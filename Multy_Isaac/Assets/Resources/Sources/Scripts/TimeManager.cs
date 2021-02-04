using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(PhotonNetwork.IsMasterClient) 
                pv.RPC("rpc", RpcTarget.All);
        }
    }
    [PunRPC]
    void rpc()
    {
        print("A");
    }
}
