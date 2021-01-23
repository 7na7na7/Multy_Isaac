using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public BoxCollider2D bound;

    public float speed = 2f;
    public GameObject target; //카메라가 따라갈 대상

    private Vector3 targetPosition; //대상의 현재 값
    private Vector3 minBound, maxBound; //박스 콜라이더 영역의 최소/최대 xyz값을 지님
    private float halfWidth, halfHeight; //카메라의 반너비, 반높이 값을 지닐 변수

    private void Start()
    {
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비 공식
    }
  
    void Update()
    {
        if (target.gameObject != null)
        {
            if (speed == 0)
            {
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
