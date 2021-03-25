using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChange : MonoBehaviour
{
    public int Index = 0;
    public Sprite[] sprites;
    public Image leftImg;
    public Image mainImg;
    public Image rightImg;
    private string playerKey = "playerKey";

    private void Start()
    {
        playerCountSave.instance.PlayerIndex = PlayerPrefs.GetInt(playerKey, 0);
      Index = playerCountSave.instance.PlayerIndex;
      mainImg.sprite = sprites[Index];
      if (Index == 0)
      {
          leftImg.sprite = sprites[sprites.Length-1];
          rightImg.sprite = sprites[Index + 1];
      }
      else
      {
          rightImg.sprite = sprites[Index + 1];
          leftImg.sprite = sprites[Index  -1];
      }
    }

    public void Left()
    {
        Index--;
        if (Index == 0)
        {
            leftImg.sprite = sprites[sprites.Length-1];
            rightImg.sprite = sprites[Index + 1];
        }
        else if (Index == -1)
        {
            Index = sprites.Length - 1;
            rightImg.sprite = sprites[0];
            leftImg.sprite = sprites[Index  -1];
        }
        else
        {
            rightImg.sprite = sprites[Index + 1];
            leftImg.sprite = sprites[Index  -1];
        }
        mainImg.sprite = sprites[Index];
        PlayerPrefs.SetInt(playerKey,Index);
        playerCountSave.instance.PlayerIndex = Index;
    }

    public void Right()
    {
        Index++;
        if (Index == sprites.Length - 1)
        {
            rightImg.sprite =sprites[0];
            leftImg.sprite = sprites[Index  -1];
        }
        else if (Index == sprites.Length)
        {
            Index = 0;
            rightImg.sprite = sprites[1];
            leftImg.sprite = sprites[sprites.Length-1];
        }
        else
        {
            rightImg.sprite = sprites[Index + 1];
            leftImg.sprite = sprites[Index  -1];
        }
        mainImg.sprite = sprites[Index]; 
        PlayerPrefs.SetInt(playerKey,Index);
        playerCountSave.instance.PlayerIndex = Index;
    }
}
