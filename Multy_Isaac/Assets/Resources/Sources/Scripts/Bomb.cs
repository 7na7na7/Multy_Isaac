using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("explode",delay);
    }

    void explode()
    {
        if(PhotonNetwork.OfflineMode) 
            Instantiate(explosion, transform.position, quaternion.identity);
        else
            PhotonNetwork.Instantiate(explosion.name, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

}
