using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class fade1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("d",0.5f);
    }

    void d()
    {
        GetComponent<Image>().DOColor(Color.clear, .5f);
    }
}
