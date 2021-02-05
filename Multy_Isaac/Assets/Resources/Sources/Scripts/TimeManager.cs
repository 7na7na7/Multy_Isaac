using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using Light2D = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class TimeManager : MonoBehaviour
{
    private bool isDay = true;
    private Light2D globalLight;
    private PhotonView pv;
    public int time = 0;
    public int dayTime;
    public int dayToNightTime;
    public int nightTime;
    public int nightToDayTime;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        globalLight = GetComponent<Light2D>();
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (PhotonNetwork.OfflineMode)
            {
                time++;
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    time++;
                    pv.RPC("timeRPC", RpcTarget.All,time);
                }   
            }

            if (time > dayTime)
            {
                if (isDay)
                {
                    isDay = false;
                    //낮에서 밤으로 전환

                    DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0, dayToNightTime)
                        .SetEase(Ease.InQuad);
                } 
            }

            if (time > dayTime + dayToNightTime + nightTime)
            {
                if (!isDay)
                {
                    isDay = true;
                    //밤에서 낮으로 전환

                    DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1, nightToDayTime)
                        .SetEase(Ease.InQuad);
                    yield return new WaitForSeconds	(nightToDayTime	);
                    if (PhotonNetwork.OfflineMode)
                    {
                        time = 0;
                    }
                    else
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            time = 0;
                            pv.RPC("timeRPC", RpcTarget.All,time);
                        }   
                    }
                }
                }
            }
        }
    [PunRPC]
    void timeRPC(int value)
    {
        time = value;
    }
}
