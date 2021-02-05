using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    private Light2D pointLight;
    private Light2D globalLight;
    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponent<Light2D>();
        globalLight = GameObject.Find("GlobalLight2D").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        pointLight.intensity = 1 - globalLight.intensity;
    }
}
