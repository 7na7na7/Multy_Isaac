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
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
