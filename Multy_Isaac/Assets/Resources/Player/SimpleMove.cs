using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spr;
    private Vector2 moveDirection; 
    private Rigidbody2D rb;
    public float speed;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            GetMove();
        }

        void FixedUpdate()
        {
            rb.velocity=new Vector2(moveDirection.x*speed,moveDirection.y*speed);
        }

        void GetMove()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (moveX > 0) //오른쪽이동이면
                spr.flipX = false;
            else if(moveX<0)
                spr.flipX = true;
            moveDirection = new Vector2(moveX, moveY).normalized; //대각선 이동 정규화
            
            anim.SetFloat("Horizontal",moveDirection.x);
            anim.SetFloat("Vertical", moveDirection.y);
            anim.SetFloat("Speed",moveDirection.sqrMagnitude);
        }
}
