using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Spaces : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            
        }
        else
        {
            if(!PhotonNetwork.IsMasterClient)
                Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
