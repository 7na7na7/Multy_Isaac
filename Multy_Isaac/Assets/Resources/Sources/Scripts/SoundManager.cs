using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class gunSounds
{
    public AudioClip[] sounds;
}
public class SoundManager : MonoBehaviour
{
    private float volumeMultiply;
    public gunSounds[] guns;
    public float minDistance = 8f;
    public float maxDistance = 20f;
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
           ClipSources[j].minDistance = minDistance;
           ClipSources[j].maxDistance = maxDistance;
           ClipSources[j].rolloffMode = AudioRolloffMode.Linear;
           ClipSources[j].loop = false;
           ClipSources[j].playOnAwake = false;
       }
   }

   private void Start()
   {
       volumeMultiply = playerCountSave.instance.soundValue;
   }

   public void Play(int clipIndex, bool isRPC, float volume=1f)
   {
       if(!isRPC)
           PlayRPC(clipIndex,volume*volumeMultiply);
       else
       {
           if(PhotonNetwork.OfflineMode)
               PlayRPC(clipIndex,volume*volumeMultiply);
           else
               pv.RPC("PlayRPC",RpcTarget.All,clipIndex,volume*volumeMultiply);
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

   public void PlayGun(int clipIndex, bool isRPC, float volume = 1f, bool isReload = false)
   {
       if (isReload)
       {
           if(!isRPC)
               PlayGunRPC(clipIndex,guns[clipIndex].sounds.Length-1,volume*volumeMultiply);
           else
           {
               if(PhotonNetwork.OfflineMode)
                   PlayGunRPC(clipIndex,guns[clipIndex].sounds.Length-1,volume*volumeMultiply);
               else
                   pv.RPC("PlayGunRPC",RpcTarget.All,clipIndex,guns[clipIndex].sounds.Length-1,volume*volumeMultiply);
           }
       }
       else
       {
           if(!isRPC)
               PlayGunRPC(clipIndex,UnityEngine.Random.Range(0,guns[clipIndex].sounds.Length-1),volume*volumeMultiply);
           else
           {
               if(PhotonNetwork.OfflineMode)
                   PlayGunRPC(clipIndex,UnityEngine.Random.Range(0,guns[clipIndex].sounds.Length-1),volume*volumeMultiply);
               else
                   pv.RPC("PlayGunRPC",RpcTarget.All,clipIndex,UnityEngine.Random.Range(0,guns[clipIndex].sounds.Length-1),volume*volumeMultiply);
           }   
       }
   }


   [PunRPC]
   void PlayGunRPC( int gunIndex,int soundIndex,float volume)
   {
       if (index == SourceCount)
           index = 0;
       
       ClipSources[index].PlayOneShot(guns[gunIndex].sounds[soundIndex],volume);
       index++;
   }
}
