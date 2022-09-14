using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public static GameManager instance = null;

    private void Awake()
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
        _totalPlayTime = 5f;
        _dayStart = false;
        _newGame = true;
        Days = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        NextCycle();
    }

    // Update is called once per frame
    void Update()
    {
        if (DayStart)
        {
            if (Mathf.Floor(_playTime) != TotalPlayTime)
            {
                _playTime += Time.deltaTime;

                Debug.Log(Mathf.Floor(_playTime));
            }
            else
            {
                Debug.Log(Days + "일 끝");
                _dayStart = false;
                Days += 1;

                InitializeDay();
                
                Invoke("NextCycle", 1f);
            }
        }
    }

    private void InitializeDay()
    {
        // 1. 분기 판단
        if (Days == 3)
        {
            CheckQuarter();
        }

        // 2. 값 초기화
        _playTime = 0f;
        Character.instance.MyPlayerController.SetInput(false, false);
        GameEnd.Invoke();

        
    }

    // 분기 판단
    private void CheckQuarter()
    {
        for(int i = 0; i < Character.instance.MyStackByJob.Length; i++)
        {
            if (Character.instance.MyStackByJob[i] != 0)
            {
                Debug.Log(i + "번 째 값이 " + Character.instance.MyStackByJob[i]);
                Character.instance.SetCharacterStat(2, 1);
            }
        }
    }

    private void NextCycle()
    {
        switch(Character.instance.MyRound)
        {
            case 1:
                _dayStart = true;
                Character.instance.MyPlayerController.SetInput(true, true);
                GameStart.Invoke();
                Debug.Log(Days + "일 시작");
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
