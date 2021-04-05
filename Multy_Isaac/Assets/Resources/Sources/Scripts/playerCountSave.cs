﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerCountSave : MonoBehaviour
{
   public int highscore;
   string highScorekey = "highScoreKey";
   public float soundValue;
   public string soundKey = "soundKey";
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
         soundValue = PlayerPrefs.GetFloat(soundKey, 1f);
         highscore = PlayerPrefs.GetInt(highScorekey, 0);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public int GetHighScore()
   {
      return highscore;
   }
   public void SetHighScore(int s)
   {
      if(s>highscore) 
         highscore = s;
      PlayerPrefs.SetInt(highScorekey,highscore);
   }
   public void SetSound(float v)
   {
      soundValue = v;
      PlayerPrefs.SetFloat(soundKey,soundValue);
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
