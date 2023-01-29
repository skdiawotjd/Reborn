using System;
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
        public int _reputation;
        public int _gold;
        public string _myMapNumber;
        public int _activePoint;
        public int _proficiency;
        public List<string> _myItem;
        public List<int> _myItemCount;
        public int _questItemOrder;
        public int[] _myStackByJob;
        public float MyWorkSpeed;
    }

    [SerializeField]
    private Stat CharacterStat;
    [SerializeField]
    private Vector2 _characterPosition;

    private UnityEvent<CharacterStatType> EventUIChange;
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
    public int Reputation
    {
        get
        {
            return CharacterStat._reputation;
        }
    }
    public int Gold
    {
        get
        {
            return CharacterStat._gold;
        }
    }
    public string MyMapNumber
    {
        get
        {
            return CharacterStat._myMapNumber;
        }
    }
    public int ActivePoint
    {
        get
        {
            return CharacterStat._activePoint;
        }
    }
    public int Proficiency
    {
        get
        {
            return CharacterStat._proficiency;
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
    public int QuestItemOrder
    {
        get
        {
            return CharacterStat._questItemOrder;
        }
    }
    public int[] MyStackByJob
    {
        get
        {
            return CharacterStat._myStackByJob;
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
    public Vector2 CharacterPosition
    {
        get
        {
            return _characterPosition;
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
        CharacterStat._myStackByJob = new int[8];
        CharacterStat._questItemOrder = 0;

        _myPlayerController = gameObject.GetComponent<PlayerController>();
        _myItemManager = gameObject.transform.GetChild(1).GetComponent<ItemManager>();
        EventUIChange = new UnityEvent<CharacterStatType>();

        GameManager.instance.AddGameStartEvent(InitializeCharacter);
        GameManager.instance.AddDayEnd(EndCharacter);
        GameManager.instance.AddLoadEvent(LoadCharacter);
    }

    public void UIChangeAddListener(UnityAction<CharacterStatType> AddEvent)
    {
        EventUIChange.AddListener(AddEvent);
    }

    public void SetCharacterInput(bool CharacterInput, bool UIInput)
    {
        MyPlayerController.SetInput(CharacterInput, UIInput);
    }
    public void SetCharacterInput(bool CharacterInput, bool CharacterMove, bool UIInput)
    {
        MyPlayerController.SetInput(CharacterInput, CharacterMove, UIInput);
    }

    public CharacterStatType ChangeJobType()
    {
        return (CharacterStatType)Enum.Parse(typeof(CharacterStatType), MyJob.ToString());
        //return (CharacterStatType)((int)MyJob + 9);
    }
    /// <summary>
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - Reputation, 5 - Gold, 6 - MyPositon, 7 - ActivePoint, 8 - Proficiency, 9 - MyItem, 10~14 - MyStack(SocialClass / Job)
    /// <para>
    /// 직업 별 Type : 10 - 대장장이, 11 - 상인, 12 - 기사, 13 - 학자, 14 - 하급귀족, 15 - 중급귀족, 16 - 상급귀족, 17 - 왕
    /// </para>
    /// </summary> 
    public void SetCharacterStat<T>(CharacterStatType Type, T value)
    {
        int StatType = 0;
        string StatTypeString = "";
        CharacterStatType TemType = (CharacterStatType)Type;

        if (TemType == CharacterStatType.MyPositon || TemType == CharacterStatType.MyItem)
        {
            StatTypeString = value.ToString();
        }
        else
        {
            StatType = (int)(object)value;
        }

        switch (TemType)
        {
            // MySocialClass
            case CharacterStatType.MySocialClass:
                CharacterStat._mySocialClass = (SocialClass)StatType;
                break;
            // MyJob
            case CharacterStatType.MyJob:
                CharacterStat._myJob = (Job)StatType;
                break;
            // MyAge
            case CharacterStatType.MyAge:
                CharacterStat._myAge = StatType;
                break;
            // Reputation
            case CharacterStatType.Reputation:
                if(CharacterStat._reputation + StatType <= 100)
                {
                    CharacterStat._reputation += StatType;
                }
                else
                {
                    CharacterStat._reputation = 100;
                    Debug.Log("Reputation가 100이 넘음");
                }
                break;
            // Gold
            case CharacterStatType.Gold:
                CharacterStat._gold += StatType;
                break;
            // MyPosition
            case CharacterStatType.MyPositon:
                CharacterStat._myMapNumber = StatTypeString;
                break;
            // ActivePoint
            case CharacterStatType.ActivePoint:
                CharacterStat._activePoint += StatType;
                break;
            // Proficiency
            case CharacterStatType.Proficiency:
                CharacterStat._proficiency += StatType;
                break;
            // MyItem
            case CharacterStatType.MyItem:
                string ItemNumber = StatTypeString.Substring(0, 4);
                int ItemOrder = MyItemManager.OrderItem(ItemNumber);
                
                // 추가
                if (StatTypeString[4] != '-')
                {
                    //Debug.Log("추가");
                    // 동일한 아이템이 없다면
                    if (!MyItemManager.IsExistItem(ItemNumber))
                    {
                        // 아이템 추가
                        //Debug.Log("아이템 추가 - " + ItemNumber);
                        CharacterStat._myItem.Insert(ItemOrder, ItemNumber);
                        // 개수 증가
                        //Debug.Log("개수 증가 - " + (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                        // 퀘스트아이템 위치 설정
                        if (CharacterStat._questItemOrder != 0)
                        {
                            if (ItemNumber[0] - '0' < 7)
                            {
                                CharacterStat._questItemOrder++;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            CharacterStat._questItemOrder = ItemOrder;

                            return;
                        }
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
                        // 아이템 삭제
                        //Debug.Log("해당 아이템 삭제 - " + _myItem[ItemOrder]);
                        CharacterStat._myItem.RemoveAt(ItemOrder);
                        // 개수 삭제
                        //Debug.Log("해당 아이템 개수 삭제 - " + _myItemCount[ItemOrder]);
                        CharacterStat._myItemCount.RemoveAt(ItemOrder);
                        // 퀘스트아이템 위치 설정
                        if (CharacterStat._questItemOrder != 0)
                        {
                            if (CharacterStat._myItem[ItemOrder][0] - '0' < 7)
                            {
                                CharacterStat._questItemOrder--;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    // 일부만 삭제
                    {
                        //Debug.Log("해당 아이템 " + MyItemCount[ItemOrder] + "에서 " + (int)(StatTypeString[5] - '0') + "만큼 삭제");
                        CharacterStat._myItemCount[ItemOrder] = MyItemCount[ItemOrder] - (int)(StatTypeString[5] - '0');
                    }
                }
                break;
            case CharacterStatType InputType when (InputType >= CharacterStatType.Smith):
                CharacterStat._myStackByJob[(int)Type - 10] += StatType;
                break;
        }

        EventUIChange.Invoke(TemType);
    }

    public void CharacterStatSetting()
    {
        // 새로 시작
        // 1. 이름 설정
        CharacterStat._myName = "주인공";
        // 2. 계급/직업 설정
        CharacterStat._mySocialClass = SocialClass.Helot;
        CharacterStat._myJob = Job.Slayer;
        // 3. 나이 설정
        CharacterStat._myAge = 10;
        // 4. 진행도 설정
        CharacterStat._reputation = 0;
        // 5. 골드 설정
        CharacterStat._gold = 0;
        // 6. 위치 설정
        CharacterStat._myMapNumber = "0007";
        // 7. 활동력 설정
        CharacterStat._activePoint = 100;
        // 8. 스택 설정
        InitializeStack();
        // 9. 작업 속도 설정
        CharacterStat.MyWorkSpeed = 1.0f;
    }

    public void CharacterStatSetting(Job StartJob)
    {
        CharacterStat._myJob = StartJob;

        switch (StartJob)
        {
            case Job.Slayer:
                CharacterStat._mySocialClass = SocialClass.Helot;
                CharacterStat._activePoint = 100;
                break;
            case Job.Smith:
            case Job.Bania:
                CharacterStat._mySocialClass = SocialClass.Commons;
                CharacterStat._activePoint = 100;
                break;
            case Job.Knight:
            case Job.Alchemist:
                CharacterStat._mySocialClass = SocialClass.SemiNoble;
                CharacterStat._activePoint = 100;
                break;
            case Job.LowNobility:
            case Job.MiddleNobility:
            case Job.HighNobility:
                CharacterStat._mySocialClass = SocialClass.Noble;
                CharacterStat._activePoint = 100;
                break;
            case Job.King:
                CharacterStat._mySocialClass = SocialClass.King;
                CharacterStat._activePoint = 100;
                break;
        }

        // 1. 이름 설정
        CharacterStat._myName = "주인공";

        // 3. 나이 설정
        CharacterStat._myAge = 10;
        // 4. 진행도 설정
        CharacterStat._reputation = 0;
        // 5. 골드 설정
        CharacterStat._gold = 0;
        // 6. 위치 설정
        CharacterStat._myMapNumber = "0" + ((int)CharacterStat._myJob).ToString() + "02";

        // 8. 스택 설정
        InitializeStack();
        // 9. 작업 속도 설정
        CharacterStat.MyWorkSpeed = 1.0f;
    }

    public void InitializeCharacter()
    {
        //InitializeMapNumber
        CharacterStat._myMapNumber = "0" + ((int)CharacterStat._myJob).ToString() + "02";
        // 3. 나이 설정
        CharacterStat._myAge = 10;
        // 6. 활동력 설정
        CharacterStat._activePoint = 100;
    }

    private void InitializeStack()
    {
        for (int i = 0; i < MyStackByJob.Length; i++)
        {
            CharacterStat._myStackByJob[i] = 0;
        }
    }

    public void CheckStack()
    {
        for (int CheckStack = 0; CheckStack < MyStackByJob.Length; CheckStack++)
        {
            //Debug.Log("MyStackBySocialClass[" + i + "] = " + MyStackBySocialClass[i]);
            if(MyStackByJob[CheckStack] >= 50)
            {
                switch(CheckStack)
                {
                    case 0:
                    case 1:
                        CharacterStat._mySocialClass = SocialClass.Commons;
                        break;
                    case 2:
                    case 3:
                        CharacterStat._mySocialClass = SocialClass.SemiNoble;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        CharacterStat._mySocialClass = SocialClass.Noble;
                        break;
                    case 7:
                        CharacterStat._mySocialClass = SocialClass.King;
                        break;
                }
                CharacterStat._myJob = (Job)(CheckStack + 1);

                InitializeStack();
            }
        }    
    }

    private void EndCharacter()
    {
        CharacterStat._activePoint = 100;
        CharacterStat._myAge += 1;
    }
    public void SaveCharacter()
    {
        //Debug.Log("SaveCharacter : " + transform.position);
        _characterPosition = transform.position;
    }

    public void LoadCharacter()
    {
        //Debug.Log("LoadEvent - Character CharacterPosition : " + CharacterPosition);
        transform.position = CharacterPosition;
    }

    public void SetCharacterPosition()
    {
        if (CharacterPosition == Vector2.zero)
        //if (CharacterPosition.x == 0 && CharacterPosition.y == 0)
        {
            /*switch (CharacterStat._myMapNumber)
            {
                //Tutorial
                case "0004":
                    _characterPosition.x = 7f;
                    _characterPosition.y = -2.7f;
                    break;
                // Home
                case "0000":
                    _characterPosition.x = -3.8f;
                    _characterPosition.y = -3.3f;
                    MyPlayerController.PlayerRotation(Direction.Right);
                    break;
                // Town
                case "0001":
                case "0101":
                case "0201":
                case "0301":
                case "0401":
                    _characterPosition.x = -11.8f;
                    _characterPosition.y = 5.3f;
                    MyPlayerController.PlayerRotation(Direction.Right);
                    break;
                case "0008":
                case "0108":
                    _characterPosition.y = -2f;
                    MyPlayerController.PlayerRotation(Direction.Right);
                    break;

            }*/
        }

        switch (CharacterStat._myMapNumber)
        {
            //Tutorial
            case "0004":
                _characterPosition.x = 7f;
                _characterPosition.y = -2.7f;
                break;
            // Home
            case "0000":
                _characterPosition.x = -3.8f;
                _characterPosition.y = -3.3f;
                MyPlayerController.PlayerRotation(Direction.Right);
                break;
            // Town
            case "0101":
            case "0201":
            case "0301":
            case "0401":
                _characterPosition.x = -11.8f;
                _characterPosition.y = 5.3f;
                MyPlayerController.PlayerRotation(Direction.Right);
                break;
            case "0008":
            case "0108":
                _characterPosition.y = -2f;
                MyPlayerController.PlayerRotation(Direction.Right);
                break;
            case "0001":
                _characterPosition.x = 0f;
                _characterPosition.y = -2f;
                break;

        }

        transform.position = CharacterPosition;
        //Debug.Log("Set " + transform.position);
        _characterPosition.x = 0f;
        _characterPosition.y = 0f;
    }
}
