using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 하루에 지나간 시간
    private float _playTime;
    // 하루의 총 시간
    private float _totalPlayTime;
    // 하루가 시작되었는지
    private bool _dayStart;
    // 새 게임인지
    private bool _newGame;
    // 며칠이 지났는지
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
    public bool DayStart
    {
        get
        {
            return _dayStart;
        }
    }
    public bool NewGame
    {
        get
        {
            return _newGame;
        }
    }

    public UnityEvent GameStart;
    public UnityEvent GameEnd;
    public UnityEvent SceneMove;

    public static GameManager instance = null;
    // 씬의 크기
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
        _dayStart = false;
        _newGame = true;
        Days = 0;

        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    
    void Start()
    {
        InitializeGame();

        NewDay();
    }
    
    void Update()
    {
        if (DayStart)
        {
            if (Mathf.Floor(_playTime) != TotalPlayTime && Character.instance.ActivePoint != 0)
            {
                _playTime += Time.deltaTime;

                //Debug.Log(Mathf.Floor(_playTime));
            }
            else
            {
                Debug.Log(Days + "일 끝");
                _dayStart = false;
                InitializeDay();
            }
        }
    }

    private void InitializeGame()
    {
        // Canvas 세팅
        GameObject CanvasObject = Instantiate(Resources.Load("Public/Canvas")) as GameObject;
        CanvasObject.name = "Canvas";
        DontDestroyOnLoad(CanvasObject);

        // Canvas 카메라 세팅
        _ = Instantiate(Resources.Load("Public/Main Camera")) as GameObject;
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
            if (Days == 3)
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
                    NewDay();
                }
                else
                {
                    // 생존
                    Debug.Log("생존생존생존생존생존생존생존생존생존생존");
                    Character.instance.CheckStack();
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

        Character.instance.SetCharacterStat(4, 0);
    }

    // 하루 시작 과정
    private void NewDay()
    {
        // 값 초기화
        _playTime = 0f;
        Character.instance.SetCharacterInput(false, false);
        GameEnd.Invoke();
        Days += 1;

        // 새로운 하루 시작
        Invoke("NextCycle", 1f);
    }

    private void NextCycle()
    {
        switch(Character.instance.MyRound)
        {
            case 1:
                _dayStart = true;
                Character.instance.SetCharacterInput(true, true);
                GameStart.Invoke();
                Debug.Log(Days + "일 시작");
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
}
