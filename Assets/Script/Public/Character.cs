using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Serializable]
    public struct Stat
    {
        public string _myName;
        public SocialClass _mySocialClass;
        public Job _myJob;
        public int _myAge;
        public int _todoProgress;
        public int _myRound;
        public string _myPosition;
        public int _activePoint;
        public List<string> _myItem;
        public List<int> _myItemCount;
        public int[] _myStackBySocialClass;
        public int[] _myStackByJob;
        public float MyWorkSpeed;
    }

    [SerializeField]
    private Stat CharacterStat;

    private UnityEvent<int> _eventUIChange;
    private PlayerController _myPlayerController;
    private ItemManager _myItemManager;

    public static Character instance = null;

    public string MyName
    {
        get
        {
            return CharacterStat._myName;
        }
    }
    public SocialClass MySocialClass
    {
        get
        {
            return CharacterStat._mySocialClass;
        }
    }
    public Job MyJob
    {
        get
        {
            return CharacterStat._myJob;
        }
    }
    public int MyAge
    {
        get
        {
            return CharacterStat._myAge;
        }
    }
    public int TodoProgress
    {
        get
        {
            return CharacterStat._todoProgress;
        }
    }
    public int MyRound
    {
        get
        {
            return CharacterStat._myRound;
        }
    }
    public string MyPosition
    {
        get
        {
            return CharacterStat._myPosition;
        }
    }
    public int ActivePoint
    {
        get
        {
            return CharacterStat._activePoint;
        }
    }
    public List<string> MyItem
    {
        get
        {
            return CharacterStat._myItem;
        }
    }
    public List<int> MyItemCount
    {
        get
        {
            return CharacterStat._myItemCount;
        }
    }
    public int[] MyStackBySocialClass
    {
        get
        {
            return CharacterStat._myStackBySocialClass;
        }
    }
    public int[] MyStackByJob
    {
        get
        {
            return CharacterStat._myStackByJob;
        }
    }
    public UnityEvent<int> EventUIChange
    {
        get
        {
            return _eventUIChange;
        }
    }
    public PlayerController MyPlayerController
    {
        get
        {
            return _myPlayerController;
        }
    }
    public ItemManager MyItemManager
    {
        get
        {
            return _myItemManager;
        }
    }


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

        CharacterStat = new Stat();
        CharacterStat._myItem = new List<string>();
        CharacterStat._myItemCount = new List<int>();
        CharacterStat._myStackByJob = new int[11];
        CharacterStat._myStackBySocialClass = new int[5];

        
        _myPlayerController = gameObject.GetComponent<PlayerController>();
        _myItemManager = gameObject.transform.GetChild(1).GetComponent<ItemManager>();
        _eventUIChange = new UnityEvent<int>();
    }


    void Start()
    {
        GameManager.instance.DayEnd.AddListener(EndCharacter);

        CharacterStatSetting();
    }

    public void UIChangeAddListener(UnityAction<int> AddEvent)
    {
        
        EventUIChange.AddListener(AddEvent);
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
                CharacterStat._mySocialClass = (SocialClass)StatType;
                break;
            // MyJob
            case 2:
                CharacterStat._myJob = (Job)StatType;
                break;
            // MyAge
            case 3:
                CharacterStat._myAge = StatType;
                break;
            // TodoProgress
            case 4:
                if(CharacterStat._todoProgress + StatType <= 100)
                {
                    CharacterStat._todoProgress += StatType;
                }
                else
                {
                    CharacterStat._todoProgress = 100;
                    Debug.Log("TodoProgress�� 100�� ����");
                }
                break;
            // MyRound
            case 5:
                CharacterStat._myRound = StatType;
                break;
            // MyPosition
            case 6:
                CharacterStat._myPosition = StatTypeString;
                break;
            // ActivePoint
            case 7:
                CharacterStat._activePoint += StatType;
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
                        CharacterStat._myItem.Insert(ItemOrder, ItemNumber);
                        // ���� ����
                        //Debug.Log("���� ���� - " + (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                    }
                    // ������ �������� �ִٸ�
                    else
                    {
                        // ������ ����
                        //Debug.Log("�ش� ������ " + _myItemCount[ItemOrder] + "���� " + (int)(StatTypeString[4] - '0') + "��ŭ ����");
                        //CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount[ItemOrder] += (int)(StatTypeString[4] - '0');
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
                        CharacterStat._myItem.RemoveAt(ItemOrder);
                        //Debug.Log("�ش� ������ ���� ���� - " + _myItemCount[ItemOrder]);
                        CharacterStat._myItemCount.RemoveAt(ItemOrder);
                    }
                    else
                    // �Ϻθ� ����
                    {
                        //Debug.Log("�ش� ������ " + MyItemCount[ItemOrder] + "���� " + (int)(StatTypeString[5] - '0') + "��ŭ ����");
                        CharacterStat._myItemCount[ItemOrder] = MyItemCount[ItemOrder] - (int)(StatTypeString[5] - '0');
                    }
                }
                break;
            // MyStackBySocialClass
            // MyStackByJob
            case 9:
                CharacterStat._myStackBySocialClass[0] += Mathf.Abs(StatType);
                CharacterStat._myStackByJob[Type - 8] += StatType;
                break;
            case 10:
            case 11:
            case 12:
            case 13:
                CharacterStat._myStackBySocialClass[1] += Mathf.Abs(StatType);
                CharacterStat._myStackByJob[Type - 8] += StatType;
                break;
            case 14:
            case 15:
            case 16:
            case 17:
                CharacterStat._myStackBySocialClass[2] += Mathf.Abs(StatType);
                CharacterStat._myStackByJob[Type - 8] += StatType;
                break;
            case 18:
                CharacterStat._myStackBySocialClass[3] += Mathf.Abs(StatType);
                CharacterStat._myStackByJob[Type - 8] += StatType;
                break;
            case 19:
                CharacterStat._myStackBySocialClass[4] += Mathf.Abs(StatType);
                CharacterStat._myStackByJob[Type - 8] += StatType;
                break;
        }

        EventUIChange.Invoke(Type);
    }

    private void CharacterStatSetting()
    {
        if (GameManager.instance.Round == 0)
        {
            // ���� ����
            // 1. �̸� ����
            CharacterStat._myName = "Admin";
            // 2. ���/���� ����
            CharacterStat._mySocialClass = SocialClass.Slayer;
            CharacterStat._myJob = Job.Slayer;
            // 3. ���� ����
            CharacterStat._myAge = 10;
            // 4. ���൵ ����
            CharacterStat._todoProgress = 50;
            // 5. ���� ����
            CharacterStat._myRound = 1;
            // 6. Ȱ���� ����
            CharacterStat._activePoint = 100;
            // 7. ���� ����
            InitializeStack();
            // 9. �۾� �ӵ� ����
            CharacterStat.MyWorkSpeed = 1.0f;
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
            CharacterStat._myStackByJob[i] = 0;
        }
        for (int i = 0; i < MyStackBySocialClass.Length; i++)
        {
            CharacterStat._myStackBySocialClass[i] = 0;
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
                        CharacterStat._mySocialClass = SocialClass.Slayer;
                        CharacterStat._myJob = Job.Slayer;

                        InitializeStack();
                    }
                    break;
                // ��� ����
                case 1:
                    // ��� 1
                    if ((MyStackBySocialClass)[i] == 50)
                    {
                        CharacterStat._mySocialClass = SocialClass.Commons;
                        // ��������
                        if (MyStackByJob[1] + MyStackByJob[2] > 0)
                        {
                            CharacterStat._myJob = Job.Smith;
                        }
                        // ����
                        else if (MyStackByJob[1] + MyStackByJob[2] < 0)
                        {
                            CharacterStat._myJob = Job.Bania;
                        }
                        else
                        {
                            CharacterStat._myJob = Job.Smith;
                        }

                        InitializeStack();
                    }
                    // ��� 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        CharacterStat._mySocialClass = SocialClass.Commons;
                        // ����
                        if (MyStackByJob[3] + MyStackByJob[4] > 0)
                        {
                            CharacterStat._myJob = Job.MasterSmith;
                        }
                        // �����
                        else if (MyStackByJob[3] + MyStackByJob[4] < 0)
                        {
                            CharacterStat._myJob = Job.Merchant;
                        }
                        else
                        {
                            CharacterStat._myJob = Job.MasterSmith;
                        }

                        InitializeStack();
                    }
                    break;
                // �ر��� ����
                case 2:
                    // �ر��� 1
                    if (MyStackBySocialClass[i] == 50)
                    {
                        CharacterStat._mySocialClass = SocialClass.SemiNoble;
                        // ���
                        if (MyStackByJob[5] + MyStackByJob[6] > 0)
                        {
                            CharacterStat._myJob = Job.Knight;
                        }
                        // ����
                        else if (MyStackByJob[5] + MyStackByJob[6] < 0)
                        {
                            CharacterStat._myJob = Job.Scholar;
                        }
                        else
                        {
                            CharacterStat._myJob = Job.Knight;
                        }

                        InitializeStack();
                    }
                    // �ر��� 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        CharacterStat._mySocialClass = SocialClass.SemiNoble;
                        // ������
                        if (MyStackByJob[7] + MyStackByJob[8] > 0)
                        {
                            CharacterStat._myJob = Job.Masterknight;
                        }
                        // ���ݼ���
                        else if (MyStackByJob[7] + MyStackByJob[8] < 0)
                        {
                            CharacterStat._myJob = Job.Alchemist;
                        }
                        else
                        {
                            CharacterStat._myJob = Job.Masterknight;
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
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Baron;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (20 <= n && n <= 29):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Viscount;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (30 <= n && n <= 39):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Earl;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (40 <= n && n <= 49):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Marquess;
                            InitializeStack();
                            break;
                        // ����
                        case int n when (50 <= n && n <= 59):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Duke;
                            InitializeStack();
                            break;
                        // ���
                        case 100:
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.GrandDuke;
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
                        CharacterStat._mySocialClass = SocialClass.King;
                        CharacterStat._myJob = Job.King;
                        InitializeStack();
                    }
                    break;
            }
        }    
    }

    private void EndCharacter()
    {
        CharacterStat._activePoint = 100;
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
