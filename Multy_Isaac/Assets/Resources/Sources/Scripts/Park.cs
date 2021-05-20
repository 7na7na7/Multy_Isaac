using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Park : MonoBehaviour
{
    public GameObject[] props;
   public GameObject[] wallProps;
   public Transform min;
   public Transform max;

   public int propCount;
   public int wallPropCount;

   private List<Vector2> cantList=new List<Vector2>();

   private void Start()
   {
       Invoke("set",.3f);
   }

   private void set()
   {
       if (PhotonNetwork.OfflineMode)
       {
           for (int j = 0; j < propCount; j++)
           {
               Instantiate(props[Random.Range(0,props.Length)], new Vector3(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y)), quaternion.identity);
           }
           for (int i = 0; i < wallPropCount; i++)
           {
               Vector2 a=Vector2.zero;
               bool canGo = false;
               while (!canGo)
               {
                   bool canGo2 = false;
                   a = new Vector3
                       (Mathf.FloorToInt(Random.Range(min.position.x, max.position.x)), Mathf.FloorToInt(Random.Range(min.position.y, max.position.y)));
                   for (int j = 0; j < cantList.Count; j++)
                   {
                       if (a == cantList[j])
                           canGo2 = true;
                   }

                   if (!canGo2)
                       canGo = true;
               }
               cantList.Add(a);
           
              
                   Instantiate(wallProps[Random.Range(0,wallProps.Length)], a, quaternion.identity);
           }
       }
       else
       {
           if (PhotonNetwork.IsMasterClient)
           {
               for (int j = 0; j < propCount; j++)
               {
                   PhotonNetwork.InstantiateRoomObject(props[Random.Range(0,props.Length)].name, new Vector3(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y)), quaternion.identity);
               }
               for (int i = 0; i < wallPropCount; i++)
               {
                   Vector2 a=Vector2.zero;
                   bool canGo = false;
                   while (!canGo)
                   {
                       bool canGo2 = false;
                       a = new Vector3
                           (Mathf.FloorToInt(Random.Range(min.position.x, max.position.x)), Mathf.FloorToInt(Random.Range(min.position.y, max.position.y)));
                       for (int j = 0; j < cantList.Count; j++)
                       {
                           if (a == cantList[j])
                               canGo2 = true;
                       }

                       if (!canGo2)
                           canGo = true;
                   }
                   cantList.Add(a);
                   
                   PhotonNetwork.InstantiateRoomObject(wallProps[Random.Range(0,wallProps.Length)].name, a, quaternion.identity);  }
           }
       }
   }
}
