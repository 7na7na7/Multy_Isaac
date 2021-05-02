using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subTranslate : MonoBehaviour
{
    public string EngText;
    void Start()
    {
        if (!FindObjectOfType<playerCountSave>().isKor())
            GetComponent<Text>().text = EngText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
