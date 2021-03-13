using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhite : MonoBehaviour
{
	public bool isLader = false;
	public Material laderMaterial;
public Material flashWhite ;
private Material defaultMaterial;

SpriteRenderer[] s;

private void Start()
{
	s =  gameObject.GetComponentsInChildren<SpriteRenderer>();
	defaultMaterial = s[0].material;
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

public void Lader()
{
	if (!isLader)
	{
		isLader = true;
		defaultMaterial = laderMaterial;
		foreach (SpriteRenderer SR in s)
		{
			SR.material = laderMaterial;
		}
	}
	
}
}
