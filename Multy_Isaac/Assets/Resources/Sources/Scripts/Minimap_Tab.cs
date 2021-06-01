using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Tab : MonoBehaviour
{
    public Animator anim;
        public Camera cam;
        public float bigSize;
        public float smallSize;
        private void Update()
        {
            if(Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("MAP"))))
                Open();
            if(Input.GetKeyUp((KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("MAP"))))
                Close();
        }

    
        void Open()
        {
            anim.Play("Open");
            cam.orthographicSize = bigSize;
        }

        void Close()
        {
            anim.Play("Close");
        }

        public void CloseSize()
        {
            cam.orthographicSize = smallSize;
        }
}
