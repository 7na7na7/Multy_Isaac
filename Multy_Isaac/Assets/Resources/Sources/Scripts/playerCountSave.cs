using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCountSave : MonoBehaviour
{
   public static playerCountSave instance;
   public int playerCount;
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
   }
}
