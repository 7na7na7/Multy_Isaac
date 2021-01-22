using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject ver1;
    public GameObject ver2;
    public GameObject ver3;
    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            int r = Random.Range(0, 3);
            if (r == 0)
                ver1.SetActive(true);
            else if(r==1)
            {
                ver2.SetActive(true);
            }
            else
            {
                ver3.SetActive(true);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int r = Random.Range(0, 3);
                if (r == 0)
                {
                    GetComponent<PhotonView>().RPC("set1",RpcTarget.All);
                }
                else if(r==1)
                {
                    GetComponent<PhotonView>().RPC("set2",RpcTarget.All);
                }
                else
                {
                    GetComponent<PhotonView>().RPC("set3",RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void set1()
    {
        ver1.SetActive(true);
    }
    [PunRPC]
    void set2()
    {
        ver2.SetActive(true);
    }
    [PunRPC]
    void set3()
    {
        ver3.SetActive(true);
    }
}
