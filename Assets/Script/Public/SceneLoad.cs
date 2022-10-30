using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= LoadedsceneEvent;
    }


    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + "으로 이동");
    }
}
