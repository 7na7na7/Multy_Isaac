using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerCountSave : MonoBehaviour
{
   public Image[] imgs;
   private string playerKey = "playerKey";
   public int PlayerIndex = 0;
   public static playerCountSave instance;
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

   private void Start()
   {
      PlayerIndex = PlayerPrefs.GetInt(playerKey, 0);
   }

   public void SetIndex(int index)
   {
      PlayerIndex = index;
      PlayerPrefs.SetInt(playerKey,PlayerIndex);

      for (int i = 0; i < imgs.Length; i++)
      {
         if (i == PlayerIndex)
         {
            Color color= Color.white;
            color.a = 1f;
            imgs[i].color = color;
         }
         else
         {
            Color color= Color.white;
            color.a = 0.5f;
            imgs[i].color = color;
         }
      }
   }
}
