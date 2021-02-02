using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Delay_Photonview : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("del",delay);
    }

    void del()
    {
        Destroy(GetComponent<PhotonView>());
    }
}
