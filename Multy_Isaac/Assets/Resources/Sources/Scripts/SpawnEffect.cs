using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
  public GameObject mon;

  public void Spawn()
  {
   mon.SetActive(true);
   mon.transform.parent = null;
    Destroy(gameObject);
  }
}
