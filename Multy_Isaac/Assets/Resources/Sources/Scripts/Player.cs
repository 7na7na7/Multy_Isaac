using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using Pathfinding;
using Pathfinding.Examples;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    #region 변수선언

    public enum  bulletType
    {
        common,snow,mushroom,electric
    }

    private int shotSoundIndex;
    private float volume;
    private bool isHouse = false;
    private offlineStat offStat;
    private TimeManager timeMgr;
    public float fireDamageTick;
    private float fireTime=0;
    public bool isTEST = true;
    private bool isRegen = true;
    public float hpRegenDelay=1f;
    public int hpRegenCut=70;
    public bool SUPERRRRRRR = true;
    private PassiveItem passive;
    
    private GameObject offlineSlash;

     public RectTransform panel;
     public RectTransform panel2;
     
    public GameObject canvas;
    private bool isHaveGun = false;

    //이동, 애니메이션
    public Ease nuckBackEase;
    public float nuckBackTime = 0.2f;
    public bool canMove = true;
    private Animator anim;
    private Vector2 moveDirection; 
    private Rigidbody2D rb;
    private float localScaleX;
    private Vector3 curPos; 
    public float footCountCut = 10; //10거리마다 발자국소리 재생
    private float footCount; //저장변수
    private bool isPan1 = false; //나무판인가?
    private bool isPan2 = false;//철판인가?
    public int pan1SpeedPercent = 40;
    public int pan2SpeedPercent = 40;
    public float speed; //속도
    public float savedSpeed; //속도 저장변수
    
    //배고픔
    public int hungrySpeed;
    public int hungryLessHpSpeed;


    public PhotonView pv; //포톤뷰
    //캔버스
    public GameObject photonviewCanvas; //포톤뷰캔벗,
   public RectTransform canvasRect; //캔버스 로컬스케일반전을 위해
   private float canvasLocalScaleX; //캔버스 로컬스케일반전을 위해
   public Text ChatBaloon; //말풍선
   public ChatBox chatbox; //챗박스
   public Text nickname; //닉네임
   public Slider hp;
   //총쏘기
   private float soundRadious;
    public Transform bulletTr; //총알이 나가는 위치
    private float time = 0; //쿨타임 계산을 위한 시간변수
    private GameObject offLineBullet; //오프라인 모드에서 나갈 총알
    public GameObject Arm; //팔
    private Vector2 savedGunPos;
    private Animator gunAnim;
    private bool canReload = false;
    private wep currentWeapon=new wep();
    //구르기
    public bool isSuper = false; //무적인가?

    //회전부분함수
    public GameObject gun;
    private Vector3 MousePosition; //총 회전을 위한 변수
    private Camera camera;
    private float angle;
    
    private PlayerItem playerItem;
    private StatManager statMgr;
    private TemManager temMgr;
    
    public LeftBullet leftBullet;
    
    private bool isReLoading = false;
    public Ease reLoadEase1;
    public Ease reLoadEase2;

    private SoundManager sound;

    private Vector3 savedCanvasScale;
    //죽음
    public bool isDead;

    private bool isAspalt = false;
    #endregion

    #region 내장함수
    private void Start()
    {
        timeMgr = FindObjectOfType<TimeManager>();
        temMgr = FindObjectOfType<TemManager>();
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.green : Color.red; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        localScaleX = transform.localScale.x;
        canvasLocalScaleX = canvasRect.localScale.x;

        savedGunPos = gun.transform.localPosition;
        speed = savedSpeed;

        gunAnim = gun.GetComponent<Animator>();
        currentWeapon.walkSpeed_P = 100;
        savedCanvasScale = photonviewCanvas.transform.localScale;
        if (pv.IsMine)
        {
            passive = GetComponent<PassiveItem>();
            StartCoroutine(hpRegenCor());
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            statMgr=transform.GetChild(0).GetComponent<StatManager>();
            playerItem = GetComponent<PlayerItem>();
            leftBullet = transform.GetChild(0).transform.GetChild(0).GetComponent<LeftBullet>();
            sound = GetComponent<SoundManager>();
            playerItem.player = this;
            offStat	=transform.GetChild(0).GetComponent<offlineStat>();
            if (SceneManager.GetActiveScene().name == "Play")
            {
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                if (!isTEST)
                {
                    Invoke("aspaltSet",FindObjectOfType<RoomTemplates>().delay);
                    Invoke("setCam",FindObjectOfType<ZombieSpawner>().FirstDelay+0.5f);   
                }
                else
                {
                    setCam();
                }
            }
            else
            {
                canMove = true;
                camera.GetComponent<CameraManager>().target = gameObject;
            }
        }
        else
        {
            Destroy(canvas);
            Destroy(GetComponent<AudioListener>());
        }
    }

    void aspaltSet()
    {
        GameObject[] aspalts = GameObject.FindGameObjectsWithTag("Aspalt");
        transform.position = aspalts[Random.Range(0, aspalts.Length)].transform.position;
    }
    private void Update()
    {
        if (pv.IsMine)
            {
//                //소리 테스트코드
//                if(Input.GetKeyDown(KeyCode.Q))
//                    sound.Play(0,true);

                if (fireTime > 0)
                    fireTime -= Time.deltaTime;
                
                
                if(time>0)  
                    time -= Time.deltaTime; //총쏘기 쿨타임용 시간 감소
             
                    if(Input.GetKeyDown(KeyCode.Z))
                        temMgr.setTem(3,transform.position);
                    if(Input.GetKeyDown(KeyCode.X))
                       temMgr.delTem(1);
                
                if (canMove) //움직일 수 있다면
                {
                    if (passive.mobilePer > 100)
                    {
                        if (passive.mobileTime < passive.savedMobileTime)
                            passive.mobileTime += Time.deltaTime;   
                    }

                    if (!RectTransformUtility.RectangleContainsScreenPoint(panel, Input.mousePosition)&&!RectTransformUtility.RectangleContainsScreenPoint(panel2, Input.mousePosition)) //클릭불가능영역이 아니면
                        {
                            if (Input.GetMouseButtonDown(0) && gun.activeSelf && !isReLoading) //연타하면 더빠르게 쏨
                            { 
                                if(playerItem.ItemList[playerItem.selectedIndex].type==itemType.Gun)
                                {
                                    if (leftBullet.bulletCount<currentWeapon.consumeBullet) //쏘는데쏠총알수보다 총알이 적을경우 재장전
                                    {
                                        speed = savedSpeed;

                                        if (leftBullet.canReload() && !isReLoading)
                                        {
                                            if(PhotonNetwork.OfflineMode)
                                                ReLoad(leftBullet.reLoadTime);
                                            else
                                                pv.RPC("ReLoad",RpcTarget.All,leftBullet.reLoadTime);
                                        }
                                           
                                        else
                                            noBulletSound();
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
                                    if (leftBullet.bulletCount>=currentWeapon.consumeBullet)
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
                        
                        if (Input.GetKeyDown(KeyCode.R) && gun.activeSelf && leftBullet.isBulletMax()==false&& !isReLoading&&leftBullet.canReload()) //총 착용중이고, 총알이 꽉차지 않았고, R키를 눌렀을 시 재장전
                        {
                            if(PhotonNetwork.OfflineMode)
                                    ReLoad(leftBullet.reLoadTime);
                                else
                                    pv.RPC("ReLoad",RpcTarget.All,leftBullet.reLoadTime);
                        }
                        
                        if (!isReLoading) //재장전중이 아닐때만 총, 플레이어로컬포지션 조정
                        {
                            if ((transform.position - camera.ScreenToWorldPoint(MousePosition)).normalized.x < 0) //커서가 오른쪽에 있으면
                            {
                                transform.localScale=new Vector3(localScaleX,transform.localScale.y,transform.localScale.z);
                                canvasRect.localScale = new Vector3(canvasLocalScaleX,canvasRect.localScale.y,canvasRect.localScale.z);

                                gun.transform.localScale=new Vector3(currentWeapon.scale.x,currentWeapon.scale.y,1);
                            }
                            else
                            {
                                transform.localScale=new Vector3(-1*localScaleX,transform.localScale.y,transform.localScale.z);
                                canvasRect.localScale = new Vector3(-1*canvasLocalScaleX,canvasRect.localScale.y,canvasRect.localScale.z);
                    
                                gun.transform.localScale=new Vector3(currentWeapon.scale.x*-1,currentWeapon.scale.y,1);
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
                
                GetMove(); //이동
            }
            else //만약 IsMine이 아니면
            {
                if ((transform.position - curPos).sqrMagnitude >= 100) //너무 많이 떨어져 있으면 순간이동
                    transform.position = curPos;
                else //조금 떨어져 있으면 Lerp로 자연스럽게 위치 동기화
                    transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);   
                
                    if (timeMgr.isNight)
                    {
                        nickname.gameObject.SetActive(false);
                        hp.gameObject.SetActive(false);
                    }
                    else
                    {
                        nickname.gameObject.SetActive(true);
                        hp.gameObject.SetActive(true);
                    }   
                
            }
        }
    
    void FixedUpdate() 
    {
        if (!isDead) //안죽었으면
        {
            if (canMove)
            {
                if (canMove && pv.IsMine) //움직일 수 있고 자고있지 않으며 자신이라면
                {
                    //이동여부에 따른 애니메이션 조정
                    if (moveDirection == Vector2.zero)
                    {
                        if (PhotonNetwork.OfflineMode)
                        {
                            SetAnimRPC("Idle");
                        }
                        else
                        {
                            pv.RPC("SetAnimRPC", RpcTarget.All, "Idle");
                        }
                    }
                    else
                    {
                        if (PhotonNetwork.OfflineMode)
                        {
                            SetAnimRPC("Walk");
                        }
                        else
                        {
                            pv.RPC("SetAnimRPC", RpcTarget.All, "Walk");
                        }

                    }

                    float PanValue = 1;
                    if (isPan1)
                        PanValue = pan1SpeedPercent / 100f;
                    else if (isPan2)
                        PanValue = pan2SpeedPercent / 100f;
                    
                    rb.velocity = new Vector2(
                        (moveDirection.x * (speed+speed*(passive.Speed*0.01f)) * (passive.machineLegCount<=0 ? currentWeapon.walkSpeed_P: 100) / 100 *
                         (passive.mobileTime >= passive.savedMobileTime ? passive.mobilePer / 100f : 1)) *
                        PanValue,
                        (moveDirection.y * (speed+speed*(passive.Speed*0.01f)) * (passive.machineLegCount<=0 ? currentWeapon.walkSpeed_P: 100) / 100 *
                         (passive.mobileTime >= passive.savedMobileTime ? passive.mobilePer / 100f : 1)) *
                    PanValue);
                    
                    anim.SetFloat("WalkSpeed",(((speed+speed*(passive.Speed*0.01f)) * currentWeapon.walkSpeed_P /
                                               100 * (passive.mobileTime >= passive.savedMobileTime ? passive.mobilePer / 100f : 1)) * PanValue)/4f);

                        //방향 x 속도 x 무기속도 x 늪속도 x 기동신속도 * 가시판에있는지
                }
                else //그 외는 전부 움직이지 않도록
                {
                    //  pv.RPC("SetAnimRPC",RpcTarget.All,false,"Idle");
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }
    #endregion

    #region 일반함수,코루틴

    IEnumerator hpRegenCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(hpRegenDelay);
            if(offStat.getHungry()>hpRegenCut && !isDead) 
                statMgr.Heal(1);
        }
    }
    public void isFight() //전투 중
    {
        passive.mobileTime = 0;
    }
    void setCam()
    {
        GetComponent<CapsuleCollider2D>().isTrigger = false;
        canMove = true;
        Destroy(GameObject.Find("LoadingPanel"));
        camera.GetComponent<CameraManager>().target = gameObject;
        MinimapPlayer[] targets = FindObjectsOfType<MinimapPlayer>();
        foreach (MinimapPlayer target in targets)
        {
            if (target.GetComponent<PhotonView>().IsMine)
                target.target = gameObject;
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
            isFight();
            StartCoroutine(speedCor());
            time = currentWeapon.CoolTime;
            
            isReLoading = true;
            sound.PlayGun(shotSoundIndex,true,volume);
            Vector3 a = gun.transform.eulerAngles;
            Vector3 a2 = a;
            a.z -=135;
         
            if (PhotonNetwork.OfflineMode) 
                Instantiate(offlineSlash,bulletTr.position,Quaternion.Euler(a2));
            else
                PhotonNetwork.Instantiate(offlineSlash.name,gun.transform.position,Quaternion.Euler(a2));

            if(playerItem.getCurrentTem().weaponIndex!=130) //전기톱
            {
                gun.transform.DORotate(a, currentWeapon.slashTime).SetEase(Ease.OutCubic).OnComplete(()=> {
                    StartCoroutine(swordInitial(a2, 0.05f)); });   
            }
            else
            {
                StartCoroutine(swordInitial(a2, 0.05f));  
            }
            //총소리 듣고 좀비오기
            RaycastHit2D[] zombieCol = Physics2D.CircleCastAll(gun.transform.position, soundRadious, Vector2.up,0);
            foreach (RaycastHit2D col in zombieCol)
            {
                if (col.collider.CompareTag("Enemy"))
                {
                    if (PhotonNetwork.OfflineMode)
                    {
                        col.collider.GetComponent<Enemy>().setPlayer(transform);    
                    }
                    else
                    {
                        col.collider.GetComponent<Enemy>().setPlayerRPC(pv.ViewID);
                    }
                }
            }   
        }
    }

    IEnumerator swordInitial(Vector3 a3,float delay)
    {
        yield return new WaitForSeconds(delay);
        isReLoading = false;
        gun.transform.eulerAngles = a3;
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

        if (canShot&&leftBullet.bulletCount>=currentWeapon.consumeBullet)
        {
            isFight();
            if (leftBullet.MinusBullet(playerItem.selectedIndex,currentWeapon.consumeBullet)) //쏘기!!!!!
            {
                sound.PlayGun(shotSoundIndex,true,volume);
                if(playerItem.getCurrentTem().weaponIndex==7) //식빵총이면
                   offStat.HungryHeal(1); //체력 1회복
              
                if(PhotonNetwork.OfflineMode)
                    gunAnimRPC(currentWeapon.weaponIndex.ToString(),false);
                else
                    pv.RPC("gunAnimRPC",RpcTarget.All,currentWeapon.weaponIndex.ToString(),false);


                StartCoroutine(speedCor());
                time = currentWeapon.CoolTime;
                   
                for (int i = 0; i < currentWeapon.shotBullet; i++)
                {
                    Quaternion q=Quaternion.Euler(bulletTr.rotation.eulerAngles.x,bulletTr.rotation.eulerAngles.y,bulletTr.rotation.eulerAngles.z+Random.Range(-1f*currentWeapon.ClusterRate,currentWeapon.ClusterRate));
                    if (PhotonNetwork.OfflineMode) 
                        Instantiate(offLineBullet,bulletTr.position,q);
                    else
                        PhotonNetwork.Instantiate(offLineBullet.name, bulletTr.position, q);
                }

                if (passive.Silence <=0) //소음기 안꼈으면
                {
                    //총소리 듣고 좀비오기
                    RaycastHit2D[] zombieCol = Physics2D.CircleCastAll(gun.transform.position, soundRadious, Vector2.up,0);
                    foreach (RaycastHit2D col in zombieCol)
                    {
                        if (col.collider.CompareTag("Enemy"))
                        {
                            if (PhotonNetwork.OfflineMode)
                            {
                                col.collider.GetComponent<Enemy>().setPlayer(transform);    
                            }
                            else
                            {
                                col.collider.GetComponent<Enemy>().setPlayerRPC(pv.ViewID);
                            }
                        }
                    }   
                }
            }
            else
            {
                speed = savedSpeed;
            }
        }
    }
    
    IEnumerator speedCor()
    {
        speed = savedSpeed * currentWeapon.shotSpeed_P / 100;
        yield return new WaitForSeconds(0.1f);
        speed = savedSpeed;
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

            GetComponent<PassiveItem>().StopLader();
            playerItem.Dead();
            gunSetfalse();
        }
//        statMgr.Heal(99999);
//        transform.position = spawnPoint;
        isDead = true;
        canMove = false;
if(PhotonNetwork.OfflineMode)
    DieRPC();
else
pv.RPC("DieRPC",RpcTarget.All);
        if(PhotonNetwork.OfflineMode) 
            SetAnimRPC("Die");
        else
            pv.RPC("SetAnimRPC",RpcTarget.All,"Die");
    }

    [PunRPC]
    void DieRPC()
    {
        rb.velocity=Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<CapsuleCollider2D>());
    }
    public void Hit(int Damage,string HitName,float nuckBackDistance,Vector3 pos=default(Vector3),bulletType type=bulletType.common) //공격받을때 공격한사람 이름도 받음
    {
        if (!isSuper&&pv.IsMine && !isDead && !SUPERRRRRRR)
        {
            switch (type)
            {
                case bulletType.snow:
                    break;
                case bulletType.mushroom:
                    break;
                case bulletType.electric:
                    pos=Vector3.zero;
                    canMove = false;
                    rb.velocity=Vector2.zero;
                    rb.DOMove(transform.position, nuckBackTime*2).SetEase(nuckBackEase).OnComplete(()=> { if(!isDead) canMove = true; });
                    break;
            }
            
            //StartCoroutine(superTick()); //0.1초 무적

            if (pos != Vector3.zero)
            {
                Vector3 dir = (transform.position - pos).normalized;
                canMove = false;
                rb.velocity=Vector2.zero;
                rb.DOMove(transform.position+dir * nuckBackDistance, nuckBackTime).SetEase(nuckBackEase).OnComplete(()=> { if(!isDead) canMove = true; });   
            }

            if (PhotonNetwork.OfflineMode) 
                flashWhiteRPC();
            else
                pv.RPC("flashWhiteRPC",RpcTarget.All); 
            
            isFight();
            if(statMgr.Hit(Damage))
                Die(HitName);
        }
    }

    [PunRPC]
    void flashWhiteRPC()
    {
        GetComponent<FlashWhite>().Flash();
    }
    IEnumerator superTick()
    {
        isSuper = true;
        yield return new WaitForSeconds(0.1f);
        isSuper = false;
    }
    void GetMove() //이동입력
    {
//        if(Input.GetKeyDown(KeyCode.Return))
//            rb.velocity=Vector2.zero;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
            
        moveDirection = new Vector2(moveX, moveY).normalized; //대각선 이동 정규화
        if (canMove)
            footCount += rb.velocity.sqrMagnitude/100;
        if (footCount > footCountCut)
        {
            if(isHouse)
                sound.Play(1,true,1.2f);
            else if(isAspalt)
                sound.Play(2,true,0.4f);
            else
            {
                sound.Play(0,true,0.4f);
            }
            footCount = 0;
        }
    }
    public void noBulletSound()
    {
        sound.Play(9,false,0.5f);
    }
    public void PutBombSound()
    {
        sound.Play(8,false,0.5f);
    }
    public void GetSound()
    {
        sound.Play(7,false,0.5f);
    }
    public void ChangeWeaponSound()
    {
        sound.Play(5,false,0.5f);
    }
    public void discardSound()
    {
        sound.Play(6,false,0.5f);
    }
    public void CombineSound()
    {
        sound.Play(3,true,0.5f);
    }
    public void eatSound()
    {
        sound.Play(4,true,0.5f);
    }
    public void loseHP()
    {
        if (!isDead)
        {
            if(statMgr.LoseHp(1))
                Die("배고픔");   
        }
    }
    public void changeWeapon(wep weapon, bool isFirst) //무기바꾸기
    {
        if (weapon.weaponIndex > 0)
        {
            if (PhotonNetwork.OfflineMode)
            {
                armgunSetTrue();
                //setSprite(weapon.weaponIndex);
                gunAnimRPC(weapon.weaponIndex.ToString(),true);
            }
            else
            {
                pv.RPC("armgunSetTrue", RpcTarget.All);
                //pv.RPC("setSprite", RpcTarget.AllBuffered,weapon.weaponIndex);
                pv.RPC("gunAnimRPC",RpcTarget.All,weapon.weaponIndex.ToString(),true);
            }
            isHaveGun = true;
            currentWeapon = weapon.DeepCopy();
            gun.transform.localPosition = currentWeapon.tr + savedGunPos;
            gun.transform.eulerAngles=Vector3.zero;
            bulletTr.localPosition =currentWeapon.bulletPos.position;
            leftBullet.reLoadTime = currentWeapon.reLoadTime;
            leftBullet.SetBullet(currentWeapon.BulletCount,playerItem.selectedIndex, isFirst);
            offlineSlash = currentWeapon.OfflineSlash;
            offLineBullet = currentWeapon.offlineBullet;
            soundRadious = currentWeapon.soundRadious;
            volume = currentWeapon.volume;
            shotSoundIndex=currentWeapon.shotSoundIndex-1;
            if (gun.GetComponent<Animator>()!=null) //애니메이터가 있으면
                gun.GetComponent<Animator>().enabled = true;
        }
    }

    public void KillReload()
    {
        isReLoading = false;
        canReload = false;
    }
    public void gunSetfalse() //총내린상태로만들기
    {
        KillReload();
        if (PhotonNetwork.OfflineMode)
            armgunSetFalse();
        else
            pv.RPC("armgunSetFalse", RpcTarget.All);
        leftBullet.SetFalse();
        isHaveGun = false;
        speed = savedSpeed;
        currentWeapon.walkSpeed_P = 100;
        currentWeapon.shotSpeed_P = 100;
    }


    #endregion

    #region 동기화, RPC함수

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //변수 동기화
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(hp.value);
            stream.SendNext(hp.maxValue);
            stream.SendNext(angle);
            stream.SendNext(MousePosition);
            stream.SendNext(moveDirection);
            stream.SendNext(transform.localScale);
            stream.SendNext(canvasLocalScaleX);
            stream.SendNext(canvasRect.localScale);
            stream.SendNext(canMove);
            stream.SendNext(gun.transform.localScale);
            stream.SendNext(gun.transform.rotation);
        }
        else
        {
            curPos = (Vector3) stream.ReceiveNext();
            hp.value = (float) stream.ReceiveNext();
            hp.maxValue = (float) stream.ReceiveNext();
            angle = (float) stream.ReceiveNext();
            MousePosition = (Vector3) stream.ReceiveNext();
            moveDirection = (Vector2) stream.ReceiveNext();
            transform.localScale = (Vector3) stream.ReceiveNext();
            canvasLocalScaleX = (float)stream.ReceiveNext();
            canvasRect.localScale = (Vector3) stream.ReceiveNext();
            canMove = (bool) stream.ReceiveNext();
            gun.transform.localScale = (Vector3) stream.ReceiveNext();
            gun.transform.rotation = (Quaternion) stream.ReceiveNext();
        }
    }
    
    [PunRPC]
    public void canvasOn()
    {
        photonviewCanvas.transform.localScale = savedCanvasScale;
    }
    [PunRPC]
    public void canvasOff()
    {
        photonviewCanvas.transform.localScale = Vector3.zero;
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
        try
        {
        anim.Play(animName);
        }
        catch (Exception e)
        { }
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
        print( temMgr.GetWeapon(i).weaponIndex+" "+ temMgr.GetWeapon(i).spr.name);
        gun.GetComponent<SpriteRenderer>().sprite =  temMgr.GetWeapon(i).spr;
        //gun.GetComponent<SpriteRenderer>().sprite = gunSprites[i - 1];
    }

    [PunRPC]
    void ReLoad(float reloadTime) //재장전
    {
        if(PhotonNetwork.OfflineMode)
            gunAnimRPC(currentWeapon.weaponIndex.ToString(),true);
        else
            pv.RPC("gunAnimRPC",RpcTarget.All,currentWeapon.weaponIndex.ToString(),true);
        Vector3 a = gun.transform.eulerAngles;
        a.z += 181;
        isReLoading = true;
        canReload = true;
        sound.PlayGun(shotSoundIndex,true,volume,true);
        gun.transform.DORotate(a, reloadTime/2).SetEase(reLoadEase1).OnComplete(()=> {
            Vector3 b = gun.transform.eulerAngles;
            b.z += 181;
            gun.transform.DORotate(b, reloadTime/2).SetEase(reLoadEase2).OnComplete(() =>
            {
                if (canReload)
                {
                    isReLoading = false;
                    leftBullet.Reload(playerItem.selectedIndex);   
                }
            });
        });
                
    }
    
    [PunRPC]
    void gunAnimRPC(string index, bool isIdle)
    {
        if(isIdle) 
            gunAnim.SetTrigger("Idle"+index);
        else
            gunAnim.SetTrigger(index);
    }
    
    #endregion
    
    #region 충돌함수
    private void OnTriggerEnter2D(Collider2D other) //충돌함수
    {
        if (pv.IsMine)
        {
            if (other.CompareTag("Teleport")) //순간이동
            {
                StopAllCoroutines();
                
                   
                        canMove = true;
                
                        if (PhotonNetwork.OfflineMode)
                        {
                            SetAnimRPC("Idle");
                        }
                        else
                        {
                            pv.RPC("SetAnimRPC",RpcTarget.All,"Idle");
                        }
                    

                    FindObjectOfType<Fade>().Teleport(this,GameObject.Find(other.name + "_T").transform.position);
            }

            if (other.CompareTag("Explosion")) //폭탄
            {
                DelayDestroy enemy = other.GetComponent<DelayDestroy>();
                Hit(enemy.playerDmg, enemy.myName,enemy.nuckBackDistance,enemy.transform.position);
                enemy.playerDmg = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (pv.IsMine)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy=other.gameObject.GetComponent<Enemy>();
                if (enemy.time >= enemy.damageDelay)
                {
                    enemy.time = 0;
                    if(enemy.name.Contains("(")) 
                        Hit(enemy.CollsionDamage, enemy.name.Substring(0, enemy.name.IndexOf("(")),enemy.nuckBackDistance,enemy.transform.position);
                    else
                        Hit(enemy.CollsionDamage, enemy.name,enemy.nuckBackDistance,enemy.transform.position);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
        {
            if (pv.IsMine)
            {
                if (other.CompareTag("HouseTile"))
                    isHouse = true;
                if (other.CompareTag("Aspalt"))
                    isAspalt = true;
                if (other.CompareTag("Pan1"))
                    isPan1 = true;
                if (other.CompareTag("Pan2"))
                    isPan2 = true;

                if (other.CompareTag("Fire"))
                {
                    if (fireTime <=0)
                    {
                        Hit(other.GetComponent<DelayDestroy>().damage, "불꽃", 0, Vector3.zero);
                        fireTime = fireDamageTick;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("HouseTile"))
                isHouse = false;
            if (other.CompareTag("Aspalt"))
                isAspalt = false;
            if (other.CompareTag("Pan1"))
                isPan1 = false;
            if (other.CompareTag("Pan2"))
                isPan2 = false;
        }
        #endregion
        public void PassiveOn(int itemIndex)
        { 
            passive.PassiveOn(itemIndex);
        }
        
        public void PassiveOff(int itemIndex)
        {
            passive.PassiveOff(itemIndex);
        }
}
