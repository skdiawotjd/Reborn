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
    /// 직업 별 Type : 9 - 노예, 10 - 대장장이, 11 - 상인, 12 - 명장, 13 - 대상인, 14 - 기사, 15 - 학자, 16 - 기사단장, 17 - 연금술사, 18 - 귀족, 19 - 왕
    /// </para>
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - TodoProgress, 5 - MyRound, 6 - MyPositon, 7 - ActivePoint, 8~18 - MyStack(SocialClass / Job)
    /// <para>
    /// 직업 별 Type : 8 - 노예, 9 - 대장장이, 10 - 상인, 11 - 명장, 12 - 대상인, 13 - 기사, 14 - 학자, 15 - 기사단장, 16 - 연금술사, 17 - 귀족, 18 - 왕
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
                    Debug.Log("TodoProgress가 100이 넘음");
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
                
                // 추가
                if (StatTypeString[4] != '-')
                {
                    //Debug.Log("추가");
                    // 동일한 아이템이 없다면
                    if (!MyItemManager.IsExistItem(ItemNumber))
                    {
                        // 실제 아이템 추가
                        //Debug.Log("아이템 추가 - " + ItemNumber);
                        CharacterStat._myItem.Insert(ItemOrder, ItemNumber);
                        // 개수 증가
                        //Debug.Log("개수 증가 - " + (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                    }
                    // 동일한 아이템이 있다면
                    else
                    {
                        // 개수만 증가
                        //Debug.Log("해당 아이템 " + _myItemCount[ItemOrder] + "에서 " + (int)(StatTypeString[4] - '0') + "만큼 삭제");
                        //CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount[ItemOrder] += (int)(StatTypeString[4] - '0');
                    }

                }
                // 삭제하기 전에 
                else
                {
                    //Debug.Log("삭제");
                    // 가지고 있는 해당 아이템 모두 삭제
                    if (MyItemCount[ItemOrder] - (int)(StatTypeString[5] - '0') == 0)
                    {
                        //Debug.Log("해당 아이템 삭제 - " + _myItem[ItemOrder]);
                        CharacterStat._myItem.RemoveAt(ItemOrder);
                        //Debug.Log("해당 아이템 개수 삭제 - " + _myItemCount[ItemOrder]);
                        CharacterStat._myItemCount.RemoveAt(ItemOrder);
                    }
                    else
                    // 일부만 삭제
                    {
                        //Debug.Log("해당 아이템 " + MyItemCount[ItemOrder] + "에서 " + (int)(StatTypeString[5] - '0') + "만큼 삭제");
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
            // 새로 시작
            // 1. 이름 설정
            CharacterStat._myName = "Admin";
            // 2. 계급/직업 설정
            CharacterStat._mySocialClass = SocialClass.Slayer;
            CharacterStat._myJob = Job.Slayer;
            // 3. 나이 설정
            CharacterStat._myAge = 10;
            // 4. 진행도 설정
            CharacterStat._todoProgress = 50;
            // 5. 라운드 설정
            CharacterStat._myRound = 1;
            // 6. 활동력 설정
            CharacterStat._activePoint = 100;
            // 7. 스택 설정
            InitializeStack();
            // 9. 작업 속도 설정
            CharacterStat.MyWorkSpeed = 1.0f;
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
                // 노예 스택
                case 0:
                    if (MyStackBySocialClass[i] == 1)
                    {
                        CharacterStat._mySocialClass = SocialClass.Slayer;
                        CharacterStat._myJob = Job.Slayer;

                        InitializeStack();
                    }
                    break;
                // 평민 스택
                case 1:
                    // 평민 1
                    if ((MyStackBySocialClass)[i] == 50)
                    {
                        CharacterStat._mySocialClass = SocialClass.Commons;
                        // 대장장이
                        if (MyStackByJob[1] + MyStackByJob[2] > 0)
                        {
                            CharacterStat._myJob = Job.Smith;
                        }
                        // 상인
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
                    // 평민 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        CharacterStat._mySocialClass = SocialClass.Commons;
                        // 명장
                        if (MyStackByJob[3] + MyStackByJob[4] > 0)
                        {
                            CharacterStat._myJob = Job.MasterSmith;
                        }
                        // 대상인
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
                // 준귀족 스택
                case 2:
                    // 준귀족 1
                    if (MyStackBySocialClass[i] == 50)
                    {
                        CharacterStat._mySocialClass = SocialClass.SemiNoble;
                        // 기사
                        if (MyStackByJob[5] + MyStackByJob[6] > 0)
                        {
                            CharacterStat._myJob = Job.Knight;
                        }
                        // 학자
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
                    // 준귀족 2
                    else if (MyStackBySocialClass[i] == 100)
                    {
                        CharacterStat._mySocialClass = SocialClass.SemiNoble;
                        // 기사단장
                        if (MyStackByJob[7] + MyStackByJob[8] > 0)
                        {
                            CharacterStat._myJob = Job.Masterknight;
                        }
                        // 연금술사
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
                // 귀족 스택
                case 3:
                    // 귀족
                    switch (MyStackBySocialClass[i])
                    {
                        // 남작
                        case int n when (10 <= n && n <= 19):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Baron;
                            InitializeStack();
                            break;
                        // 자작
                        case int n when (20 <= n && n <= 29):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Viscount;
                            InitializeStack();
                            break;
                        // 백작
                        case int n when (30 <= n && n <= 39):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Earl;
                            InitializeStack();
                            break;
                        // 후작
                        case int n when (40 <= n && n <= 49):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Marquess;
                            InitializeStack();
                            break;
                        // 공작
                        case int n when (50 <= n && n <= 59):
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.Duke;
                            InitializeStack();
                            break;
                        // 대공
                        case 100:
                            CharacterStat._mySocialClass = SocialClass.Noble;
                            CharacterStat._myJob = Job.GrandDuke;
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
