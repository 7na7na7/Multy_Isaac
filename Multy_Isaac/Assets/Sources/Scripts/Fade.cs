using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    public float speed;
    private Image img;
    public Color fadecolor;
    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void fade()
    {
        StartCoroutine(fadeCor());
    }

    public void UnFade()
    {
        StartCoroutine(UnFadeCor());
    }

    public void Teleport(Player player, Vector3 tr)
    {
        if(fadecolor.a==0) 
            StartCoroutine(TeleportCor(player, tr));
    }

    IEnumerator TeleportCor(Player player, Vector3 tr)
    {
        print("A");
        player.canMove = false;
        print(player.canMove);
        fadecolor = img.color;
        while (fadecolor.a<1)
        {
            fadecolor.a += speed;
            yield return new WaitForSeconds(0.01f);
            img.color = fadecolor;
        }

        player.transform.position = tr;
        fadecolor = img.color;
        while (fadecolor.a>0)
        {
            fadecolor.a -= speed;
            yield return new WaitForSeconds(0.01f);
            img.color = fadecolor;
        }

        player.canMove = true;
        fadecolor.a = 0;
        img.color = fadecolor;
    }
    IEnumerator fadeCor()
    {
        fadecolor = img.color;
        fadecolor.a = 0;
        while (fadecolor.a<1)
        {
            fadecolor.a += speed;
            yield return new WaitForSeconds(0.01f);
            img.color = fadecolor;
        }
    }

    IEnumerator UnFadeCor()
    {
        fadecolor = img.color;
        while (fadecolor.a>0)
        {
            fadecolor.a -= speed;
            yield return new WaitForSeconds(0.01f);
            img.color = fadecolor;
        }
    }
    
}
