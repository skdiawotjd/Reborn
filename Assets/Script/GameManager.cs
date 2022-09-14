using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // �Ϸ翡 ������ �ð�
    private float _playTime;
    // �Ϸ��� �� �ð�
    private float _totalPlayTime;
    // �Ϸ簡 ���۵Ǿ�����
    private bool _dayStart;
    // �� ��������
    private bool _newGame;
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
                Debug.Log(Days + "�� ��");
                _dayStart = false;
                Days += 1;

                InitializeDay();
                
                Invoke("NextCycle", 1f);
            }
        }
    }

    private void InitializeDay()
    {
        // 1. �б� �Ǵ�
        if (Days == 3)
        {
            CheckQuarter();
        }

        // 2. �� �ʱ�ȭ
        _playTime = 0f;
        Character.instance.MyPlayerController.SetInput(false, false);
        GameEnd.Invoke();

        
    }

    // �б� �Ǵ�
    private void CheckQuarter()
    {
        for(int i = 0; i < Character.instance.MyStackByJob.Length; i++)
        {
            if (Character.instance.MyStackByJob[i] != 0)
            {
                Debug.Log(i + "�� ° ���� " + Character.instance.MyStackByJob[i]);
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
                Debug.Log(Days + "�� ����");
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
