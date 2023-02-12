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
    
    private bool _isDayStart;       // 하루가 시작되었는지
    [SerializeField]
    private int _round;             // 몇회차
    [SerializeField]
    private int _days;              // 며칠이 지났는지

    [SerializeField]
    private DirectoryInfo SaveDataDirectory;
    [SerializeField]
    private int _saveDataCount;
    [SerializeField]
    public GameObject MainCanvas;

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
            return _isDayStart;
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
    public void RemoveSceneMoveEvent(UnityAction AddEvent)
    {
        SceneMoveEvent.RemoveListener(AddEvent);
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
        _isDayStart = false;
        _isNewGenerate = true;
        _days = -1;
        _round = 0;
        _saveDataCount = 0;
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
                _isDayStart = false;
                InitializeDay();
            }
        }
    }

    public void GameStart()
    {
        if (IsNewGenerate)
        {
            _isNewGenerate = false;
            /*_isDayStart = true;
            Pause = true;*/

            // 1. 게임 초기화
            InitializeGame();
            Character.instance.CharacterStatSetting();
            GenerateGameEvent.Invoke();
            GameStartEvent.Invoke();

            //GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
            // 2. 시작 씬 이동
            SceneManager.LoadScene("JustChat");
        }
        else
        {
            SaveData();
            GameStartEvent.Invoke();
            SceneManager.LoadScene("JustChat");
        }
    }
    public void GameStart(Job StartJob)
    {
        if (IsNewGenerate)
        {
            _isNewGenerate = false;

            //GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
            // 1. 게임 초기화
            InitializeGame();
            Character.instance.CharacterStatSetting(StartJob);
            GenerateGameEvent.Invoke();
            GameStartEvent.Invoke();

            // 2. 시작 씬 이동
            SceneManager.LoadScene("JustChat");
        }
        else
        {
            SaveData();
            GameStartEvent.Invoke();
            SceneManager.LoadScene("JustChat");
        }
    }

    private void InitializeGame()
    {
        // Character 세팅
        if(!Character.instance)
        {
            GameObject PlayerCharacter = Instantiate(Resources.Load("Public/PlayerCharacter")) as GameObject;
            PlayerCharacter.name = "PlayerCharacter";
        }
        
        // QuestManager 세팅
        GameObject QuestManager = Instantiate(Resources.Load("Public/QuestManager")) as GameObject;
        QuestManager.name = "QuestManager";
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
        switch (Days)
        {
            case 0:
                Debug.Log("3 Or 6일이 지남");
                Character.instance.CheckStack();
                Character.instance.SetCharacterStat(CharacterStatType.Reputation, -Character.instance.Reputation);

                GameStart();
                break;
            case 3:
            case 6:
                Debug.Log("3 Or 6일이 지남");
                if (Character.instance.Reputation >= 10 / Days)
                {
                    // 리본 모양 변경 기능
                }
                else
                {
                    // 내 직업에 맞는 경고 대사를 위한 NpcNumberChatType set
                }
                NewDay();
                break;
            case 9:
                Debug.Log("9일이 지남");
                if (Character.instance.Reputation == 100)
                {
                    Character.instance.CheckStack();
                    Character.instance.SetCharacterStat(CharacterStatType.Reputation, -Character.instance.Reputation);
                    _days = 0;
                    _round++;
                    GameStart();
                }
                else
                {
                    // 사망
                    Debug.Log("사망사망사망사망사망사망사망사망사망사망");
                }
                break;
        }
    }

    // 하루 시작 과정
    public void NewDay()
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
        /*switch (Round)
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
        }*/
        _isDayStart = true;
        Pause = false;
        //Debug.Log(Days + "일 시작");
        DayStart.Invoke();
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if(Character.instance)
        {
            Character.instance.SetCharacterPosition();
        }
        

        SceneMoveEvent.Invoke();


        if (!IsDayStart)
        {
            switch (SceneName)
            { 
                case "Home":
                    NewDay();
                    break;
                case "MiniGame":
                    break;
            }
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
            case "02": // JustChat
                SceneManager.LoadScene("JustChat");
                break;
            case "01": // 아지트
            case "03": // DDR
            case "04": // 타이밍
            case "05": // 퀴즈
            case "06": // 오브젝트 배치
            case "07": // 물건 전달
            case "08": // 탐헝
            case "09": // 모헝
            case "10": // 학문수련
            case "11": // 아카데미
            case "12": // 방치형
                SceneManager.LoadScene("MiniGame");
                break;
            case "13":
                SceneManager.LoadScene("Town");
                break;
        }

        Invoke("NextCycle", 0.1f);
    }

    private void SaveSaveList()
    {
        string path = SaveDataDirectory.ToString() + "SaveDataList.csv";
        string str = "\n" + _saveDataCount.ToString() + "," + 
            ((int)Character.instance.MyJob).ToString() + "," + 
            Character.instance.MyAge.ToString() + "," + 
            PlayTime + "," + 
            Character.instance.MyMapNumber.ToString() + "," + 
            Round.ToString();
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

    public Job CanPlayJob()
    {
        List<Dictionary<string, object>> SaveList;
        int highList = 0;
        
        SaveList = CSVReader.Read("SaveData/SaveDataList");

        for(int i = 0; i < SaveList.Count; i++)
        {
            try
            {
                if (int.Parse(SaveList[i]["Round"].ToString()) > 0)
                {
                    if(int.Parse(SaveList[i]["Job"].ToString()) > highList)
                    {
                        highList = int.Parse(SaveList[i]["Job"].ToString());
                    }
                }
            }
            catch (InvalidCastException e)
            {
                Debug.Log(e.Message);

                return Job.Slayer;
            }
        }

        highList--;

        if (Enum.IsDefined(typeof(Job), highList))
        {
            return (Job)highList;
        }
        else
        {
            return Job.Slayer;
        }
    }

    public void ActivateDay()
    {
        Pause = !Pause;
    }
}
