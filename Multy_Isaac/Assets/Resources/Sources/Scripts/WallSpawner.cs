using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
   public bool canDetect = false;
  public GameObject wall;

  private void Start()
  {
    Invoke("detect",5f);
  }

  void detect()
  {
    canDetect = true;
  }
  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Wall")&&canDetect)
    {
      wall.SetActive(true);
        Destroy(gameObject);
    }
  }
}
