using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakableObj : MonoBehaviour
{
   private AudioSource source;
   public AudioClip clip;
   public float volume=1;
   public float minPitch;
   public float maxPitch;
   private void Start()
   {
      source = GetComponent<AudioSource>();
      source.pitch = Random.Range(minPitch, maxPitch);
      source.spatialBlend = 1f;
      source.minDistance = 8;
      source.maxDistance = 20;
      source.rolloffMode = AudioRolloffMode.Linear;
      source.loop = false;
      source.playOnAwake = false;
   }

   public Sprite spr;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player") || other.CompareTag("Bullet")||other.CompareTag("Slash"))
      {
         //사운드출력, 인자로 뭐 주면 항아리깨지는소리 출력
         GetComponent<SpriteRenderer>().sprite = spr;
         Color c;
         c.r = 0.85f; c.g = 0.85f; c.b = 0.85f; c.a = 1;
         GetComponent<SpriteRenderer>().color = c;
         Destroy(GetComponent<Collider2D>());
         Destroy(GetComponent<Rigidbody2D>());
         Destroy(GetComponent<BreakableObj>());
         source.PlayOneShot(clip,volume);
      }
   }
}
