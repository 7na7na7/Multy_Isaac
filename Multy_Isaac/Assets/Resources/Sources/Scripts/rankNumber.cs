using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankNumber : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string s = transform.parent.gameObject.name;
        if (s.Length == 9)
        {
            string sum = s[6].ToString() + s[7].ToString();
            GetComponent<Text>().text = (int.Parse(sum)+1).ToString();
        }
        else
        {
            GetComponent<Text>().text = (int.Parse(s[6].ToString())+1).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
