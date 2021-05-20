using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Pathfinding;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AddRoom_First : MonoBehaviour
{
    public Vector4 map;
    public GameObject[] objects;
    public int[] counts;
    private RoomTemplates templates;
    private Procedural proc;
    
    private void Awake()
  {
      if (PhotonNetwork.OfflineMode)
      {
          set();
      }
      else
      {
          if(PhotonNetwork.IsMasterClient)
              set();
      }
  }

  void set()
  {
      if (SceneManager.GetActiveScene().name != "Tutorial")
      {
          proc = FindObjectOfType<Procedural>();
          templates=GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
          for (int i = 0; i < objects.Length; i++)
          {
              for (int j = 0; j < counts[i]; j++)
              {
                  float delay=proc.getCount();
                  StartCoroutine(SetRoom(templates.firstDelay + delay, i));
              }
          }   
      }
  }
 
  public IEnumerator SetRoom(float del, int index)
  {
      yield return new WaitForSeconds(del);
      templates.rooms.Add(this.gameObject);
    SetRoomProps(index); //생성////////////////////////////////////////////////////////////
  }

  void SetRoomProps(int index)
  {
      Vector2 pos;
      while (true)
         {
             pos= new Vector2(Random.Range((int)map.x, (int)map.y+1)*18, Random.Range((int)map.w, (int)map.z+1)*10);
      
             RaycastHit2D[] hit = Physics2D.BoxCastAll(
                 pos + objects[index].GetComponent<RoomProps>().offset,
                 objects[index].GetComponent<RoomProps>().BoxSize, 0, Vector2.down, 0);

             bool canSpawn = true;
             foreach (RaycastHit2D ray in hit)
             {
                 if (ray.collider.CompareTag("Entry") || ray.collider.CompareTag("Aspalt") || ray.collider.CompareTag("Destroyer") || ray.collider.CompareTag("Wall") || ray.collider.CompareTag("SpawnPoint"))
                 {
                     canSpawn = false;
                 }
             }

             if (canSpawn)
             {
                 foreach (RaycastHit2D c in hit)
                 {
                     if (c.collider.CompareTag("Space"))
                     {
                         Destroy(c.collider.gameObject);   
                     }
                 }
                 break;
             }
         }
         if (PhotonNetwork.OfflineMode)
         {
             Instantiate(objects[index], pos, quaternion.identity);
         }
         else
         {
             PhotonNetwork.InstantiateRoomObject(objects	[index].name, pos,quaternion.identity);
         }
  }
}
