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
    private int _myRound;
    private int _todoProgress;
    private int[] _myStackByJob;
    private float MyWorkSpeed;
    private Inventory MyInven;
    public string MyName
    {
        get
        {
            return _myName;
        }
    }
    public int MySocialClass
    {
        get
        {
            return (int)_mySocialClass;
        }
    }
    public int MyJob
    {
        get
        {
            return (int)_myJob;
        }
    }
    public int MyAge
    {
        get
        {
            return _myAge;
        }
    }
    public int MyRound
    {
        get
        {
            return _myRound;
        }
    }
    public float TodoProgress
    {
        get
        {
            return _todoProgress;
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

        MyInven = gameObject.GetComponent<Inventory>();
        MyPlayerController = gameObject.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterStatSetting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCharacterInput(bool CharacterInput, bool UIInput)
    {
        MyPlayerController.SetInput(CharacterInput, UIInput);
    }

    /// <summary>
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - MyRound, 5 - TodoProgress, 6 - MyStackByJob
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
            // MyRound
            case 4:
                _myRound = StatType;
                break;
            // TodoProgress
            case 5:
                _todoProgress += StatType;
                break;
            // MyStackByJob
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
            case 21:
                _myStackByJob[Type - 6] += StatType;
                break;
        }

        EventUIChange.Invoke(Type);
    }

    private void CharacterStatSetting()
    {
        if(GameManager.instance.NewGame)
        {
            // 새로 시작
            // 1. 이름 설정
            _myName = "Admin";
            // 2. 계급/직업 설정
            _mySocialClass = SocialClass.Slayer;
            _myJob = Job.Slayer;
            // 3. 나이 설정
            _myAge = 10;
            // 4. 라운드 설정
            _myRound = 1;
            // 5. 진행도 설정
            _todoProgress = 0;
            // 6. 스택 설정
            _myStackByJob = new int[16];
            for (int i = 0; i < MyStackByJob.Length; i++)
            {
                MyStackByJob[i] = 0;
            }
            // 7. 작업 속도 설정
            MyWorkSpeed = 1.0f;
        }
        else
        {
            // 이어 하기
        }
    }
}
