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
    [SerializeField]
    private float _playTime;        // 하루에 지나간 시간
    [SerializeField]
    private float _totalPlayTime;   // 하루의 총 시간
    [SerializeField]
    private bool _isdayStart;       // 하루가 시작되었는지
    [SerializeField]
    private bool _isNewGame;        // 새 게임인지
    [SerializeField]
    private int Days;               // 며칠이 지났는지

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
    public bool IsNewGame
    {
        get
        {
            return _isNewGame;
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

    public UnityEvent DayStart;
    public UnityEvent DayEnd;
    public UnityEvent SceneMove;
    public UnityEvent LoadEvent;

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
        _isNewGame = true;
        Days = 0;

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
                _playTime += Time.deltaTime;
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
        if(_isNewGame)
        {
            // 1. Start씬의 카메라 false
            GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
            // 2. 시작 씬 이동
            SceneManager.LoadScene("JustChat");
            // 3. 게임 초기화
            InitializeGame();
        }
        else
        {
            InitializeGame();
        }
    }


    private void InitializeGame()
    {
        // Canvas 세팅
        GameObject CanvasObject = Instantiate(Resources.Load("Public/Main Canvas 1")) as GameObject;
        CanvasObject.name = "Main Canvas";
        DontDestroyOnLoad(CanvasObject);

        // Canvas 카메라 세팅
        GameObject MainCamera = Instantiate(Resources.Load("Public/Main Camera")) as GameObject;
        MainCamera.name = "Main Camera";

        // Character 세팅
        GameObject PlayerCharacter = Instantiate(Resources.Load("Public/PlayerCharacter")) as GameObject;
        PlayerCharacter.name = "PlayerCharacter";

        // QuestManager 세팅
        GameObject QuestManager = Instantiate(Resources.Load("Public/QuestManager")) as GameObject;
        QuestManager.name = "QuestManager";

        // SceneLoadManager 세팅
        GameObject SceneLoadManager = Instantiate(Resources.Load("Public/SceneLoadManager")) as GameObject;
        SceneLoadManager.name = "SceneLoadManager";
    }

    // 각 분기까지 3일이 걸림, 9일째는 회차 완료라 가정
    private void InitializeDay()
    {
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
                QuestManager.instance.QuestGive();
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
                    QuestManager.instance.QuestGive();
                    NewDay();
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
                    QuestManager.instance.QuestGive();
                    NewDay();
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
                    //Character.instance.CheckStack();
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
                    QuestManager.instance.QuestGive();
                    NewDay();
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
                    Character.instance.CheckStack();
                    QuestManager.instance.QuestGive();
                    NewDay();
                }
                else
                {
                    // 생존
                    Debug.Log("생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                    QuestManager.instance.QuestGive();
                    NewDay();
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
                }
                else
                {
                    // 생존
                    Debug.Log("끝인데 생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
                }
            }
        }

        Character.instance.SetCharacterStat(4, -Character.instance.TodoProgress);
    }

    // 하루 시작 과정
    private void NewDay()
    {
        // 값 초기화
        _playTime = 0f;
        //Debug.Log(Days + "일 끝");
        DayEnd.Invoke();
        Days += 1;

        // 새로운 하루 시작
        Invoke("NextCycle", 0.01f);
    }

    private void NextCycle()
    {
        switch(Character.instance.MyRound)
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
        SceneMove.Invoke();

        

        if (IsNewGame && SceneName == "Home")
        {
            NewDay();
            _isNewGame = false;
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

        AssetDatabase.Refresh();
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
}
