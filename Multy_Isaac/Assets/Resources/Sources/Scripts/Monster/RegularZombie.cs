using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.Tweening;
using Photon.Pun;
using Random = UnityEngine.Random;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class RegularZombie : MonoBehaviour
{
    private IEnumerator corr;
    private PhotonView pv;
    public float AttackTime;
    public float minIdleTime=0.5f;
    public float maxIdleTIme = 2f;
    private bool isFinding = false;
    public float minMove;
    public float maxMove;
    public float speed;
    private float localX;
    private Rigidbody2D rigid;
    private Vector3 startingPosition;
    Animator anim;
    
    private Vector2Int startPos, targetPos; 
    private Vector2Int bottomLeft=new Vector2Int(-20,-10); 
    private Vector2Int topRight=new Vector2Int(20,10); 
    public List<Node> FinalNodeList; 
    public bool allowDiagonal, dontCrossCorner;
    
    int sizeX, sizeY; 
    Node[,] NodeArray; 
    Node StartNode, TargetNode, CurNode; 
    List<Node> OpenList, ClosedList;

    
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        anim = GetComponent<Animator>();
        corr = slimeCor();
        localX = transform.localScale.x*-1;

        if (PhotonNetwork.OfflineMode)
        {
         StartCoroutine(corr);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(velocitySync());
            }
                
        }
    }

    IEnumerator velocitySync()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            pv.RPC("veloSyncRPC", RpcTarget.All,rigid.position);
        }
    }

    [PunRPC]
    void veloSyncRPC(Vector2 p)
    {
        rigid.position = p;
    }
    IEnumerator slimeCor()
    {
        while (true)
        {
            rigid.velocity = Vector2.zero;
            Vector2 roamPos = GetRoamingPosition();
            //print(roamPos+" + "+Vector2.Distance(transform.position, roamPos));
            if (Vector2.Distance(transform.position, roamPos) < 1f)
            {
                rigid.velocity = Vector2.zero;
                
                if(PhotonNetwork.OfflineMode)
                    animRPC("RegularZombie_Idle");
                else
                    pv.RPC("animRPC",RpcTarget.All,"RegularZombie_Idle");
                
                yield return new WaitForSeconds(Random.Range(minIdleTime,maxIdleTIme));
            }
            else
            {
                if(PhotonNetwork.OfflineMode)
                    animRPC("RegularZombie_Walk");
                else
                    pv.RPC("animRPC",RpcTarget.All,"RegularZombie_Walk");
                
                Vector2 dir = (roamPos- (Vector2)transform.position).normalized;
                rigid.velocity = dir * speed;
            
                float reachedPositionDistance = 0.2f;
                yield return new WaitUntil(() =>Vector2.Distance(transform.position,roamPos)<reachedPositionDistance);      
            }

//            isFinding = true;
//            startPos = new Vector2Int((int)transform.position.x,(int)transform.position.y);
//            Vector2 roamPos = GetRoamingPosition();
//            targetPos=new Vector2Int((int)roamPos.x,(int)roamPos.y);
//            PathFinding();
//            yield return new WaitUntil(() => isFinding ==false);   
        }
    }
    
   
   private Vector2 GetRoamingPosition()
   {
       float randomMove = Random.Range(minMove, maxMove);
       return startingPosition + UtilsClass.GetRandomDir() * randomMove;
   }


   private void OnCollisionStay2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Wall"))
       {
           if(PhotonNetwork.OfflineMode)
           {
               StopCoroutine(corr);
               StartCoroutine(corr);
           }
           else
           {
               if(PhotonNetwork.IsMasterClient)
               {
                   StopCoroutine(corr);
                   StartCoroutine(corr);
               }
           }
       }
   }
   private void OnCollisionEnter2D(Collision2D other)
   {
       if (other.gameObject.CompareTag("Wall"))
       {
           if(PhotonNetwork.OfflineMode)
           {
               StopCoroutine(corr);
               StartCoroutine(corr);
           }
           else
           {
               if(PhotonNetwork.IsMasterClient)
               {
                   StopCoroutine(corr);
                   StartCoroutine(corr);
               }
           }
       }
   }
   private void OnTriggerEnter2D(Collider2D other)
   {
       if (other.CompareTag("Player"))
       {
           if(PhotonNetwork.OfflineMode)
           {
               StopCoroutine(corr);
               rigid.velocity=Vector2.zero;
               StartCoroutine(Attack(other.transform.position.x));
           }
           else
           {
               if (PhotonNetwork.IsMasterClient)
               {
                   StopCoroutine(corr);
                   rigid.velocity = Vector2.zero;
                   StartCoroutine(Attack(other.transform.position.x));
               }
           }
       }
   }

   IEnumerator Attack(float x)
   {
       if (x > transform.position.x) //오른쪽에있으면
       {
           if(PhotonNetwork.OfflineMode)
               localScaleRPC(localX*-1);
           else
               pv.RPC("localScaleRPC",RpcTarget.All,localX*-1);
       }
       else
       {
           if(PhotonNetwork.OfflineMode)
               localScaleRPC(localX);
           else
               pv.RPC("localScaleRPC",RpcTarget.All,localX);
       }
       
       if(PhotonNetwork.OfflineMode)
           animRPC("RegularZombie_Attack");
       else
           pv.RPC("animRPC",RpcTarget.All,"RegularZombie_Attack");
       
       yield return new WaitForSeconds(AttackTime);
       StartCoroutine(corr);
   }

   #region 길찾기
    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x;
        sizeY = topRight.y - bottomLeft.y;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }
        

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        
        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];
            //이동비용이 가장 싼 것을 현재 노드(CurNode)로 함

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            
            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                //for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                StartCoroutine(Move(FinalNodeList.Count));
                return;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }
   void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨(그러니까 내 위쪽과 오른쪽에 벽이 있으면 오른쪽 위로 대각선 이동 불가라는 뜻)
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            
            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나(그래야 더 빠른길이니까) 또는 열린리스트에 이웃노드가 없다면(새로운 이웃노드추가해야지) G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
   void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }
   IEnumerator Move(int count)
    {
        for (int i = 1; i < count; i++)
        {
            Vector2 v2 = new Vector2(FinalNodeList[i].x, FinalNodeList[i].y);
            rigid.velocity=Vector2.zero;
            rigid.DOMove(v2, 1f);
            yield return new WaitUntil(() => Mathf.Abs(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y).SqrMagnitude() -transform.position.sqrMagnitude)<0.1f 
                                             && Mathf.Abs(transform.position.sqrMagnitude-new Vector2(FinalNodeList[i].x, FinalNodeList[i].y).SqrMagnitude())<0.1f);
        }
        isFinding = false;
    }
   #endregion


   [PunRPC]
   void animRPC(string animName)
   {
       try
       {
           anim.Play(animName);
       }
       catch (Exception e)
       { }
   }

   [PunRPC]
   void localScaleRPC(float x)
   {
       transform.localScale=new Vector3(x,transform.localScale.y,transform.localScale.z);
   }
}
