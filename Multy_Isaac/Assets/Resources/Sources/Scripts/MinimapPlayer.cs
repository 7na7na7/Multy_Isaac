using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MinimapPlayer : MonoBehaviour
{
    private Player[] players;
    private PhotonView pv;
    private TimeManager time;
    public GameObject target;
    private Vector3 pos;
    public GameObject red;

    private void Start()
    {
        pos = transform.position;
        pv = GetComponent<PhotonView>();
        time = FindObjectOfType<TimeManager>();
        if (pv.IsMine)
        {
            red.SetActive(true);   
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(pos.x, pos.y, 0) + new Vector3(target.transform.position.x * 0.1f,
                                     target.transform.position.y * 0.1f, 0);
        }

        if (!pv.IsMine)
            {
                if (time.isNight)
                {
                    players = FindObjectsOfType<Player>();
                    foreach (Player p in players)
                    {
                        if (p.GetComponent<PhotonView>().IsMine)
                        {
                            if (p.passive.laderCount > 0)
                            {
                                GetComponent<SpriteRenderer>().color = Color.white;
                                return;
                            }
                        }
                    }
                    GetComponent<SpriteRenderer>().color=Color.clear;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                }   
            }

    }
}
