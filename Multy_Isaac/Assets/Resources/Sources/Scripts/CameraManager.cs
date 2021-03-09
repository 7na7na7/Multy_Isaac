using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public bool isMinimap = false;
    public BoxCollider2D bound;

    public float speed = 2f;
    public GameObject target;
    public GameObject minimapHead;
    private Vector3 targetPosition; //대상의 현재 값
    private Vector3 minBound, maxBound; //박스 콜라이더 영역의 최소/최대 xyz값을 지님
    private float halfWidth, halfHeight; //카메라의 반너비, 반높이 값을 지닐 변수

    private void Start()
    {
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        if (isMinimap)
        {
            if (PhotonNetwork.OfflineMode)
            {
                GameObject t=Instantiate(minimapHead, transform.position, Quaternion.identity);
                target = t;
            }
            else
            {
                GameObject t=PhotonNetwork.Instantiate(minimapHead.name, transform.position, Quaternion.identity);
                target = t;
            }   
        }
    }
  
    void Update()
    {
        if (target.gameObject != null)
        {
            if (speed == 0)
            {
                halfHeight = GetComponent<Camera>().orthographicSize;
                halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비 공식
                float clampedX = Mathf.Clamp(target.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
                float clampedY = Mathf.Clamp(target.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
                transform.position=new Vector3(clampedX,clampedY,this.transform.position.z);
            }
        }
    }

    public void SetBound(Vector3 min, Vector3 max)
    {
        minBound =min; //minbound에 box콜라이더의 영역 최솟값 대입
        maxBound = max;
    }
}
