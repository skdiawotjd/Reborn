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
    private string _myPosition;
    private int _activePoint;
    private List<string> _myItem;
    private List<int> _myItemCount;
    private int[] _myStackBySocialClass;
    private int[] _myStackByJob;
    private float MyWorkSpeed;

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
    public string MyPosition
    {
        get
        {
            return _myPosition;
        }
    }
    public int ActivePoint
    {
        get
        {
            return _activePoint;
        }
    }
    public List<string> MyItem
    {
        get
        {
            return _myItem;
        }
    }
    public List<int> MyItemCount
    {
        get
        {
            return _myItemCount;
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
    public ItemManager MyItemManager;
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

        _myItem = new List<string>();
        _myItemCount = new List<int>();
        _myStackByJob = new int[11];
        _myStackBySocialClass = new int[5];
    }


    void Start()
    {
        GameManager.instance.DayEnd.AddListener(EndCharacter);

        CharacterStatSetting();
    }

    public void SetCharacterInput(bool CharacterInput, bool UIInput)
    {
        MyPlayerController.SetInput(CharacterInput, UIInput);
    }

    /// <summary>
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - TodoProgress, 5 - MyRound, 6 - MyPositon, 7 - ActivePoint, 8 - MyItem, 9~19 - MyStack(SocialClass / Job)
    /// <para>
    /// ���� �� Type : 9 - �뿹, 10 - ��������, 11 - ����, 12 - ����, 13 - �����, 14 - ���, 15 - ����, 16 - ������, 17 - ���ݼ���, 18 - ����, 19 - ��
    /// </para>
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - TodoProgress, 5 - MyRound, 6 - MyPositon, 7 - ActivePoint, 8~18 - MyStack(SocialClass / Job)
    /// <para>
    /// ���� �� Type : 8 - �뿹, 9 - ��������, 10 - ����, 11 - ����, 12 - �����, 13 - ���, 14 - ����, 15 - ������, 16 - ���ݼ���, 17 - ����, 18 - ��
    /// </para>
    /// </summary> 
    public void SetCharacterStat<T>(int Type, T value)
    {
        int StatType = 0;
        string StatTypeString = "";
        if(Type == 6 || Type == 8)
        {
            StatTypeString = value.ToString();
        }
        else
        {
            StatType = (int)(object)value;
        }

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
                if(_todoProgress + StatType <= 100)
                {
                    _todoProgress += StatType;
                }
                else
                {
                    _todoProgress = 100;
                    Debug.Log("TodoProgress�� 100�� ����");
                }
                break;
            // MyRound
            case 5:
                _myRound = StatType;
                break;
            // MyPosition
            case 6:
                _myPosition = StatTypeString;
                break;
            // ActivePoint
            case 7:
                _activePoint += StatType;
                break;
            // MyItem
            case 8:
                string ItemNumber = StatTypeString.Substring(0, 4);
                int ItemOrder = MyItemManager.OrderItem(ItemNumber);
                
                // �߰�
                if (StatTypeString[4] != '-')
                {
                    //Debug.Log("�߰�");
                    // ������ �������� ���ٸ�
                    if (!MyItemManager.IsExistItem(ItemNumber))
                    {
                        // ���� ������ �߰�
                        //Debug.Log("������ �߰� - " + ItemNumber);
                        _myItem.Insert(ItemOrder, ItemNumber);
                        // ���� ����
                        //Debug.Log("���� ���� - " + (int)(StatTypeString[4] - '0'));
                        _myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                    }
                    // ������ �������� �ִٸ�
                    else
                    {
                        // ������ ����
                        //Debug.Log("�ش� ������ " + _myItemCount[ItemOrder] + "���� " + (int)(StatTypeString[4] - '0') + "��ŭ ����");
                        _myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                    }

                }
                // �����ϱ� ���� 
                else
                {
                    //Debug.Log("����");
                    // ������ �ִ� �ش� ������ ��� ����
                    if (MyItemCount[ItemOrder] - (int)(StatTypeString[5] - '0') == 0)
                    {
                        //Debug.Log("�ش� ������ ���� - " + _myItem[ItemOrder]);
                        _myItem.RemoveAt(ItemOrder);
                        //Debug.Log("�ش� ������ ���� ���� - " + _myItemCount[ItemOrder]);
                        _myItemCount.RemoveAt(ItemOrder);
                    }
                    else
                    // �Ϻθ� ����
                    {
                        //Debug.Log("�ش� ������ " + MyItemCount[ItemOrder] + "���� " + (int)(StatTypeString[5] - '0') + "��ŭ ����");
                        _myItemCount[ItemOrder] = MyItemCount[ItemOrder] - (int)(StatTypeString[5] - '0');
                    }
                }
                break;
            // MyStackBySocialClass
            // MyStackByJob
            case 9:
                _myStackBySocialClass[0] += Mathf.Abs(StatType);
                _myStackByJob[Type - 8] += StatType;
                break;
            case 10:
            case 11:
            case 12:
            case 13:
                _myStackBySocialClass[1] += Mathf.Abs(StatType);
                _myStackByJob[Type - 8] += StatType;
                break;
            case 14:
            case 15:
            case 16:
            case 17:
                _myStackBySocialClass[2] += Mathf.Abs(StatType);
                _myStackByJob[Type - 8] += StatType;
                break;
            case 18:
                _myStackBySocialClass[3] += Mathf.Abs(StatType);
                _myStackByJob[Type - 8] += StatType;
                break;
            case 19:
                _myStackBySocialClass[4] += Mathf.Abs(StatType);
                _myStackByJob[Type - 8] += StatType;
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
            // 6. Ȱ���� ����
            _activePoint = 100;
            // 7. ���� ����
            InitializeStack();
            // 9. �۾� �ӵ� ����
            MyWorkSpeed = 1.0f;
        }
        else
        {
            // �̾� �ϱ�
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
                // �뿹 ����
                case 0:
                    if (MyStackBySocialClass[i] == 1)
                    {
                        _mySocialClass = SocialClass.Slayer;
                        _myJob = Job.Slayer;

                        InitializeStack();
                    }
                    break;
                // ��� ����
                case 1:
                    // ��� 1
                    if ((MyStackBySocialClass)[i] == 50)
                    {
                        _mySocialClass = SocialClass.Commons;
                        // ��������
                        if (MyStackByJob[1] + MyStackByJob[2] > 0)
                        {
                            _myJob = Job.Smith;
                        }
                        // ����
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
                    // ��� 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        _mySocialClass = SocialClass.Commons;
                        // ����
                        if (MyStackByJob[3] + MyStackByJob[4] > 0)
                        {
                            _myJob = Job.MasterSmith;
                        }
                        // �����
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
                // �ر��� ����
                case 2:
                    // �ر��� 1
                    if (MyStackBySocialClass[i] == 50)
                    {
                        _mySocialClass = SocialClass.SemiNoble;
                        // ���
                        if (MyStackByJob[5] + MyStackByJob[6] > 0)
                        {
                            _myJob = Job.Knight;
                        }
                        // ����
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
                    // �ر��� 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        _mySocialClass = SocialClass.SemiNoble;
                        // ������
                        if (MyStackByJob[7] + MyStackByJob[8] > 0)
                        {
                            _myJob = Job.Masterknight;
                        }
                        // ���ݼ���
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
                // ���� ����
                case 3:
                    // ����
                    switch (MyStackBySocialClass[i])
                    {
                        // ����
                        case int n when (10 <= n && n <= 19):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Baron;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (20 <= n && n <= 29):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Viscount;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (30 <= n && n <= 39):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Earl;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (40 <= n && n <= 49):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Marquess;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (50 <= n && n <= 59):
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.Duke;
                            InitializeStack();
                            break;
                        // ���
                        case 100:
                            _mySocialClass = SocialClass.Noble;
                            _myJob = Job.GrandDuke;
                            InitializeStack();
                            break;
                    }

                    //InitializeStack();
                    break;
                // �� ����
                case 4:
                    // ��
                    if (MyStackBySocialClass[i] == 100)
                    {
                        // ��
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

/*    public void PopItem(int ItemType)
    {
        EventItemPop.Invoke(ItemType);
    }

    public void PushItem(int ItemType)
    {
        EventItemPush.Invoke(ItemType);
    }*/
}
