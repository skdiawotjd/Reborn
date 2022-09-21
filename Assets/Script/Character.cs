using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    private string _myName;
    private SocialClass _mySocialClass;
    private Job _myJob;
    private int _myAge;
    private int _todoProgress;
    private int _myRound;
    private int _activePoint;
    private int[] _myStackBySocialClass;
    private int[] _myStackByJob;
    private float MyWorkSpeed;
    private InventoryManager MyInven;
    public string MyName
    {
        get
        {
            return _myName;
        }
    }
    public SocialClass MySocialClass
    {
        get
        {
            return _mySocialClass;
        }
    }
    public Job MyJob
    {
        get
        {
            return _myJob;
        }
    }
    public int MyAge
    {
        get
        {
            return _myAge;
        }
    }
    public int TodoProgress
    {
        get
        {
            return _todoProgress;
        }
    }
    public int MyRound
    {
        get
        {
            return _myRound;
        }
    }
    public int ActivePoint
    {
        get
        {
            return _activePoint;
        }
    }
    public int[] MyStackBySocialClass
    {
        get
        {
            return _myStackBySocialClass;
        }
    }
    public int[] MyStackByJob
    {
        get
        {
            return _myStackByJob;
        }
    }

    public UnityEvent<int> EventUIChange;


    public PlayerController MyPlayerController;
    public static Character instance = null;

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

        MyInven = gameObject.GetComponent<InventoryManager>();
        MyPlayerController = gameObject.GetComponent<PlayerController>();

        _myStackByJob = new int[11];
        _myStackBySocialClass = new int[5];
    }


    void Start()
    {
        GameManager.instance.GameEnd.AddListener(EndCharacter);

        CharacterStatSetting();
    }

    public void SetCharacterInput(bool CharacterInput, bool UIInput)
    {
        MyPlayerController.SetInput(CharacterInput, UIInput);
    }

    /// <summary>
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - TodoProgress, 5 - MyRound, 6 - ActivePoint, 7~17 - MyStack(SocialClass / Job)
    /// <para>
    /// 직업 별 Type : 7 - 노예, 8 - 대장장이, 9 - 상인, 10 - 명장, 11 - 대상인, 12 - 기사, 13 - 학자, 14 - 기사단장, 15 - 연금술사, 16 - 귀족, 17 - 왕
    /// </para>
    /// </summary> 
    public void SetCharacterStat<T>(int Type, T value)
    {
        int StatType = (int)(object)value;

        switch (Type)
        {
            // MySocialClass
            case 1:
                _mySocialClass = (SocialClass)StatType;
                break;
            // MyJob
            case 2:
                _myJob = (Job)StatType;
                break;
            // MyAge
            case 3:
                _myAge = StatType;
                break;
            // TodoProgress
            case 4:
                _todoProgress += StatType;
                break;
            // MyRound
            case 5:
                _myRound = StatType;
                break;
            // ActivePoint
            case 6:
                _activePoint = StatType;
                break;
            // MyStackBySocialClass
            // MyStackByJob
            case 7:
                _myStackBySocialClass[0] += Mathf.Abs(StatType);
                _myStackByJob[Type - 7] += StatType;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
                _myStackBySocialClass[1] += Mathf.Abs(StatType);
                _myStackByJob[Type - 7] += StatType;
                break;
            case 12:
            case 13:
            case 14:
            case 15:
                _myStackBySocialClass[2] += Mathf.Abs(StatType);
                _myStackByJob[Type - 7] += StatType;
                break;
            case 16:
                _myStackBySocialClass[3] += Mathf.Abs(StatType);
                _myStackByJob[Type - 7] += StatType;
                break;
            case 17:
                _myStackBySocialClass[4] += Mathf.Abs(StatType);
                _myStackByJob[Type - 7] += StatType;
                break;
        }

        EventUIChange.Invoke(Type);
    }

    private void CharacterStatSetting()
    {
        if (GameManager.instance.NewGame)
        {
            // 새로 시작
            // 1. 이름 설정
            _myName = "Admin";
            // 2. 계급/직업 설정
            _mySocialClass = SocialClass.Slayer;
            _myJob = Job.Slayer;
            // 3. 나이 설정
            _myAge = 10;
            // 4. 진행도 설정
            _todoProgress = 50;
            // 5. 라운드 설정
            _myRound = 1;
            // 6. 활동력 설정
            _activePoint = 100;
            // 7. 스택 설정
            InitializeStack();
            // 9. 작업 속도 설정
            MyWorkSpeed = 1.0f;
        }
        else
        {
            // 이어 하기
        }
    }
    private void InitializeStack()
    {
        for (int i = 0; i < MyStackByJob.Length; i++)
        {
            _myStackByJob[i] = 0;
        }
        for (int i = 0; i < MyStackBySocialClass.Length; i++)
        {
            _myStackBySocialClass[i] = 0;
        }
    }

    public void CheckStack()
    {
        for(int i = 0; i < MyStackBySocialClass.Length; i++)
        {
            //Debug.Log("MyStackBySocialClass[" + i + "] = " + MyStackBySocialClass[i]);
            switch (i)
            {
                // 노예 스택
                case 0:
                    if (MyStackBySocialClass[i] == 1)
                    {
                        _mySocialClass = SocialClass.Slayer;
                        _myJob = Job.Slayer;

                        InitializeStack();
                    }
                    break;
                // 평민 스택
                case 1:
                    // 평민 1
                    if ((MyStackBySocialClass)[i] == 50)
                    {
                        _mySocialClass = SocialClass.Commons;
                        // 대장장이
                        if (MyStackByJob[1] + MyStackByJob[2] > 0)
                        {
                            _myJob = Job.Smith;
                        }
                        // 상인
                        else if (MyStackByJob[1] + MyStackByJob[2] < 0)
                        {
                            _myJob = Job.Bania;
                        }
                        else
                        {
                            _myJob = Job.Smith;
                        }

                        InitializeStack();
                    }
                    // 평민 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        _mySocialClass = SocialClass.Commons;
                        // 명장
                        if (MyStackByJob[3] + MyStackByJob[4] > 0)
                        {
                            _myJob = Job.MasterSmith;
                        }
                        // 대상인
                        else if (MyStackByJob[3] + MyStackByJob[4] < 0)
                        {
                            _myJob = Job.Merchant;
                        }
                        else
                        {
                            _myJob = Job.MasterSmith;
                        }

                        InitializeStack();
                    }
                    break;
                // 준귀족 스택
                case 2:
                    // 준귀족 1
                    if (MyStackBySocialClass[i] == 50)
                    {
                        _mySocialClass = SocialClass.SemiNoble;
                        // 기사
                        if (MyStackByJob[5] + MyStackByJob[6] > 0)
                        {
                            _myJob = Job.Knight;
                        }
                        // 학자
                        else if (MyStackByJob[5] + MyStackByJob[6] < 0)
                        {
                            _myJob = Job.Scholar;
                        }
                        else
                        {
                            _myJob = Job.Knight;
                        }

                        InitializeStack();
                    }
                    // 준귀족 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        _mySocialClass = SocialClass.SemiNoble;
                        // 기사단장
                        if (MyStackByJob[7] + MyStackByJob[8] > 0)
                        {
                            _myJob = Job.Masterknight;
                        }
                        // 연금술사
                        else if (MyStackByJob[7] + MyStackByJob[8] < 0)
                        {
                            _myJob = Job.Alchemist;
                        }
                        else
                        {
                            _myJob = Job.Masterknight;
                        }

                        InitializeStack();
                    }

                    //InitializeStack();
                    break;
                // 귀족 스택
                case 3:
                    // 귀족
                    switch (MyStackBySocialClass[i])
                    {
                        // 남작
                        case int n when (10 <= n && n <= 19):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Baron;
                            InitializeStack();
                            break;
                        // 자작
                        case int n when (20 <= n && n <= 29):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Viscount;
                            InitializeStack();
                            break;
                        // 백작
                        case int n when (30 <= n && n <= 39):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Earl;
                            InitializeStack();
                            break;
                        // 후작
                        case int n when (40 <= n && n <= 49):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Marquess;
                            InitializeStack();
                            break;
                        // 공작
                        case int n when (50 <= n && n <= 59):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Duke;
                            InitializeStack();
                            break;
                        // 대공
                        case 100:
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.GrandDuke;
                            InitializeStack();
                            break;
                    }

                    //InitializeStack();
                    break;
                // 왕 스택
                case 4:
                    // 왕
                    if (MyStackBySocialClass[i] == 100)
                    {
                        // 왕
                        _mySocialClass = SocialClass.King;
                        _myJob = Job.King;
                        InitializeStack();
                    }
                    break;
            }
        }    
    }

    private void EndCharacter()
    {
        _activePoint = 100;
    }
}
