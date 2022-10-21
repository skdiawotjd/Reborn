using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �Ϸ翡 ������ �ð�
    private float _playTime;
    // �Ϸ��� �� �ð�
    private float _totalPlayTime;
    // �Ϸ簡 ���۵Ǿ�����
    private bool _isdayStart;
    // �� ��������
    private bool _isnewGame;
    // ��ĥ�� ��������
    private int Days;

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
            return _isnewGame;
        }
    }
    public string SceneName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

    public UnityEvent DayStart;
    public UnityEvent DayEnd;
    public UnityEvent SceneMove;

    public static GameManager instance = null;
    // ���� ũ��
    public RectTransform Background;

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
        _totalPlayTime = 60f;
        _isdayStart = false;
        _isnewGame = true;
        Days = 0;

        SceneManager.sceneLoaded += LoadedsceneEvent;

        SceneMove.AddListener(CheckScene);
    }

    
    void Start()
    {
        InitializeGame();
        NewDay();
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

    private void InitializeGame()
    {
        // Canvas ����
        GameObject CanvasObject = Instantiate(Resources.Load("Public/Main Canvas")) as GameObject;
        CanvasObject.name = "Main Canvas";
        DontDestroyOnLoad(CanvasObject);

        // Canvas ī�޶� ����
        GameObject MainCamera = Instantiate(Resources.Load("Public/Main Camera")) as GameObject;
        MainCamera.name = "Main Camera";

        // Character ����
        GameObject PlayerCharacter = Instantiate(Resources.Load("Public/PlayerCharacter")) as GameObject;
        PlayerCharacter.name = "PlayerCharacter";

        // QuestManager ����
        GameObject QuestManager = Instantiate(Resources.Load("Public/QuestManager")) as GameObject;
        QuestManager.name = "QuestManager";
    }

    // �� �б���� 3���� �ɸ�, 9��°�� ȸ�� �Ϸ�� ����
    private void InitializeDay()
    {
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
                QuestManager.instance.QuestGive();
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
                    Character.instance.CheckStack();
                    QuestManager.instance.QuestGive();
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
                    Character.instance.CheckStack();
                    QuestManager.instance.QuestGive();
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
                    // ���
                    Debug.Log("������������������������������");
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    Character.instance.CheckStack();
                    QuestManager.instance.QuestGive();
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
                    QuestManager.instance.QuestGive();
                    NewDay();
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    Character.instance.CheckStack();
                    QuestManager.instance.QuestGive();
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
                }
                else
                {
                    // ����
                    Debug.Log("���ε� ����������������������������������������");
                    Character.instance.CheckStack();
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
        Debug.Log(Days + "�� ��");
        DayEnd.Invoke();
        Days += 1;

        // ���ο� �Ϸ� ����
        Invoke("NextCycle", 0.01f);
    }

    private void NextCycle()
    {
        switch(Character.instance.MyRound)
        {
            case 1:
                _isdayStart = true;
                Debug.Log(Days + "�� ����");
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
        Background = GameObject.Find("Background").GetComponent<RectTransform>();
        
        SceneMove.Invoke();
    }

    private void CheckScene()
    {
        if(IsNewGame && SceneName == "Home")
        {
            NewDay();
        }
    }

    public void NewGame()
    {
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;

        SceneManager.LoadScene("JustChat");

        InitializeGame();
    }

    public void LoadGame()
    {
        _isnewGame = false;
        Debug.Log("����");
    }
}
