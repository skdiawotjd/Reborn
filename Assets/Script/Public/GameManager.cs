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
    private bool _isNewGenerate;    // ������ ó�� �⵿�ߴ���
    [SerializeField]
    private float _playTime;        // �Ϸ翡 ������ �ð�
    [SerializeField]
    private float _totalPlayTime;   // �Ϸ��� �� �ð�
    
    private bool _isDayStart;       // �Ϸ簡 ���۵Ǿ�����
    [SerializeField]
    private int _round;             // ��ȸ��
    [SerializeField]
    private int _days;              // ��ĥ�� ��������

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

    // ���� ���� ���� ��
    private UnityEvent GenerateGameEvent;
    // ������ �Ϸ�� �� ���� ��
    private UnityEvent GameStartEvent;
    // �Ϸ簡 ���� ��
    private UnityEvent DayStart;
    // �Ϸ簡 ���� ��
    private UnityEvent DayEnd;
    // ���� ����� ��
    private UnityEvent SceneMoveEvent;
    // �ε� ��
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

            // 1. ���� �ʱ�ȭ
            InitializeGame();
            Character.instance.CharacterStatSetting();
            GenerateGameEvent.Invoke();
            GameStartEvent.Invoke();

            //GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
            // 2. ���� �� �̵�
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
            // 1. ���� �ʱ�ȭ
            InitializeGame();
            Character.instance.CharacterStatSetting(StartJob);
            GenerateGameEvent.Invoke();
            GameStartEvent.Invoke();

            // 2. ���� �� �̵�
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
        // Character ����
        if(!Character.instance)
        {
            GameObject PlayerCharacter = Instantiate(Resources.Load("Public/PlayerCharacter")) as GameObject;
            PlayerCharacter.name = "PlayerCharacter";
        }
        
        // QuestManager ����
        GameObject QuestManager = Instantiate(Resources.Load("Public/QuestManager")) as GameObject;
        QuestManager.name = "QuestManager";
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
        switch (Days)
        {
            case 0:
                Debug.Log("3 Or 6���� ����");
                Character.instance.CheckStack();
                Character.instance.SetCharacterStat(CharacterStatType.Reputation, -Character.instance.Reputation);

                GameStart();
                break;
            case 3:
            case 6:
                Debug.Log("3 Or 6���� ����");
                if (Character.instance.Reputation >= 10 / Days)
                {
                    // ���� ��� ���� ���
                }
                else
                {
                    // �� ������ �´� ��� ��縦 ���� NpcNumberChatType set
                }
                NewDay();
                break;
            case 9:
                Debug.Log("9���� ����");
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
                    // ���
                    Debug.Log("������������������������������");
                }
                break;
        }
    }

    // �Ϸ� ���� ����
    public void NewDay()
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
        /*switch (Round)
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
        }*/
        _isDayStart = true;
        Pause = false;
        //Debug.Log(Days + "�� ����");
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
            case "00": // ��
                SceneManager.LoadScene("Home");
                break;
            case "02": // JustChat
                SceneManager.LoadScene("JustChat");
                break;
            case "01": // ����Ʈ
            case "03": // DDR
            case "04": // Ÿ�̹�
            case "05": // ����
            case "06": // ������Ʈ ��ġ
            case "07": // ���� ����
            case "08": // Ž��
            case "09": // ����
            case "10": // �й�����
            case "11": // ��ī����
            case "12": // ��ġ��
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
