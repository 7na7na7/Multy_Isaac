using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = FindObjectOfType<ZombieSpawner>().FirstDelay;
    }

    void Update()
    {
        slider.value += Time.deltaTime;
    }
}
