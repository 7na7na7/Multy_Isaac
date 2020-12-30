using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhite : MonoBehaviour
{
public Material flashWhite ;
private Material defaultMaterial;

SpriteRenderer[] s;

void Start ()
{
	s =  gameObject.GetComponentsInChildren<SpriteRenderer> ();
	defaultMaterial = s [0].material;
}

public void Flash()
{
	foreach (SpriteRenderer SR in s)
	{
		SR.material = flashWhite; 
	}
	Invoke ("HideChange", 0.1f);
}

void HideChange()
{
	foreach (SpriteRenderer SR in s) {
		SR.material = defaultMaterial;
	}
}

}
