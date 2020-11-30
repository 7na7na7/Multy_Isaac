using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager instance;

   public AudioClip[] clips;
   private AudioSource[] ClipSources;
   public AudioSource BgmSource;

   private int index = 0;
   private int length;
   
   private void Awake()
   {
       if (instance == null)
       {
           instance = this;
           DontDestroyOnLoad(gameObject);
       }
       else
       {
           Destroy(gameObject);
       }
       
       ClipSources=new AudioSource[clips.Length];
       for (int i = 0; i < clips.Length; i++)
       {
           ClipSources[i] = gameObject.AddComponent<AudioSource>();
           ClipSources[i].Stop();
       }
       
       for (int j = 0; j < clips.Length; j++)
       {
           ClipSources[j].loop = false;
           ClipSources[j].playOnAwake = false;
       }

       length = clips.Length;
   }


   public void Play( int clipIndex )
   {
       if (index == length)
           index = 0;
       
       ClipSources[index].PlayOneShot(clips[clipIndex]);
       print(index);
          
       index++;
   }
   
}
