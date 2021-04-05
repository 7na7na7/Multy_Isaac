using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerCountSave : MonoBehaviour
{
   public int isFullScreen = 1;
   public string fullScreenKey = "fullScreenKey";
   public string resolutionKey = "resolutionKey";
   public int resolutionIndex = 0;
   public int PlayerIndex = 0;
   public Vector2[] resolutions;
   public static playerCountSave instance;
   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
         resolutionIndex = PlayerPrefs.GetInt(resolutionKey, 0);
         isFullScreen = PlayerPrefs.GetInt(fullScreenKey, 1);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void SetRes(int resIndex)
   {
      resolutionIndex = resIndex;
      PlayerPrefs.SetInt(resolutionKey,resolutionIndex);
   }

   public void SetFullScreen(bool isTrue)
   {
      if (isTrue)
         isFullScreen = 1;
      else
         isFullScreen = 0;
      PlayerPrefs.SetInt(fullScreenKey,isFullScreen);
   }
}
