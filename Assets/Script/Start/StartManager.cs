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
        // ������ �����Ͱ� ���ٸ�
        if(true)
        {
            Debug.Log("NewGame - 1������ ��ģ ������ ����");
            // 1. Start���� ī�޶� false
            StartAudioListener.enabled = false;

            GameManager.instance.GameStart();
        }
        // ������ �����Ͱ� ���ٸ�
        else 
        {
            Debug.Log("NewGame - 1������ ��ģ ������ ����");
            // ĳ���� ���� ȭ�� �����ֱ�
        }
    }

    public void ClickLoadGame()
    {
        // ����� ������ ���� üũ
        GameManager.instance.SetSaveDataCount();

        // ������ ������ �ִٸ�
        if (GameManager.instance.SaveDataCount != 0)
        {
            Debug.Log("LoadGame - ������ �����Ͱ� ����");
            // 1. Start���� ī�޶� false
            //StartAudioListener.enabled = false;

            PopUpUIManager.VisibleSpecificUI(UIPopUpOrder.SettingPanel);
        }
        else
        {
            Debug.Log("LoadGame - ������ �����Ͱ� ����");
        }
    }
}
