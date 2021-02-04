using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    private PhotonView pv;

    public GameObject pointlight;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
           Destroy(pointlight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
