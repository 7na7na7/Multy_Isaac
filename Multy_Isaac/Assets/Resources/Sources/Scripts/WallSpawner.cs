using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
  public int dir;
  public GameObject wall;
  
  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Wall"))
    {
      wall.SetActive(true);
        Destroy(gameObject);
    }
  }
}
