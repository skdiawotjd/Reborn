using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> StartButtonList;

    //private AudioListener StartAudioListener;
    private PopUpUIManager PopUpUIManager;

    private CharacterSelectManager CharacterSelectManager;

    private bool SetActiveLoad;

    void Awake()
    {
        SetActiveLoad = false;

        StartButtonList[(int)StartButtonOrder.New].onClick.AddListener(ClickNewGame);
        StartButtonList[(int)StartButtonOrder.Load].onClick.AddListener(ClickLoadGame);
        StartButtonList[(int)StartButtonOrder.Exit].onClick.AddListener(ClickExitGame);
    }

    void Start()
    {
        //StartAudioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        PopUpUIManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetComponent<PopUpUIManager>();
        CharacterSelectManager = GameObject.Find("Main Canvas").transform.GetChild(1).GetComponent<CharacterSelectManager>();

        CanLoad();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CharacterSelectManager.SetActiveCharacterSelectPanel();
            if (SetActiveLoad && SetActiveLoad == PopUpUIManager.UIManagerList[(int)UIPopUpOrder.SettingPanel].gameObject.activeSelf)
            {
                ClickLoadGame();
            }
        }
    }

    public void SetActivePopUpUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            PopUpUIManager.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void ClickNewGame()
    {
        //PopUpUIManager.SetActiveLoad();
        Job StartJob = GameManager.instance.CanPlayJob();
        //Debug.Log("������ �� �ִ� ����" + StartJob);
        switch (StartJob)
        {
            case Job.Slayer :
                //StartAudioListener.enabled = false;
                GameManager.instance.GameStart();
                break;
            default:
                CharacterSelectManager.SetCharacterSelectButton(StartJob);
                CharacterSelectManager.SetActiveCharacterSelectPanel();
                break;
        }

    }

    private void CanLoad()
    {
        // ����� ������ ���� üũ
        GameManager.instance.SetSaveDataCount();

        // ������ ������ �ִٸ�
        if (GameManager.instance.SaveDataCount != 0)
        {
            //Debug.Log("LoadGame - ������ �����Ͱ� ����");
            // 1. Start���� ī�޶� false
            //StartAudioListener.enabled = false;

            StartButtonList[(int)StartButtonOrder.Load].interactable = true;
        }
        else
        {
            //Debug.Log("LoadGame - ������ �����Ͱ� ����");
        }
    }

    private void ClickLoadGame()
    {
        PopUpUIManager.SetActiveLoadPanel();
        SetActiveLoad = PopUpUIManager.UIManagerList[(int)UIPopUpOrder.SettingPanel].gameObject.activeSelf;
    }

    private void ClickExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
