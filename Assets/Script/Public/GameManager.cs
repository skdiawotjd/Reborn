using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//[Serializable]
public class GameManager : MonoBehaviour
{
    private bool _isNewGenerate;    // 게임을 처음 기동했는지
    [SerializeField]
    private float _playTime;        // 하루에 지나간 시간
    [SerializeField]
    private float _totalPlayTime;   // 하루의 총 시간
    
    private bool _isdayStart;       // 하루가 시작되었는지
    [SerializeField]
    private int _round;             // 몇회차
    [SerializeField]
    private int _days;              // 며칠이 지났는지

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

    // 게임 최초 시작 시
    private UnityEvent GenerateGameEvent;
    // 게임이 완료된 후 시작 시
    private UnityEvent GameStartEvent;
    // 하루가 시작 시
    private UnityEvent DayStart;
    // 하루가 끝날 시
    private UnityEvent DayEnd;
    // 씬이 변경될 시
    private UnityEvent SceneMoveEvent;
    // 로드 시
    private UnityEvent LoadEvent;
    private bool Pause;


    public static GameManager instance = null;

    // PopUpUIManager - SettingUIGame
    public void AddGenerateGameEvent(UnityAction AddEvent)
    {
        GenerateGameEvent.AddListener(AddEvent);
    }
    // SPUM_SpriteList - InitializeSprite, Character - InitializeCharacter, ConversationManager - InitializeNpcNumberChatType
    public void AddGameStartEvent(UnityAction AddEvent)
    {
        GameStartEvent.AddListener(AddEvent);
    }
    // PlayerController - StartPlayerController, ///Character - LoadCharacter, UIManager - StartUI
    // PopUpUIManager - AllClosePopUpUI, UIInventoryManager - InitializeInventory, QuestManager - GiveQuest
    public void AddDayStart(UnityAction AddEvent)
    {
        DayStart.AddListener(AddEvent);
    }
    // PlayerController - EndPlayerController, Character - EndCharacter, UIManager - EndUI
    // PopUpUIManager - AllClosePopUpUI, ConversationManager - DayEnd
    public void AddDayEnd(UnityAction AddEvent)
    {
        DayEnd.AddListener(AddEvent);
    }
    // SceneLoadManager - MapSetting, SoundManager - SetBackgroundSource, PopUpUIManager - SceneMovePopUI
    public void AddSceneMoveEvent(UnityAction AddEvent)
    {
        SceneMoveEvent.AddListener(AddEvent);
    }
    // Character - LoadCharacter, PopUpUIManager - ActiveUIManagerList, UIMainManager - LoadUI
    // UIInventoryManager - LoadInventory, UISettingManager - SetActivePanel
    public void AddLoadEvent(UnityAction AddEvent)
    {
        LoadEvent.AddListener(AddEvent);
    }

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

        GenerateGameEvent = new UnityEvent();
        GameStartEvent = new UnityEvent();
        DayStart = new UnityEvent();
        DayEnd = new UnityEvent();
        SceneMoveEvent = new UnityEvent();
        LoadEvent = new UnityEvent();

        SaveDataDirectory = new DirectoryInfo(Application.dataPath + "/Resources/SaveData/");
        SceneManager.sceneLoaded += LoadedsceneEvent;

