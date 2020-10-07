using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Text text;
    public Outline outline;
    public float textFadeSpeed;
    public float fadeSpeed;
    public float goUpSpeed;
    public float canGoDelay;
    private bool canGo = false;
    private void Start()
    {
        StartCoroutine(fade());
        StartCoroutine(delayGo());
    }

    IEnumerator fade()
    {
        Color color;
        Color color2;
        
        color = text.color;
        color.a = 1;
        text.color = color;

        color2 = outline.effectColor;
        color2.a = 1;
        outline.effectColor = color2;
        
        while (true)
        {
            if (canGo)
            {
                if (color.a > 0)
                {
                    color.a -= textFadeSpeed;
                    text.color = color;
                    
                    color2.a -= textFadeSpeed;
                   outline.effectColor = color2;
                }
                else
                    break;
            }
            GetComponent<RectTransform>().anchoredPosition+=new Vector2(0,goUpSpeed);
            yield return new WaitForSeconds(fadeSpeed);
        }
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator delayGo()
    {
        yield return new WaitForSeconds(canGoDelay);
        canGo = true;
    }
}
