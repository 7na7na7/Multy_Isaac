using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool canMove = true;
    
    private Animator anim;
    private Vector2 moveDirection; 
    private Rigidbody2D rb;
    private float localScaleX;
    private Vector3 curPos;
    private float canvasLocalScaleX;
    private Vector3 canvasLoalScale;
    public Animator headAnim; //다리위쪽 애니메이션
    public float speed;
    public Text nickname; //닉네임
    public PhotonView pv; //포톤뷰
    public GameObject PlayerCanvas;
    public Slider hp; //체력
    public Slider mp; //기력
    public RectTransform canvasRect; //캔버스 로컬스케일반전을 위해
    //회전부분함수
    public GameObject gun;
    private Vector3 MousePosition; //총 회전을 위한 변수
    private Camera camera;
    private float angle;

    private void Awake()
    {
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.green : Color.red; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            camera=Camera.main;
            localScaleX = transform.localScale.x;
            canvasLocalScaleX = canvasRect.localScale.x;
    }

        private void Update()
        {
            if (pv.IsMine)
            {
                //커서에 따른 애니메이션변화
                if ((transform.position - camera.ScreenToWorldPoint(MousePosition)).normalized.y < -0.2f) //마우스커서가 위에있으면
                {
                    headAnim.SetBool("IsGoDown",false);
                }
                else //아래에있으면
                {
                    headAnim.SetBool("IsGoDown",true);
                }

                if ((transform.position - camera.ScreenToWorldPoint(MousePosition)).normalized.x < 0) //커서가 오른쪽에 있으면
                {
                    transform.localScale=new Vector3(localScaleX,transform.localScale.y,transform.localScale.z); 
                    canvasLoalScale=new Vector3(canvasLocalScaleX,canvasRect.localScale.y,canvasRect.localScale.z);
                    canvasRect.localScale = canvasLoalScale;
                }
                else
                {
                    transform.localScale=new Vector3(-1*localScaleX,transform.localScale.y,transform.localScale.z);
                    canvasLoalScale=new Vector3(-1*canvasLocalScaleX,canvasRect.localScale.y,canvasRect.localScale.z);
                    canvasRect.localScale = canvasLoalScale;
                }


                PlayerCanvas.transform.position = transform.position;
                GetMove();
            
                //총 회전
                MousePosition = Input.mousePosition;
                Vector3 MousePosition2 = camera.ScreenToWorldPoint(MousePosition) - transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
                //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!
                MousePosition2.y -= 0.25f; //오차조정을 위한 코드
                angle = Mathf.Atan2(MousePosition2.y, MousePosition2.x) * Mathf.Rad2Deg;
            
            
                if (Mathf.Abs(angle) > 90)
                {
                    gun.transform.rotation = Quaternion.Euler(180, 0f, -1*angle);
                }
                else
                {
                    gun.transform.rotation = Quaternion.Euler(0, 0f, angle);
                }

                if (transform.localScale.x == localScaleX)
                    pv.RPC("FlipXRPC",RpcTarget.AllBuffered,false);
                else
                    pv.RPC("FlipXRPC",RpcTarget.AllBuffered,true); 
            }
            //IsMine이 아닌 것들은 부드럽게 위치 동기화
            else if ((transform.position - curPos).sqrMagnitude >= 100)
                transform.position = curPos;
            else
                transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        
            if (Mathf.Abs(angle) > 90)
            {
                gun.transform.rotation = Quaternion.Euler(180, 0f, -1*angle);
            }
            else
            {
                gun.transform.rotation = Quaternion.Euler(0, 0f, angle);
            }
        }

        void FixedUpdate()
        {
            if (canMove)
            {
                if (moveDirection == Vector2.zero)
                {
                    anim.SetBool("IsWalk", false);
                }
                else
                {
                    anim.SetBool("IsWalk", true);
                }
                rb.velocity=new Vector2(moveDirection.x*speed,moveDirection.y*speed);
            }
            else
            {
                anim.SetBool("IsWalk", false);
               rb.velocity=Vector2.zero;
            }
        }

        void GetMove()
        {
            if(Input.GetKeyDown(KeyCode.Return))
                rb.velocity=Vector2.zero;
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            
            moveDirection = new Vector2(moveX, moveY).normalized; //대각선 이동 정규화
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //변수 동기화
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(hp.value);
                stream.SendNext(mp.value);
                stream.SendNext(angle);
                stream.SendNext(MousePosition);
                stream.SendNext(moveDirection);
                stream.SendNext(transform.localScale);
                stream.SendNext(canvasLocalScaleX);
                stream.SendNext(canvasLoalScale);
            }
            else
            {
                curPos = (Vector3) stream.ReceiveNext();
                hp.value = (float) stream.ReceiveNext();
                mp.value = (float) stream.ReceiveNext();
                angle = (float) stream.ReceiveNext();
                MousePosition = (Vector3) stream.ReceiveNext();
                moveDirection = (Vector2) stream.ReceiveNext();
                transform.localScale = (Vector3) stream.ReceiveNext();
                canvasLocalScaleX = (float)stream.ReceiveNext();
                canvasLoalScale = (Vector3) stream.ReceiveNext();
            }
        }


        [PunRPC]
        void FlipXRPC(bool isFlip)
        {
            gun.GetComponent<SpriteRenderer>().flipX = isFlip;
        }
}
