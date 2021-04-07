using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailMarble : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, cam.ScreenToWorldPoint(Input.mousePosition).x,cam.ScreenToWorldPoint(Input.mousePosition).y)+90); 
    }

    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }
}
