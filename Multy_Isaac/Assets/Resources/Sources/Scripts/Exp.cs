using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public float speed;
    public int ExpAmount = 1;
    public float radius;
    public LayerMask player;
    public Ease ease;

   
 
    void Update()
    {
        //자신 기준으로 radius반경의 plaeyer탐색
        Collider2D col = Physics2D.OverlapCircle(transform.position, radius, player);
        if (col != null) //플레이어가 비지 않았다면
        {
            transform.position=Vector2.Lerp(transform.position, col.transform.position, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().getEXP(ExpAmount);
            Destroy(gameObject);   
        }
    }
    
}
