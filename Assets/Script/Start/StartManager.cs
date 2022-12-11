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

    private AudioListener StartAudioListener;
    private PopUpUIManager PopUpUIManager;

    void Awake()
    {
        NewGame.onClick.AddListener(ClickNewGame);
        LoadGame.onClick.AddListener(ClickLoadGame);
    }

    void Start()
    {
        StartAudioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        PopUpUIManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetComponent<PopUpUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickNewGame()
    {
        // 저장한 데이터가 없다면
        if(true)
        {
            Debug.Log("NewGame - 1게임을 마친 정보가 없음");
            // 1. Start씬의 카메라 false
            StartAudioListener.enabled = false;

            GameManager.instance.GameStart();
        }
        // 저장한 데이터가 없다면
        else 
        {
            Debug.Log("NewGame - 1게임을 마친 정보가 있음");
            // 캐릭터 선택 화면 보여주기
        }
    }

    public void ClickLoadGame()
    {
        // 저장된 데이터 개수 체크
        GameManager.instance.SetSaveDataCount();

        // 저장한 내역이 있다면
        if (GameManager.instance.SaveDataCount != 0)
        {
            Debug.Log("LoadGame - 저장한 데이터가 있음");
            // 1. Start씬의 카메라 false
            //StartAudioListener.enabled = false;

            PopUpUIManager.VisibleSpecificUI(UIPopUpOrder.SettingPanel);
        }
        else
        {
            Debug.Log("LoadGame - 저장한 데이터가 없음");
        }
    }
}
