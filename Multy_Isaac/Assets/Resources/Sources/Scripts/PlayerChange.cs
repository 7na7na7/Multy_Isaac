using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChange : MonoBehaviour
{
    public string[] pTxts;
    public Text pTxt;
    public string[] pTxts2;
    public Text pTxt2;
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

      pTxt.text = pTxts[Index];
      pTxt2.text = pTxts2[Index];
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
        pTxt2.text = pTxts2[Index];
        pTxt.text = pTxts[Index];
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
        pTxt2.text = pTxts2[Index];
        pTxt.text = pTxts[Index];
        mainImg.sprite = sprites[Index]; 
        PlayerPrefs.SetInt(playerKey,Index);
        playerCountSave.instance.PlayerIndex = Index;
    }
}
