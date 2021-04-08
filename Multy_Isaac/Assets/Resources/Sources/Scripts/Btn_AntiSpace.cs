using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Btn_AntiSpace : MonoBehaviour
{
    public static Btn_AntiSpace instance;
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
    }
    void OnEnable()
    {  
        // 델리게이트 체인 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Button[] btns = FindObjectsOfType<Button>();
        foreach (Button b in btns)
        {
            var navigation = b.navigation;
            navigation.mode = Navigation.Mode.None;
            b.navigation = navigation;
        }
    }
    void OnDisable()
    {
        // 델리게이트 체인 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
