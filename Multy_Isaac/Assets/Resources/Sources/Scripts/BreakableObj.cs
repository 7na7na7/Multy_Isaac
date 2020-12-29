using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObj : MonoBehaviour
{
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
         Destroy(GetComponent<BreakableObj>());
      }
   }
}
