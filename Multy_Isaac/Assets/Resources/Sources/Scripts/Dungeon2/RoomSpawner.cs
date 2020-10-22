using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    //1 --> need bottom door
    //2 --> need top door
    //3 --> need left door
    //4 --> need right door
    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn",0.1f);
    }

    void Spawn()
    {
        if (spawned == false)//생성되지 않았으면
        {
            print(openingDirection);
            if (openingDirection == 1) {//아래쪽에 문
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position,
                    templates.bottomRooms[rand].transform.rotation);
            }else if (openingDirection == 2){//위쪽에 문
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position,
                    templates.topRooms[rand].transform.rotation);
            }else if (openingDirection == 3) {//왼쪽에 문
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position,
                    templates.leftRooms[rand].transform.rotation);
            } else if (openingDirection == 4) {//오른쪽에 문
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position,
                    templates.rightRooms[rand].transform.rotation);
            }

            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            print("방겹침, 파괴!");
            Destroy(gameObject); //방이 겹치면 자신을 파괴   
        }
    }
}
