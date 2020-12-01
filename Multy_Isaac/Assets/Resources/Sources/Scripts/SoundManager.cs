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
   private int length;
   
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
           ClipSources[j].loop = false;
           ClipSources[j].playOnAwake = false;
       }

       length = clips.Length;
   }


   public void Play(int clipIndex)
   {
       if(PhotonNetwork.OfflineMode)
           PlayRPC(clipIndex);
       else
           pv.RPC("PlayRPC",RpcTarget.All,clipIndex);
   }
  
   
   [PunRPC]
   void PlayRPC( int clipIndex )
   {
       if (index == length)
           index = 0;
       
       ClipSources[index].PlayOneShot(clips[clipIndex]);
       print(index);
          
       index++;
   }
}
