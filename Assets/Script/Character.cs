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

    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.GameEnd.AddListener(EndCharacter);

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
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - TodoProgress, 5 - MyRound, 6 - MyStackByJob
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
        if (GameManager.instance.NewGame)
        {
            // ���� ����
            // 1. �̸� ����
            _myName = "Admin";
            // 2. ���/���� ����
            _mySocialClass = SocialClass.Slayer;
            _myJob = Job.Slayer;
            // 3. ���� ����
            _myAge = 10;
            // 4. ���൵ ����
            _todoProgress = 50;
            // 5. ���� ����
            _myRound = 1;
            // 6. ���� ����
            _myStackByJob = new int[16];
            for (int i = 0; i < MyStackByJob.Length; i++)
            {
                MyStackByJob[i] = 0;
            }
            // 7. �۾� �ӵ� ����
            MyWorkSpeed = 1.0f;
        }
        else
        {
            // �̾� �ϱ�
        }
    }

    private void EndCharacter()
    {
        _todoProgress = 0;
    }
}
