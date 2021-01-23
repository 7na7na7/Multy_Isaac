using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AspaltMinimap : MonoBehaviour
{
    public GameObject minimapObj;
    private void Start()
    {
        if(PhotonNetwork.OfflineMode) 
            Instantiate(minimapObj, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
        else
        {
            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.InstantiateRoomObject(minimapObj.name, GameObject.Find("minimapTr").transform.position+new Vector3(transform.position.x*0.1f,transform.position.y*0.1f), Quaternion.identity);
        }
    }
}
