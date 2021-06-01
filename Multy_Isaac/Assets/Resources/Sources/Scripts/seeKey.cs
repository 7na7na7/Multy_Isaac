using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seeKey : MonoBehaviour
{
    public int index;

    void Update()
    {
        GetComponent<Text>().text = KeySettings.instance.returnkey(index);
    }
}
