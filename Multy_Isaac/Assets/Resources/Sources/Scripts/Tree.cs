using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    private SoundManager sound;
    public GameObject txt;
    private TemManager temMgr;
    private PhotonView pv;
    public int hp;
    public GameObject wood;
    public GameObject[] randomTems;
    public int percent;
    private int hpSave;
    public Transform tr;
    public float isSuperTime = 0.3f;
    private float time = 0;
    private void Start()
    {
        sound = GetComponent<SoundManager>();
        pv = GetComponent<PhotonView>();
        temMgr = FindObjectOfType<TemManager>();
        hpSave = hp;
    }

    private void OnMouseOver()
    {
        if (!txt.activeSelf)
            txt.SetActive(true);
    }

    private void OnMouseExit()
    {
        txt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slash"))
        {
            if (time > isSuperTime)
            {
                sound.Play(0, false, 0.75f);

                timeRPC();
                Hit(other.GetComponent<Slash>().Dmg, Random.Range(0, randomTems.Length), percentreturn(percent));
            }
        }
    }

    [PunRPC]
    void timeRPC()
    {
        time = 0;
    }
    private void Update()
    {
        if (time <= isSuperTime)
            time += Time.deltaTime;
    }

    [PunRPC]
    void Hit(int value,int randomValue, bool isRandomTem)
    {
        GetComponent<FlashWhite>().Flash();
                transform.DOScale(new Vector3(1.6f,1.6f), 0.15f).OnComplete(() =>
                {
                    transform.DOScale(new Vector3(1.5f,1.5f), 0.15f);
                });   
                if (hp-value > 0)
                    hp -= value;
                else
                {
                    hp = hpSave;
                    if (PhotonNetwork.OfflineMode)
                    {
                        temMgr.setTem(wood.GetComponent<Item>().item.index, tr.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)));
                    }
                    else
                    {
                        if(PhotonNetwork.IsMasterClient)
                            temMgr.setTem(wood.GetComponent<Item>().item.index, tr.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)));
                    }
                   
                }

                if (isRandomTem)
                {
                    if (PhotonNetwork.OfflineMode)
                    {
                        temMgr.setTem(randomTems[randomValue].GetComponent<Item>().item.index, tr.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)));
                    }
                    else
                    {
                        if(PhotonNetwork.IsMasterClient)
                            temMgr.setTem(randomTems[randomValue].GetComponent<Item>().item.index, tr.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)));
                    }
                }
    }

    bool percentreturn(int per)
    {
        if (Random.Range(1, 101) <= percent)
            return true;
        else
            return false;
    }
}
