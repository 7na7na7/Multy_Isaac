using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AspaltMinimap : MonoBehaviour
{
    public GameObject bush;
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

        if (Random.Range(1, 11) == 1)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Vector2 pos = new Vector3(Random.Range(box.bounds.min.x, box.bounds.max.x), Random.Range(box.bounds.min.y, box.bounds.max.y));

            if (PhotonNetwork.OfflineMode)
                Instantiate(bush, pos, Quaternion.identity);
            else
            {
                if(PhotonNetwork.IsMasterClient)
                    PhotonNetwork.InstantiateRoomObject(bush.name, pos, Quaternion.identity);
            }
        }
    }
}
