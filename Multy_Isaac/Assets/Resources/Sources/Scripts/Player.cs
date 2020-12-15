using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject slash;
    
    
    public GameObject canvas;
    private bool isHaveGun = false;
    private TweenParams parms = new TweenParams();

    public Text Lv;
    //시작시 미니맵표시
    public LayerMask doorCol;
    public float radius;
    private Vector3 spawnPoint;
    //수면
    public bool isSleeping; //자고있는가?
    //이동, 애니메이션
    private int shotSpeed_p = 100;
    private int walkSpeed_p = 100;
    private CapsuleCollider2D col;
    public bool canMove = true;
    private Animator anim;
    private Vector2 moveDirection; 
    private Rigidbody2D rb;
    private float localScaleX;
    private Vector3 curPos;
    // public Animator headAnim; //다리위쪽 애니메이션
    public float speed;
    public float savedSpeed;
    public PhotonView pv; //포톤뷰
    //캔버스
    public RectTransform canvasRect; //캔버스 로컬스케일반전을 위해
   private float canvasLocalScaleX; //캔버스 로컬스케일반전을 위해
   public Text ChatBaloon; //말풍선
   public ChatBox chatbox; //챗박스
   public Text nickname; //닉네임
   public Slider hp;
   public Slider mp;
    //총쏘기
    public Transform bulletTr; //총알이 나가는 위치
    public float CoolTime = 0.2f; //총 쏘는 쿨타임
    private float time = 0; //쿨타임 계산을 위한 시간변수
    public GameObject offLineBullet; //오프라인 모드에서 나갈 총알
    public GameObject Arm; //팔
    private string bulletName;
    private Vector2 gunScale=Vector2.one;
    private Vector2 savedGunPos;
    private float clusterRate = 0;
    
    //구르기
    public bool isSuper = false; //무적인가?
    public Ease easeMode;
    public float rollTime;
    public float rollDistance;
    public float rollDelayMultiply;
    private bool canRoll = true;
    public int rollMp = 20;

    //회전부분함수
    public GameObject gun;
    private Vector3 MousePosition; //총 회전을 위한 변수
    private Camera camera;
    private float angle;
    
    private PlayerItem playerItem;
    private LevelMgr LvMgr;
    private StatManager statMgr;
    private ItemData itemData;
    
    public Sprite[] gunSprites;
    public LeftBullet leftBullet;
    
    private bool isReLoading = false;
    public Ease reLoadEase1;
    public Ease reLoadEase2;

    private SoundManager sound;
    private void Start()
    {
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.green : Color.red; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        localScaleX = transform.localScale.x;
        canvasLocalScaleX = canvasRect.localScale.x;
        col = GetComponent<CapsuleCollider2D>();
        
        savedGunPos = gun.transform.localPosition;
        speed = savedSpeed;
        if (pv.IsMine)
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                    //FindObjectOfType<CameraManager>().target = p.gameObject;
                    itemData = transform.GetChild(0).GetComponent<ItemData>();
                    LvMgr = transform.GetChild(0).GetComponent<LevelMgr>();
                    statMgr=transform.GetChild(0).GetComponent<StatManager>();
                    playerItem = GetComponent<PlayerItem>();
                    leftBullet = transform.GetChild(0).transform.GetChild(0).GetComponent<LeftBullet>();
                    sound = GetComponent<SoundManager>();
                    playerItem.player = this;
                    if (SceneManager.GetActiveScene().name == "Play")
                    {
                        GetComponent<CapsuleCollider2D>().isTrigger = true;
                        Invoke("setCam", 2f);
                    }
                    else
                    {
                        canMove = true;
                        statMgr.canMove = true;
                    }
                }
                else
                    Destroy(canvas);
       
    }
    void setCam()
    {
        GetComponent<CapsuleCollider2D>().isTrigger = false;
       spawnPoint = transform.position;
       canMove = true;
       statMgr.canMove = true;
        Destroy(GameObject.Find("LoadingPanel"));
        camera.transform.position=new Vector3(transform.position.x,transform.position.y-0.25f,-10);
        camera.GetComponent<CameraManager>().target = gameObject;


        //자신 기준으로 이내의 반경의 doorCol검색
            Collider2D col = Physics2D.OverlapCircle(transform.position, radius, doorCol);
            if (col != null) //플레이어가 비지 않았다면
            {
                col.GetComponent<DoorCol>().Minimap();
            }
            else
            {
                print("감지 실패!");
            }   
       
   }
    void roll(Vector2 dir)
    {
        if (statMgr.LoseMp(rollMp))
        {
            if (canRoll)
            {
                canMove = false;
                statMgr.canMove = false;
                canRoll = false;
                StartCoroutine(rollCor(dir,rollDistance));
            }   
        }
        else
        {
            print("Mp부족!");
        }
    }
    IEnumerator rollCor(Vector2 dir, float distance)
    {
        rb.velocity=Vector2.zero;
        if (PhotonNetwork.OfflineMode)
        {
            SetAnimRPC("Roll");
            SetAnimRPC("None");
        }
        else
        {
            pv.RPC("SetAnimRPC",RpcTarget.All,"Roll");
            pv.RPC("SetAnimRPC",RpcTarget.All,"None");
        }
      
        
        isSuper = true; //무적 ON
        Vector2 originalSize = col.size;
        col.size=new Vector2(col.size.x-0.02f,col.size.y-0.02f); //크기 아주조금 줄여서 콜라이더 벽에 닿아서 끊기는거 방지
        rb.DOMove(transform.position + new Vector3(dir.x*distance,dir.y*distance),rollTime).SetEase(easeMode).SetAs(parms);
        yield return new WaitForSeconds(rollTime);
        isSuper = false; //무적 OFF
        yield return new WaitForSeconds(rollTime*rollDelayMultiply); //스턴시간은 구르는시간의 10분의 1
        col.size = originalSize; //원래 크기로 돌려줌
        if (PhotonNetwork.OfflineMode)
        {
            SetAnimRPC("Idle" + (!isHaveGun ? "2" : null));
            SetAnimRPC("GoDown");
        }
        else
        {
            pv.RPC("SetAnimRPC", RpcTarget.All, "Idle" + (!isHaveGun ? "2" : null));
            pv.RPC("SetAnimRPC", RpcTarget.All, "GoDown");
        }


        canMove = true;
        statMgr.canMove = true;
        canRoll = true;
    }
    
    public RectTransform panel;
    private void Update()
    {

        if (pv.IsMine)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                    sound.Play(0);
                Lv.text = "Lv." + LvMgr.Lv; //레벨 표시
                
                if(time>0)  
                    time -= Time.deltaTime; //총쏘기 쿨타임용 시간 감소
                
                if (canMove) //움직일 수 있다면
                {
                    if (!isSleeping) //잠자고 있지 않다면
                    {
                        if (!RectTransformUtility.RectangleContainsScreenPoint(panel, Input.mousePosition)) //클릭불가능영역이 아니면
                        {
                            if (Input.GetMouseButtonDown(0) && gun.activeSelf && !isReLoading) //연타하면 더빠르게 쏨
                            { 
                                if(playerItem.ItemList[playerItem.selectedIndex].type==itemType.Gun)
                                {
                                    if (!leftBullet.canShoot())
                                    {
                                        speed = savedSpeed;

                                        if(leftBullet.canReload() && !isReLoading)
                                            reLoad(leftBullet.reLoadTime);
                                        else
                                            print("총알이 부족합니다!");
                                    }
                                    else
                                        ShotGun(true);
                                }
                                else if (playerItem.ItemList[playerItem.selectedIndex].type == itemType.Melee) //근접공격
                                {
                                    Slash(true);
                                }
                            }
                            if (Input.GetMouseButton(0) && gun.activeSelf && !isReLoading) //그냥 누르고있을경우
                            {
                                if (playerItem.ItemList[playerItem.selectedIndex].type == itemType.Gun)
                                {
                                    if (leftBullet.canShoot())
                                        ShotGun(false);
                                    else
                                        speed = savedSpeed;
                                }
                                else if (playerItem.ItemList[playerItem.selectedIndex].type == itemType.Melee)
                                {
                                    Slash(false);
                                }
                            }
                        }
                        
                        if (Input.GetKeyDown(KeyCode.R) && gun.activeSelf && leftBullet.isBulletMax()==false&& !isReLoading) //총 착용중이고, 총알이 꽉차지 않았고, R키를 눌렀을 시 재장전
                            reLoad(leftBullet.reLoadTime);

                      
                          
                        if (Input.GetMouseButtonUp(0)) //버튼에서 손을 떼면 원래속도로 돌아오기
                            speed = savedSpeed;
                        
                        if (Input.GetKeyDown(KeyCode.LeftShift)) //왼쪽쉬프트키 누르면 대쉬
                        {
                            Vector2 dir;
                            dir=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
                            if(dir!=Vector2.zero) 
                                roll(new Vector2(dir.x,dir.y).normalized); //방향 정해준후 대쉬
                        }   
                        
                        if (!isReLoading) //재장전중이 아닐때만 총, 플레이어로컬포지션 조정
                        {
                            if ((transform.position - camera.ScreenToWorldPoint(MousePosition)).normalized.x < 0) //커서가 오른쪽에 있으면
                            {
                                transform.localScale=new Vector3(localScaleX,transform.localScale.y,transform.localScale.z);
                                canvasRect.localScale = new Vector3(canvasLocalScaleX,canvasRect.localScale.y,canvasRect.localScale.z);

                                gun.transform.localScale=new Vector3(gunScale.x,gunScale.y,1);
                            }
                            else
                            {
                                transform.localScale=new Vector3(-1*localScaleX,transform.localScale.y,transform.localScale.z);
                                canvasRect.localScale = new Vector3(-1*canvasLocalScaleX,canvasRect.localScale.y,canvasRect.localScale.z);
                    
                                gun.transform.localScale=new Vector3(gunScale.x*-1,gunScale.y,1);
                            } //커서가 왼쪽에 있으면

                            
                            
                            //총 회전
                            MousePosition = Input.mousePosition;
                            Vector3 MousePosition2 = camera.ScreenToWorldPoint(MousePosition) - gun.transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
                            //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!
                            //MousePosition2.y -= 0.25f; //오차조정을 위한 코드
                            angle = Mathf.Atan2(MousePosition2.y, MousePosition2.x) * Mathf.Rad2Deg;
                            
                            if (Mathf.Abs(angle) > 90&&transform.localScale.x==localScaleX*-1) 
                            {
                                gun.transform.rotation = Quaternion.Euler(180, 0f, -1*angle);
                            } //총 로컬스케일 플레이어와 맞춰주기
                            else
                            {
                                gun.transform.rotation = Quaternion.Euler(0, 0f, angle);
                            }   
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.LeftControl)) //왼쪽컨트롤키를 누르면 잠자기/잠깨기
                    {
                        if (!isSleeping)
                        {
                            Sleep();
                        }
                        else
                        {
                           WakeUp();
                        }
                    }
                }
                
                GetMove(); //이동
            }
            else //만약 IsMine이 아니면
            {
                if ((transform.position - curPos).sqrMagnitude >= 100) //너무 많이 떨어져 있으면 순간이동
                    transform.position = curPos;
                else //조금 떨어져 있으면 Lerp로 자연스럽게 위치 동기화
                    transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);   
            }
        }

    void Slash(bool isDown)
    {
        bool canShot = false;
        if (isDown)
        {
            if (time <= 0.1f)
                canShot = true;
        }
        else
        {
            if (time <= 0)
            {
                canShot = true;
            }
        }
        
        if (canShot)
        {
            print(speed+" "+savedSpeed+" "+shotSpeed_p);
            speed = savedSpeed * shotSpeed_p/100;
            time = CoolTime;
            
            isReLoading = true;
            Vector3 a = gun.transform.eulerAngles;
            Vector3 a2 = a;
            a.z -=180;
            
            if (PhotonNetwork.OfflineMode) 
                Instantiate(slash,gun.transform.position,Quaternion.Euler(a2));
            else
                PhotonNetwork.Instantiate(slash.name,gun.transform.position,Quaternion.Euler(a2));

            gun.transform.DORotate(a, 0.25f).SetEase(reLoadEase1).OnComplete(()=> {
                StartCoroutine(swordInitial(a2, 0.1f));
            });   
        }
    }

    IEnumerator swordInitial(Vector3 a3,float delay)
    {
        yield return new WaitForSeconds(delay);
        isReLoading = false;
        gun.transform.eulerAngles = a3;
        speed = savedSpeed;
    }
        void FixedUpdate() 
        {
            if (canMove)
            {
                if (canMove&&!isSleeping&&pv.IsMine) //움직일 수 있고 자고있지 않으며 자신이라면
                {
                    //이동여부에 따른 애니메이션 조정
                    if (moveDirection == Vector2.zero)
                    {
                        if(PhotonNetwork.OfflineMode)
                        {
                            SetAnimRPC("Idle"+ (!isHaveGun ? "2" : null));   
                        }
                        else
                        {
                            pv.RPC("SetAnimRPC",RpcTarget.All,"Idle"+ (!isHaveGun ? "2" : null));
                        }
                    } 
                    else
                    {
                        if (PhotonNetwork.OfflineMode)
                        {
                            SetAnimRPC("Walk"+ (!isHaveGun ? "2" : null));
                        }
                        else
                        {
                            pv.RPC("SetAnimRPC",RpcTarget.All,"Walk"+ (!isHaveGun ? "2" : null));
                        }
                    
                    }
                    
                    rb.velocity=new Vector2(moveDirection.x*speed*walkSpeed_p/100,moveDirection.y*speed*walkSpeed_p/100); //이동
                }
                else//그 외는 전부 움직이지 않도록
                {
                    //  pv.RPC("SetAnimRPC",RpcTarget.All,false,"Idle");
                    rb.velocity=Vector2.zero;
                }   
            }
        }

        void ShotGun(bool isDown) //총쏘는 함수
        {
            bool canShot = false;
            if (isDown)
            {
                if (time <= 0.1f)
                    canShot = true;
            }
            else
            {
                if (time <= 0)
                {
                    canShot = true;
                }
            }

            if (canShot)
            {
                if (leftBullet.MinusBullet(playerItem.selectedIndex))
                {
                    speed = savedSpeed * shotSpeed_p / 100;
                    time = CoolTime;
                   Quaternion q=Quaternion.Euler(bulletTr.rotation.eulerAngles.x,bulletTr.rotation.eulerAngles.y,bulletTr.rotation.eulerAngles.z+Random.Range(-1f*clusterRate,clusterRate));
                    if (PhotonNetwork.OfflineMode) 
                        Instantiate(offLineBullet,bulletTr.position,q);
                    else
                        PhotonNetwork.Instantiate(bulletName, bulletTr.position, q);
                }
                else
                {
                    speed = savedSpeed;
                }
            }
        } 

        void reLoad(float reloadTime) //재장전
        {
            if (leftBullet.canReload())
            {
                Vector3 a = gun.transform.eulerAngles;
                a.z += 181;
                isReLoading = true;

                gun.transform.DORotate(a, reloadTime/2).SetEase(reLoadEase1).OnComplete(()=> {
                    Vector3 b = gun.transform.eulerAngles;
                    b.z += 181;
                    gun.transform.DORotate(b, reloadTime/2).SetEase(reLoadEase2).OnComplete(() =>
                    {
                        isReLoading = false;
                        leftBullet.Reload(playerItem.selectedIndex);
                    });
                });

            }
        }
        
        private void OnTriggerEnter2D(Collider2D other) //충돌함수
    {
        if (pv.IsMine)
        {
            if (other.CompareTag("Teleport")) //순간이동
            {
                StopAllCoroutines();
                
                    if (!isSleeping)
                    {
                        DOTween.Kill(parms);
                        canRoll = true;
                        canMove = true;
                        statMgr.canMove = true;
                        if (PhotonNetwork.OfflineMode)
                        {
                            SetAnimRPC("Idle"+ (!isHaveGun ? "2" : null));
                            SetAnimRPC("GoDown");
                        }
                        else
                        {
                            pv.RPC("SetAnimRPC",RpcTarget.All,"Idle"+ (!isHaveGun ? "2" : null));
                            pv.RPC("SetAnimRPC",RpcTarget.All,"GoDown");      
                        }
                    }

                    FindObjectOfType<Fade>().Teleport(this,GameObject.Find(other.name + "_T").transform.position);
            }
            
        }
    }

    public void GetBullet() 
    {
        leftBullet.GetBullet(1);
    }

    private void OnCollisionStay2D(Collision2D other)  //플레이어 강체 충돌판정
    {
        if (pv.IsMine)
        {
            if (other.gameObject.tag == "Wall") //벽에 닿으면 DOTween취소
            {
                if (!canRoll)
                {
                    DOTween.Kill(parms);
                }
            }   
        }
    }

    public void Die(string AttackerName) //죽을때 공격한사람 이름을 받아 로그띄울때 씀
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            if (AttackerName==PhotonNetwork.NickName)
            {
                InGameNetwork.instance.PV.RPC("ChatRPC", RpcTarget.All, 
                    nickname.text+"<color=red> Suicided </color>");   
            }
            else
            {
                InGameNetwork.instance.PV.RPC("ChatRPC", RpcTarget.All, 
                    AttackerName+"<color=red> Killed </color>"+ PhotonNetwork.NickName);      
            }

            playerItem.Dead();
            gunSetfalse();
        }
      statMgr.Heal(99999);
        transform.position = spawnPoint;
        camera.transform.position=new Vector3(transform.position.x,transform.position.y,-10);
    }
 

    public void Hit(int Damage,string HitName="") //공격받을때 공격한사람 이름도 받음
        {
            if (!isSuper&&pv.IsMine)
            {
                if(statMgr.LoseHp(Damage))
                    Die(HitName);
            }
        }

    void GetMove() //이동입력
        {
            if(Input.GetKeyDown(KeyCode.Return))
                rb.velocity=Vector2.zero;
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            
            moveDirection = new Vector2(moveX, moveY).normalized; //대각선 이동 정규화
        }
    
    public void changeWeapon(wep weapon, bool isFirst) //무기바꾸기
    {
        if (weapon.weaponIndex > 0)
        {
            if (PhotonNetwork.OfflineMode)
            {
                armgunSetTrue();
                //gun.GetComponent<SpriteRenderer>().sprite = weapon.spr;
                setSprite(weapon.weaponIndex);
            }
            else
            {
                pv.RPC("armgunSetTrue", RpcTarget.All); 
                //gun.GetComponent<SpriteRenderer>().sprite = weapon.spr;
                //setSprite(weapon.weaponIndex);
                pv.RPC("setSprite", RpcTarget.AllBuffered,weapon.weaponIndex); 
            }
                
            isHaveGun = true;
            Vector2 new_SavedGunPos=Vector2.zero; 
            Vector2 new_weapontr=Vector2.zero;


                new_weapontr = weapon.tr;
                new_SavedGunPos = savedGunPos;
            

            gun.transform.localPosition = new_SavedGunPos + new_weapontr;
            gunScale = weapon.scale;
            CoolTime = weapon.CoolTime;
            gun.transform.eulerAngles=Vector3.zero;
            bulletTr.localPosition =weapon.bulletPos.position;
            bulletName = weapon.BulletName;
            leftBullet.reLoadTime = weapon.reLoadTime;
            clusterRate = weapon.ClusterRate;
            shotSpeed_p = weapon.shotSpeed_P;
            walkSpeed_p = weapon.walkSpeed_P;
            leftBullet.SetBullet(weapon.BulletCount,playerItem.selectedIndex, isFirst);
        }
    } 

    public void gunSetfalse() //총내린상태로만들기
    {
        if (PhotonNetwork.OfflineMode)
            armgunSetFalse();
        else
            pv.RPC("armgunSetFalse", RpcTarget.All);
        leftBullet.SetFalse();
        isHaveGun = false;
        speed = savedSpeed;
        walkSpeed_p = 100;
        shotSpeed_p = 100;
    }

    public void getEXP(int value) //경험치획득
    {
        LvMgr.GetExp(value);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //변수 동기화
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(hp.value);
            stream.SendNext(mp.value);
            stream.SendNext(hp.maxValue); 
            stream.SendNext(mp.maxValue);
            stream.SendNext(angle);
            stream.SendNext(MousePosition);
            stream.SendNext(moveDirection);
            stream.SendNext(transform.localScale);
            stream.SendNext(canvasLocalScaleX);
            stream.SendNext(canvasRect.localScale);
            stream.SendNext(canMove);
            stream.SendNext(gun.transform.localScale);
            stream.SendNext(gun.transform.rotation);
            stream.SendNext(isSleeping);
            stream.SendNext(Lv.text);
            }
        else
        {
            curPos = (Vector3) stream.ReceiveNext();
            hp.value = (float) stream.ReceiveNext();
            mp.value = (float) stream.ReceiveNext();
            hp.maxValue = (float) stream.ReceiveNext();
            mp.maxValue = (float) stream.ReceiveNext();
            angle = (float) stream.ReceiveNext();
            MousePosition = (Vector3) stream.ReceiveNext();
            moveDirection = (Vector2) stream.ReceiveNext();
            transform.localScale = (Vector3) stream.ReceiveNext();
            canvasLocalScaleX = (float)stream.ReceiveNext();
            canvasRect.localScale = (Vector3) stream.ReceiveNext();
            canMove = (bool) stream.ReceiveNext();
            gun.transform.localScale = (Vector3) stream.ReceiveNext();
            gun.transform.rotation = (Quaternion) stream.ReceiveNext();
            isSleeping = (bool) stream.ReceiveNext();
            Lv.text = (string) stream.ReceiveNext();
        }
    }

        public void Sleep() //잠자기
        {
            isSleeping = true;
            statMgr.isSleeping = true;
            if (PhotonNetwork.OfflineMode)
            {
                anim.Play("Sleep");
                //headAnim.Play("None");
                if (isHaveGun)
                { 
                    armgunSetFalse();
                }
            }
            else
            {
                pv.RPC("SetAnimRPC",RpcTarget.All,"Sleep");
                pv.RPC("SetAnimRPC",RpcTarget.All,"None");
                if (isHaveGun)
                {
                    pv.RPC("armgunSetFalse", RpcTarget.All);
                }
            }
        }
        public void WakeUp() //잠깨기
        {
            isSleeping = false;
            statMgr.isSleeping = false;
            statMgr.tempHp=0;
            if (PhotonNetwork.OfflineMode)
            {
                anim.Play("Idle"+ (!isHaveGun ? "2" : ""));
                //headAnim.Play("GoDown");
                if (isHaveGun)
                { 
                    armgunSetTrue();
                }
            }
            else
            {
                pv.RPC("SetAnimRPC",RpcTarget.All,"Idle"+ (!isHaveGun ? "2" : ""));
                pv.RPC("SetAnimRPC",RpcTarget.All,"GoDown");
                if (isHaveGun)
                {
                    pv.RPC("armgunSetTrue", RpcTarget.All);
                }  
            }
        }
        
        [PunRPC]
        public void armgunSetFalse()
        {
            Arm.SetActive(false);
            gun.SetActive(false);
        }
        [PunRPC]
        public void armgunSetTrue()
        {
            Arm.SetActive(true);
            gun.SetActive(true);
        }
        [PunRPC]
        public void SetAnimRPC(string animName)
        {
            anim.Play(animName);
        }
        [PunRPC]
        public void ChatBaloonRPC(string txt)
        {
            chatbox.gameObject.SetActive(true);
            chatbox.SetTime();
            ChatBaloon.text = txt;
        }
        [PunRPC]
        public void Move(Vector3 pos)
        {
            transform.position = pos;
        }
        [PunRPC]
        void setSprite(int i)
        {
            //gun.GetComponent<SpriteRenderer>().sprite = itemData.GetWeapon(i).spr;
            gun.GetComponent<SpriteRenderer>().sprite = gunSprites[i - 1];
        }

}