        LoadEvent.AddListener(NextCycle);
    }

    
    void Start()
    {
        //MainCanvas = Instantiate(Resources.Load("Public/Main Canvas")) as GameObject;
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

            GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
            // 1. 게임 초기화
            InitializeGame();

            GenerateGameEvent.Invoke();

            // 2. 시작 씬 이동
            SceneManager.LoadScene("JustChat");
        }
        else
        {
            GameStartEvent.Invoke();
            SaveData();
            SceneManager.LoadScene("JustChat");
        }
    }


    private void InitializeGame()
    {
        /*/// Canvas 세팅
        GameObject CanvasObject = Instantiate(Resources.Load("Public/Main Canvas")) as GameObject;
        CanvasObject.name = "Main Canvas";
        DontDestroyOnLoad(CanvasObject);*/


        // Camera 세팅
        GameObject MainCamera = Instantiate(Resources.Load("Public/Main Camera")) as GameObject;
        MainCamera.name = "Main Camera";

        /*// Sound 세팅
        GameObject SoundManager = Instantiate(Resources.Load("Public/SoundManager")) as GameObject;
        SoundManager.name = "SoundManager";*/

        // Character 세팅
        if(!Character.instance)
        {
            GameObject PlayerCharacter = Instantiate(Resources.Load("Public/PlayerCharacter")) as GameObject;
            PlayerCharacter.name = "PlayerCharacter";
        }
        
        
        // QuestManager 세팅
        GameObject QuestManager = Instantiate(Resources.Load("Public/QuestManager")) as GameObject;
        QuestManager.name = "QuestManager";

        /*// SceneLoadManager 세팅
        GameObject SceneLoadManager = Instantiate(Resources.Load("Public/SceneLoadManager")) as GameObject;
        SceneLoadManager.name = "SceneLoadManager";*/
    }

    // 각 분기까지 3일이 걸림, 9일째는 회차 완료라 가정
    private void InitializeDay()
    {
        DayEnd.Invoke();

        // 1. 분기 판단
        if (Days % 3 == 0)
        {
            // 1.1 특정 분기 진입
            Debug.Log("분기 진입");
            Quarter();
        }
        else
        {
            // 1.2 특정 분기 없음
            NewDay();
        }
    }

    // 분기 판단
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
                    // 사망
                    Debug.Log("사망사망사망사망사망사망사망사망사망사망");
                }
                else
                {
                    // 생존
                    Debug.Log("생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    GameStart();
                }
            }
            else if (Days == 6)
            {
                if (Character.instance.MyJob >= Job.Baron)
                {
                    // 사망
                    Debug.Log("사망사망사망사망사망사망사망사망사망사망");
                }
                else
                {
                    // 생존
                    Debug.Log("생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    GameStart();
                }
            }
            else if (Days == 9)
            {
                if (Character.instance.MyJob >= Job.Knight)
                {
                    // 사망
                    Debug.Log("끝인데 사망사망사망사망사망사망사망사망사망사망");
                }
                else
                {
                    // 생존
                    Debug.Log("끝인데 생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
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
                    // 사망
                    Debug.Log("사망사망사망사망사망사망사망사망사망사망");
                }
                else
                {
                    // 생존
                    Debug.Log("생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    GameStart();
                }
            }
            else if (Days == 6)
            {
                if (Character.instance.MyJob >= Job.GrandDuke)
                {
                    // 사망
                    Debug.Log("사망사망사망사망사망사망사망사망사망사망");
                }
                else if (Character.instance.MyJob >= Job.Baron)
                {
                    // 강등
                    Debug.Log("강등강등강등강등강등강등강등강등강등강등");
                    Debug.Log("강등 기능");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    GameStart();
                }
                else
                {
                    // 생존
                    Debug.Log("생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    GameStart();
                }
            }
            else if (Days == 9)
            {
                if (Character.instance.MyJob >= Job.Baron)
                {
                    // 사망
                    Debug.Log("끝인데 사망사망사망사망사망사망사망사망사망사망");
                }
                else if (Character.instance.MyJob >= Job.Knight)
                {
                    // 강등
                    Debug.Log("끝인데 강등강등강등강등강등강등강등강등강등강등");
                    Debug.Log("강등 기능");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    GameStart();
                }
                else
                {
                    // 생존
                    Debug.Log("끝인데 생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    _days = 0;
                    _round++;
                    // 새로운 싸이클 혹은 최종으로 넘어가기 전에 justchat에 사용할 ConversationManager.NpcNumberChatType을 셋 하고
                    GameStart();
                }
            }
        }

        Character.instance.SetCharacterStat(CharacterStatType.TodoProgress, -Character.instance.TodoProgress);
    }

    // 하루 시작 과정
    private void NewDay()
    {
        // 값 초기화
        _playTime = 0f;
        //Debug.Log(Days + "일 끝");
        //DayEnd.Invoke();
        _days += 1;

        // 새로운 하루 시작
        Invoke("NextCycle", 0.01f);
    }

    private void NextCycle()
    {
        switch (Character.instance.MyRound)
        {
            case 1:
                _isdayStart = true;
                //Debug.Log(Days + "일 시작");
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
        if(Character.instance)
        {
            Character.instance.SetCharacterPosition();
        }
        

        SceneMoveEvent.Invoke();

        

        if (!IsDayStart && SceneName == "Home")
        {
            NewDay();
        }
    }


    public void SaveData()
    {
        SetSaveDataCount();
        SaveSaveList();

        Character.instance.SaveCharacter();
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
        if (Character.instance == null)
        {
            InitializeGame();
            Character.instance.LoadCharacter();
            GenerateGameEvent.Invoke();
        }
        JsonUtility.FromJsonOverwrite(json2, Character.instance);

        LoadEvent.Invoke();

        switch (Character.instance.MyMapNumber.Substring(2,2))
        {
            case "00": // 집
                SceneManager.LoadScene("Home");
                break;
            case "01": // 타운
                SceneManager.LoadScene("Town");
                break;
            case "02": // DDR
            case "03": // 타이밍
            case "04": // 퀴즈
            case "05": // 오브젝트 배치
            case "08": // 물건전달
                SceneManager.LoadScene("MiniGame");
                break;
            case "06": // 미니 RPG
                
                break;
            case "07": // 대화
                SceneManager.LoadScene("JustChat");
                break;   
        }

        Invoke("NextCycle", 0.1f);
    }

    private void SaveSaveList()
    {
        string path = SaveDataDirectory.ToString() + "SaveDataList.csv";
        string str = "\n" + _saveDataCount.ToString() + "," + ((int)Character.instance.MyJob).ToString() + "," + Character.instance.MyAge.ToString() + "," + PlayTime + "," + Character.instance.MyMapNumber.ToString();
        //Debug.Log("SaveDataNumber : " + _saveDataCount.ToString() + " Job : " + ((int)Character.instance.MyJob).ToString() + " Age : " + Character.instance.MyAge.ToString() + " PlayTime : " + PlayTime + " MapNumber : " + Character.instance.MyMapNumber.ToString());

        File.AppendAllText(path, str);
    }

    public string LoadSaveList(int Order)
    {
        string path = SaveDataDirectory.ToString() + "SaveDataList.csv";

        foreach (string line in File.ReadLines(path))
        {
            if(line[0] - '0' == Order)
            {
                return line;
            }
        }

        return "0,0,10,0,0000";
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
