using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerLight : MonoBehaviour
{
    public float lightValue = 1.1f;
    public bool isPlayer = false;
    private Light2D pointLight;
    private Light2D globalLight;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            pointLight = GetComponent<Light2D>();
            globalLight = GameObject.Find("GlobalLight2D").GetComponent<Light2D>();

            if (!isPlayer)
            {
                pointLight.intensity =lightValue - globalLight.intensity;   
            }
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer && globalLight != null)
        {
            pointLight.intensity = lightValue - globalLight.intensity;   
        }
    }

    public void torchOn()
    {
        pointLight.pointLightOuterRadius += 3f;
    }

    public void torchOff()
    {
        pointLight.pointLightOuterRadius -= 3f;
    }
}
