using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    private enum ItemType { Plus = 4, Minus }

    [Serializable]
    public struct Stat
    {
        public string _myName;
        public SocialClass _mySocialClass;
        public Job _myJob;
        public int _myAge;
        public int _reputation;
        public string _myMapNumber;
        public int _activePoint;
        public int _proficiency;
        public List<string> _myItem;
        public List<int> _myItemCount;
        public int _questItemOrder;
        public int[] _myStack;
        public float MyWorkSpeed;
    }

    [SerializeField]
    private Stat CharacterStat;
    [SerializeField]
    private Vector2 _characterPosition;

    private int _stackOrder;

    private UnityEvent<CharacterStatType> EventUIChange;
    private PlayerController _myPlayerController;
    private ItemManager _myItemManager;

    public static Character instance = null;

    public string MyName
    {
        get { return CharacterStat._myName; }
    }
    public SocialClass MySocialClass
    {
        get { return CharacterStat._mySocialClass; }
    }
    public Job MyJob
    {
        get { return CharacterStat._myJob; }
    }
    public int MyAge
    {
        get { return CharacterStat._myAge; }
    }
    public int Reputation
    {
        get { return CharacterStat._reputation; }
    }
    public string MyMapNumber
    {
        get { return CharacterStat._myMapNumber; }
    }
    public int ActivePoint
    {
        get { return CharacterStat._activePoint; }
    }
    public int Proficiency
    {
        get { return CharacterStat._proficiency; }
    }
    public List<string> MyItem
    {
        get { return CharacterStat._myItem; }
    }
    public List<int> MyItemCount
    {
        get { return CharacterStat._myItemCount; }
    }
    public int QuestItemOrder
    {
        get { return CharacterStat._questItemOrder; }
    }
    public int[] MyStack
    {
        get { return CharacterStat._myStack; }
    }
    public int StackOrder
    {
        get { return _stackOrder; }
    }
    public PlayerController MyPlayerController
    {
        get { return _myPlayerController; }
    }
    public ItemManager MyItemManager
    {
        get { return _myItemManager; }
    }
    public Vector2 CharacterPosition
    {
        get { return _characterPosition; }
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

        CharacterStat._myStack = new int[(int)Enum.GetValues(typeof(CharacterStatType)).Cast<CharacterStatType>().Last() - (int)CharacterStatType.Smith];
        CharacterStat._questItemOrder = 0;
        _stackOrder = 0;

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
    /// Type : 1 - MySocialClass, 2 - MyJob, 3 - MyAge, 4 - Reputation, 5 - MyPositon, 6 - ActivePoint, 7 - Proficiency, 8 - MyItem, 9~17 - MyStack(SocialClass / Job)
    /// <para>
    /// 직업 별 Type : 9 - 대장장이, 10 - 상인, 11 - 기사, 12 - 연금술사, 13 - 귀족, 14 - 왕, 15 - 평민, 16 - 백성, 17 - 귀족
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
            // MyPosition
            case CharacterStatType.MyPositon:
                CharacterStat._myMapNumber = StatTypeString;
                break;
            // ActivePoint
            case CharacterStatType.ActivePoint:
                Debug.Log("현재 ActivePoint = " + CharacterStat._activePoint);
                CharacterStat._activePoint += StatType;
                Debug.Log("변경된 ActivePoint = " + CharacterStat._activePoint);
                break;
            // Proficiency
            case CharacterStatType.Proficiency:
                CharacterStat._proficiency += StatType;
                break;
            // MyItem
            case CharacterStatType.MyItem:
                string ItemNumber = StatTypeString.Substring(0, 4);
                int ItemOrder = MyItemManager.OrderItem(ItemNumber);
                int ItemCount = 0;

                
                // 추가
                if (StatTypeString[4] != '-')
                {
                    ItemCount = int.Parse(StatTypeString.Substring((int)ItemType.Plus, (StatTypeString.Length - (int)ItemType.Plus)));

                    //Debug.Log("추가");
                    // 동일한 아이템이 없다면
                    if (!MyItemManager.IsExistItem(ItemNumber))
                    {
                        // 아이템 추가
                        //Debug.Log("아이템 추가 - " + ItemNumber);
                        CharacterStat._myItem.Insert(ItemOrder, ItemNumber);
                        // 개수 증가
                        //Debug.Log("개수 증가 - " + (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount.Insert(ItemOrder, ItemCount);
                        // 퀘스트아이템 위치 설정
                        if (ItemNumber[0] - '0' < 7)
                        {
                            CharacterStat._questItemOrder++;
                        }
                        else
                        {
                            //return;
                        }
                    }
                    // 동일한 아이템이 있다면
                    else
                    {
                        // 개수만 증가
                        //Debug.Log("해당 아이템 " + _myItemCount[ItemOrder] + "에서 " + (int)(StatTypeString[4] - '0') + "만큼 삭제");
                        //CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount[ItemOrder] += ItemCount;
                    }

                }
                // 삭제하기 전에 
                else
                {
                    ItemCount = int.Parse(StatTypeString.Substring((int)ItemType.Minus, (StatTypeString.Length - (int)ItemType.Minus)));

                    // 해당 아이템이 있는지 확인
                    if (CharacterStat._myItem[ItemOrder] == ItemNumber)
                    {
                        // 일부만 삭제
                        Debug.Log("가지고 있는 개수 " + CharacterStat._myItemCount[ItemOrder] + " 삭제할 개수 " + ItemCount);
                        if (CharacterStat._myItemCount[ItemOrder] > ItemCount)
                        {
                            //Debug.Log("해당 아이템 " + MyItemCount[ItemOrder] + "에서 " + (int)(StatTypeString[5] - '0') + "만큼 삭제");
                            CharacterStat._myItemCount[ItemOrder] = MyItemCount[ItemOrder] - ItemCount;
                        }
                        // 전체 삭제
                        else if (CharacterStat._myItemCount[ItemOrder] == ItemCount)
                        {
                            // 퀘스트아이템 위치 설정
                            if (CharacterStat._questItemOrder != 0)
                            {
                                // 일반 아이템의 경우
                                if (CharacterStat._myItem[ItemOrder][0] - '0' < 7)
                                {
                                    // 아이템 삭제
                                    //Debug.Log("해당 아이템 삭제 - " + _myItem[ItemOrder]);
                                    CharacterStat._myItem.RemoveAt(ItemOrder);
                                    // 개수 삭제
                                    //Debug.Log("해당 아이템 개수 삭제 - " + _myItemCount[ItemOrder]);
                                    CharacterStat._myItemCount.RemoveAt(ItemOrder);

                                    CharacterStat._questItemOrder--;
                                }
                                // 퀘스트 아이템이 추가된 경우
                                else
                                {
                                    // 아이템 삭제
                                    //Debug.Log("해당 아이템 삭제 - " + _myItem[ItemOrder]);
                                    CharacterStat._myItem.RemoveAt(ItemOrder);
                                    // 개수 삭제
                                    //Debug.Log("해당 아이템 개수 삭제 - " + _myItemCount[ItemOrder]);
                                    CharacterStat._myItemCount.RemoveAt(ItemOrder);

                                    //Debug.Log("_questItemOrder의 위치를 변경할 필요가 없음");
                                    return;
                                }
                            }
                            else
                            {
                                // 아이템 삭제
                                //Debug.Log("해당 아이템 삭제 - " + _myItem[ItemOrder]);
                                CharacterStat._myItem.RemoveAt(ItemOrder);
                                // 개수 삭제
                                //Debug.Log("해당 아이템 개수 삭제 - " + _myItemCount[ItemOrder]);
                                CharacterStat._myItemCount.RemoveAt(ItemOrder);

                                //Debug.Log("_questItemOrder의 위치를 변경할 필요가 없음");
                                return;
                            }
                        }
                        // 초과 삭제
                        else
                        {
                            Debug.Log("가지고 있는 개수보다 더 삭제하려고 함");
                        }
                    }
                    else
                    {
                        Debug.Log("없는 아이템을 삭제하려고 함");
                    }
                }
                break;
            // JobStack
            case CharacterStatType InputType when (InputType >= CharacterStatType.Smith):
                _stackOrder = ((int)Type - (int)CharacterStatType.Smith);
                CharacterStat._myStack[_stackOrder] += StatType;
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
        // 5. 위치 설정
        CharacterStat._myMapNumber = "0007";
        // 6. 활동력 설정
        CharacterStat._activePoint = 100;
        // 7. 스택 설정
        InitializeStack();
        // 8. 작업 속도 설정
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
        // 5. 위치 설정
        CharacterStat._myMapNumber = "0" + ((int)CharacterStat._myJob).ToString() + "02";

        // 7. 스택 설정
        InitializeStack();
        // 8. 작업 속도 설정
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
        for (int i = 0; i < CharacterStat._myStack.Length; i++)
        {
            CharacterStat._myStack[i] = 0;
        }
    }

    public void CheckStack()
    {
        for (int CheckStack = 0; CheckStack < CharacterStat._myStack.Length; CheckStack++)
        {
            //Debug.Log("MyStackBySocialClass[" + i + "] = " + MyStackBySocialClass[i]);
            if(CharacterStat._myStack[CheckStack] >= 50)
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
        transform.position = _characterPosition;
    }

    public void SetCharacterPosition()
    {
        if (_characterPosition == Vector2.zero)
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
            case "0009":
                _characterPosition.x = -7f;
                _characterPosition.y = 0f;
                break;

        }

        transform.position = _characterPosition;
        //Debug.Log("Set " + transform.position);
        _characterPosition.x = 0f;
        _characterPosition.y = 0f;
    }
}
