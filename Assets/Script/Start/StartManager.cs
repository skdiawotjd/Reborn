using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private Button NewGame;
    [SerializeField]
    private Button LoadGame;

    
    void Start()
    {
        NewGame.onClick.AddListener(GameStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        // 1. Start¾ÀÀÇ Ä«¸Þ¶ó false
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;

        GameManager.instance.GameStart();
    }
}
