using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;
using Light2D = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class TimeManager : MonoBehaviour
{
    public ZombieSpawner spawner;
    public Text dayText;
    public int day = 1;
    private float maxTime;
    public RectTransform ClockRect;
    public bool isNight = false;
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
        maxTime = dayTime + dayToNightTime + nightTime + nightToDayTime;
        spawner.DaybyDay(day);
    }

    private void Update()
    {
        ClockRect.eulerAngles=new Vector3( ClockRect.eulerAngles.x, ClockRect.eulerAngles.y, time/maxTime*-360);
        dayText.text = "Day " + day;
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

            if (time > dayTime + dayToNightTime)
            {
                if (!isNight)
                {
                    if (PhotonNetwork.OfflineMode)
                    {
                        setIsNight(true);
                    }
                    else
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            pv.RPC("setIsNight", RpcTarget.All,true);
                        }   
                    }
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
                        day++;
                        spawner.DaybyDay(day);
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
            if (time ==0)
            {
                if (isNight)
                {
                    if (PhotonNetwork.OfflineMode)
                    {
                        setIsNight(false);
                    }
                    else
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            pv.RPC("setIsNight", RpcTarget.All,false);
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

    [PunRPC]
    void setIsNight(bool istrue)
    {
        isNight = istrue;
    }
}
