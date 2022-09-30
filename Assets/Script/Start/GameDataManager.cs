using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SceneMove()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }

    public void Save()
    {
        Debug.Log("¿˙¿Â!!!!!!!!!!!!!");
    }
}
