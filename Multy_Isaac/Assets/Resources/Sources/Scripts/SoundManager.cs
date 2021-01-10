using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public PhotonView pv;
    public AudioClip[] clips;
   private AudioSource[] ClipSources;
   public int SourceCount;
   private int index = 0;

   private void Awake()
   {
       ClipSources = new AudioSource[SourceCount];
       for (int i = 0; i < SourceCount; i++)
       {
           ClipSources[i] = gameObject.AddComponent<AudioSource>();
           ClipSources[i].Stop();
       }
       
       for (int j = 0; j < SourceCount; j++)
       {
           ClipSources[j].spatialBlend = 1f;
           ClipSources[j].minDistance = 8;
           ClipSources[j].maxDistance = 12;
           ClipSources[j].rolloffMode = AudioRolloffMode.Linear;
           ClipSources[j].loop = false;
           ClipSources[j].playOnAwake = false;
       }
   }


   public void Play(int clipIndex, bool isRPC, float volume=1f)
   {
       if(!isRPC)
           PlayRPC(clipIndex,volume);
       else
       {
           if(PhotonNetwork.OfflineMode)
               PlayRPC(clipIndex,volume);
           else
               pv.RPC("PlayRPC",RpcTarget.All,clipIndex,volume);
       }
   }
  
   
   [PunRPC]
   void PlayRPC( int clipIndex ,float volume)
   {
       if (index == SourceCount)
               index = 0;
       
           ClipSources[index].PlayOneShot(clips[clipIndex],volume);
           index++;
   }
}
