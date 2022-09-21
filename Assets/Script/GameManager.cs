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
        NewDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (DayStart)
        {
            if (Mathf.Floor(_playTime) != TotalPlayTime && Character.instance.ActivePoint != 0)
            {
                _playTime += Time.deltaTime;

                Debug.Log(Mathf.Floor(_playTime));
            }
            else
            {
                Debug.Log(Days + "�� ��");
                _dayStart = false;
                InitializeDay();
            }
        }
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
            if (Days == 3)
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
                    NewDay();
                }
                else
                {
                    // ����
                    Debug.Log("����������������������������������������");
                    Character.instance.CheckStack();
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
    }

    // �Ϸ� ���� ����
    private void NewDay()
    {
        // �� �ʱ�ȭ
        _playTime = 0f;
        Character.instance.SetCharacterInput(false, false);
        GameEnd.Invoke();
        Days += 1;

        // ���ο� �Ϸ� ����
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
                Debug.Log(Days + "�� ����");
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
