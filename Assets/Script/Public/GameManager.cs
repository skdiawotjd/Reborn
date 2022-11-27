using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//[Serializable]
public class GameManager : MonoBehaviour
{
    private bool _isNewGenerate;    // ������ ó�� �⵿�ߴ���
    [SerializeField]
    private float _playTime;        // �Ϸ翡 ������ �ð�
    [SerializeField]
    private float _totalPlayTime;   // �Ϸ��� �� �ð�
    [SerializeField]
    private bool _isdayStart;       // �Ϸ簡 ���۵Ǿ�����
    [SerializeField]
    private int _round;             // ��ȸ��
    [SerializeField]
    private int _days;              // ��ĥ�� ��������

    [SerializeField]
    private DirectoryInfo SaveDataDirectory;
    [SerializeField]
    private int _saveDataCount;

    public float PlayTime
    {
        get
        {
            return _playTime;
        }
    }
    public float TotalPlayTime
    {
        get
        {
            return _totalPlayTime;
        }
    }
    public bool IsDayStart
    {
        get
        {
            return _isdayStart;
        }
    }
    public bool IsNewGenerate
    {
        get
        {
            return _isNewGenerate;
        }
    }
    public int Round
    {
        get
        {
            return _round;
        }
    }
    public int Days
    {
        get
        {
            return _days;
        }
    }
    public string SceneName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }
    public int SaveDataCount
    {
        get
        {
            return _saveDataCount;
        }
    }

    public UnityEvent GameStartEvent;   // SPUM_SpriteList - InitializeSprite, Character - InitializeCharacter
    public UnityEvent DayStart;    // PlayerController - StartPlayerController, MainUIManager - StartUI, InventoryManager - InitializeInventory, QuestManager - GiveQuest
    public UnityEvent DayEnd;      // PlayerController - EndPlayerController, MainUIManager - EndUI, PopUpUIManager - AllClosePopUpUI, ConversationManager - DayEnd, Character - EndCharacter
    public UnityEvent SceneMove;        // PopUpUIManager - SceneMovePopUI, SceneManager - MapSetting
    public UnityEvent LoadEvent;      // PopUpUIManager - AllClosePopUpUI, UIMainManager - LoadUI, UIInventoryManager - LoadInventory
    private bool Pause;


    public static GameManager instance = null;


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

        _playTime = 0f;
        _totalPlayTime = 600f;
        _isdayStart = false;
        _isNewGenerate = true;
        _days = 0;
        Pause = false;

        SaveDataDirectory = new DirectoryInfo(Application.dataPath + "/Resources/SaveData/");

        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    
    void Start()
    {

    }
    
    void Update()
    {
        if (IsDayStart)
        {
            if (Mathf.Floor(_playTime) != TotalPlayTime && Character.instance.ActivePoint != 0)
            {
                if(!Pause)
                {
                    _playTime += Time.deltaTime;
                }
                //Debug.Log(Mathf.Floor(_playTime));
            }
            else
            {
                _isdayStart = false;
                InitializeDay();
            }
        }
    }

    public void GameStart()
    {
        if(IsNewGenerate)
        {
            _isNewGenerate = false;
            // 1. Start���� ī�޶� false
            GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
            // 2. ���� �ʱ�ȭ
            InitializeGame();
            // 3. ���� �� �̵�
            SceneManager.LoadScene("JustChat");

            /*if(_days == 0)
            {
                // 3. ���� �� �̵�
                SceneManager.LoadScene("JustChat");
            }*/
        }
        else
        {
            GameStartEvent.Invoke();
            SaveData();
            SceneManager.LoadScene("JustChat");

            /*if (_days == 0)
            {
                SceneManager.LoadScene("JustChat");
            }*/
        }
    }


    private void InitializeGame()
    {
        // Canvas ����
        GameObject CanvasObject = Instantiate(Resources.Load("Public/Main Canvas 2")) as GameObject;
        CanvasObject.name = "Main Canvas";
        DontDestroyOnLoad(CanvasObject);

        // Camera ����
        GameObject MainCamera = Instantiate(Resources.Load("Public/Main Camera")) as GameObject;
        MainCamera.name = "Main Camera";

        // Sound ����
        GameObject SoundManager = Instantiate(Resources.Load("Public/SoundManager")) as GameObject;
        SoundManager.name = "SoundManager";

        // Character ����
        GameObject PlayerCharacter = Instantiate(Resources.Load("Public/TemPlayerCharacter")) as GameObject;
        PlayerCharacter.name = "PlayerCharacter";
        
        // QuestManager ����
        GameObject QuestManager = Instantiate(Resources.Load("Public/QuestManager")) as GameObject;
        QuestManager.name = "QuestManager";

        // SceneLoadManager ����
        GameObject SceneLoadManager = Instantiate(Resources.Load("Public/SceneLoadManager")) as GameObject;
        SceneLoadManager.name = "SceneLoadManager";
    }

    // �� �б���� 3���� �ɸ�, 9��°�� ȸ�� �Ϸ�� ����
    private void InitializeDay()
    {
        DayEnd.Invoke();

        // 1. �б� �Ǵ�
        if (Days % 3 == 0)
        {
            // 1.1 Ư�� �б� ����
            Debug.Log("�б� ����");
            Quarter();
        }
        else
        {
            // 1.2 Ư�� �б� ����
            NewDay();
        }
    }

    // �б� �Ǵ�
    private void Quarter()
    {
        if (Character.instance.TodoProgress == 100)
        {
            if(Days == 0)
            {
                //QuestManager.instance.QuestGive();
                NewDay();
            }
            else if (Days == 3)
            {
                if (Character.instance.MyJob == Job.King)
                {
                    // ���
                    Debug.Log("������������������������������");
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    //Character.instance.CheckStack();
                    //QuestManager.instance.QuestGive();
                    NewDay();
                }
            }
            else if (Days == 6)
            {
                if (Character.instance.MyJob >= Job.Baron)
                {
                    // ���
                    Debug.Log("������������������������������");
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    //Character.instance.CheckStack();
                    //QuestManager.instance.QuestGive();
                    NewDay();
                }
            }
            else if (Days == 9)
            {
                if (Character.instance.MyJob >= Job.Knight)
                {
                    // ���
                    Debug.Log("���ε� ������������������������������");
                }
                else
                {
                    // ����
                    Debug.Log("���ε� ����������������������������������������");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    // ���ο� ����Ŭ Ȥ�� �������� �Ѿ�� ���� justchat�� ����� ConversationManager.NpcNumberChatType�� �� �ϰ�
                    GameStart();
                }
            }
        }
        else if (Character.instance.TodoProgress < 100)
        {
            if (Days == 3)
            {
                if (Character.instance.MyJob >= Job.GrandDuke)
                {
                    // ���
                    Debug.Log("������������������������������");
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    //Character.instance.CheckStack();
                    //QuestManager.instance.QuestGive();
                    NewDay();
                }
            }
            else if (Days == 6)
            {
                if (Character.instance.MyJob >= Job.GrandDuke)
                {
                    // ���
                    Debug.Log("������������������������������");
                }
                else if (Character.instance.MyJob >= Job.Baron)
                {
                    // ����
                    Debug.Log("�������������");
                    Character.instance.CheckStack();
                    //QuestManager.instance.QuestGive();
                    NewDay();
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    //Character.instance.CheckStack();
                    //QuestManager.instance.QuestGive();
                    NewDay();
                }
            }
            else if (Days == 9)
            {
                if (Character.instance.MyJob >= Job.Baron)
                {
                    // ���
                    Debug.Log("���ε� ������������������������������");
                }
                else if (Character.instance.MyJob >= Job.Knight)
                {
                    // ����
                    Debug.Log("���ε� �������������");
                    Character.instance.CheckStack();
                }
                else
                {
                    // ����
                    Debug.Log("���ε� ����������������������������������������");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    // ���ο� ����Ŭ Ȥ�� �������� �Ѿ�� ���� justchat�� ����� ConversationManager.NpcNumberChatType�� �� �ϰ�
                    GameStart();
                }
            }
        }

        Character.instance.SetCharacterStat(4, -Character.instance.TodoProgress);
    }

    // �Ϸ� ���� ����
    private void NewDay()
    {
        // �� �ʱ�ȭ
        _playTime = 0f;
        //Debug.Log(Days + "�� ��");
        //DayEnd.Invoke();
        _days += 1;

        // ���ο� �Ϸ� ����
        Invoke("NextCycle", 0.01f);
    }

    private void NextCycle()
    {
        switch(Character.instance.MyRound)
        {
            case 1:
                _isdayStart = true;
                //Debug.Log(Days + "�� ����");
                DayStart.Invoke();
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        SceneMove.Invoke();

        

        if (!IsDayStart && SceneName == "Home")
        {
            NewDay();
        }
    }


    public void SaveData()
    {
        SetSaveDataCount();
        
        string Json1 = JsonUtility.ToJson(Character.instance);
        string path1 = SaveDataDirectory.ToString() + "PlayerCharacter" + SaveDataCount.ToString() + ".Json";
        File.WriteAllText(path1, Json1);

        string Json2 = JsonUtility.ToJson(gameObject.GetComponent<GameManager>());
        string path2 = SaveDataDirectory.ToString() + "GameManager" + SaveDataCount.ToString() + ".Json";
        File.WriteAllText(path2, Json2);

        //AssetDatabase.Refresh();
    }

    public void LoadData(int Number)
    {
        string path1 = SaveDataDirectory.ToString() + "GameManager" + Number.ToString() + ".Json";
        string json1 = File.ReadAllText(path1);
        JsonUtility.FromJsonOverwrite(json1, gameObject.GetComponent<GameManager>());

        string path2 = SaveDataDirectory.ToString() + "PlayerCharacter" + Number.ToString() + ".Json";
        string json2 = File.ReadAllText(path2);
        JsonUtility.FromJsonOverwrite(json2, Character.instance);

        LoadEvent.Invoke();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void SetSaveDataCount()
    {
        _saveDataCount = 0;
        foreach (FileInfo File in SaveDataDirectory.GetFiles())
        {
            if (File.Extension == ".Json")
            {
                _saveDataCount++;
            }
        }
        _saveDataCount /= 2;
    }

    public void ActivateDay()
    {
        Pause = !Pause;
    }
}
