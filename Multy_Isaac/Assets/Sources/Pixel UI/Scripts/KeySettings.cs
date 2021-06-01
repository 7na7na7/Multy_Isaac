using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Keys { UP, DOWN, LEFT, RIGHT, COMBINE, DROP, USE, MAP,RELOAD }

public static class SetKey
{
    public static Dictionary<Keys,KeyCode> keys=
        new Dictionary<Keys, KeyCode>();
}
public class KeySettings : MonoBehaviour
{
    public static KeySettings instance;
    private string[] keys = new[] {"UP", "DOWN", "LEFT", "RIGHT", "COMBINE", "DROP", "USE", "MAP","RELOAD"};
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetKey.keys.Add(Keys.UP, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("UP","W")));
            SetKey.keys.Add(Keys.DOWN,(KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("DOWN","S")));
            SetKey.keys.Add(Keys.LEFT, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("LEFT","A")));
            SetKey.keys.Add(Keys.RIGHT, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("RIGHT","D")));
            SetKey.keys.Add(Keys.COMBINE, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("COMBINE","Space")));
            SetKey.keys.Add(Keys.DROP, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("DROP","Q")));
            SetKey.keys.Add(Keys.USE, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("USE","E")));
            SetKey.keys.Add(Keys.MAP, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("MAP","Tab")));   
            SetKey.keys.Add(Keys.RELOAD, (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("RELOAD","R")));   
         
        
            for (int i = 0; i < SetKey.keys.Count; i++)
            {
                PlayerPrefs.SetString(keys[i], SetKey.keys[(Keys) i].ToString());
            }
    }

    public string returnkey(int i)
    {
        return SetKey.keys[(Keys) i].ToString();
    }
    private void OnGUI()
    {
        Event keyEvent=Event.current; //현재 이벤트 가지고옴
        if (keyEvent.isKey&&key!=-1) //만약 이벤트가 키고 설정 가능하다면
        {
            SetKey.keys[(Keys)key] = keyEvent.keyCode; //0,1,2,3을 Keys형으로 변환해 각각 UP, DOWN, LEFT, RIGHT로 캐스팅
            PlayerPrefs.SetString(keys[key],SetKey.keys[(Keys)key].ToString());
           // keyTexts[key].text = keyEvent.keyCode.ToString(); //바꾼키 텍스트 표시
            key = -1; //첫 키만 적용되게 다시 -1로 바꿈
        }
    }

    private int key=-1;

    public void ChangeKey(int num)
    {
        key = num;
    }
}