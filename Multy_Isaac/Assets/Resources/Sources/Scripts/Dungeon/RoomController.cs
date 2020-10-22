using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfoClass
{
    public string name;
    public int x;
    public int y;
    
}
public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    private string currentWorldName = "Basement";

    private RoomInfoClass currentLoadRoomData;
    
    Queue<RoomInfoClass> loadRoomQueue=new Queue<RoomInfoClass>();
    
    public List<Room> loadedRooms=new List<Room>();

    private bool isLoadingRoom = false;
    private void Awake()
    {
        instance = this;
    }

    public bool DoesRoomExist(int x, int y) //방이 해당 그리드에 있는지 검사
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y)!= null; 
    }
}
