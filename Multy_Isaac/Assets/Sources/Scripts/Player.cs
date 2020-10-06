using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Animator anim;
    private Vector2 moveDirection; 
    private Rigidbody2D rb;
    public float speed;
    
    //회전
    public GameObject gun;
    private Vector3 MousePosition; //총 회전을 위한 변수
    private Camera camera;
    private float angle;
    private float localScaleX;
    private void Start()
    {
        anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            camera=Camera.main;
            localScaleX = transform.localScale.x;
        }

        private void Update()
        {
           print((transform.position-Input.mousePosition).normalized);
            GetMove();
            
            //총 회전
            MousePosition = Input.mousePosition;
            MousePosition = camera.ScreenToWorldPoint(MousePosition) - transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
            //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!
            MousePosition.y -= 0.25f; //오차조정을 위한 코드
            angle = Mathf.Atan2(MousePosition.y, MousePosition.x) * Mathf.Rad2Deg;
            
            
            if (Mathf.Abs(angle) > 90)
            {
                gun.transform.rotation = Quaternion.Euler(180, 0f, -1*angle);
            }
            else
            {
                gun.transform.rotation = Quaternion.Euler(0, 0f, angle);
            }

            if (transform.localScale.x == localScaleX)
                gun.GetComponent<SpriteRenderer>().flipX = false;
            else
                gun.GetComponent<SpriteRenderer>().flipX = true;
        }

        void FixedUpdate()
        {
            if (moveDirection == Vector2.zero)
            {
                anim.Play("Idle");
            }
            else
            {
                anim.Play("Walk");
            }
            rb.velocity=new Vector2(moveDirection.x*speed,moveDirection.y*speed);
        }

        void GetMove()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (moveX > 0) //오른쪽이동이면
                transform.localScale=new Vector3(localScaleX,transform.localScale.y,transform.localScale.z);
            else if(moveX<0)
                transform.localScale=new Vector3(-1*localScaleX,transform.localScale.y,transform.localScale.z);
            moveDirection = new Vector2(moveX, moveY).normalized; //대각선 이동 정규화
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //변수 동기화
        {
          
        }

}
