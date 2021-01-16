using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour
{
    public float minMove;
    public float maxMove;
    public float speed;
    private Rigidbody2D rigid;
    private Vector3 startingPosition;
   private Vector3 roamingPosition;
   private void Start()
   {
       rigid = GetComponent<Rigidbody2D>();
      startingPosition = transform.position;
      roamingPosition = GetRoamingPosition();
      roamingPosition = GetRoamingPosition();
   }

   private void Update()
   {
       Vector2 dir = (roamingPosition - transform.position).normalized;
       rigid.velocity = dir * speed;

       float reachedPositionDistance = 1f;
       if (Vector2.Distance(transform.position, roamingPosition) < reachedPositionDistance)
       {
           //지정한 곳에 도착했으니 다시 값 줌
           roamingPosition = GetRoamingPosition();
       }
   }

   private Vector3 GetRoamingPosition()
   {
       float randomMove = Random.Range(minMove, maxMove);
       print(randomMove);
       return startingPosition + UtilsClass.GetRandomDir() * randomMove;
   }

   private void OnCollisionStay2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Wall"))
           roamingPosition = GetRoamingPosition();
   }
}
