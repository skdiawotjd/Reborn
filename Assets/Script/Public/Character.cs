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
    /// ���� �� Type : 9 - ��������, 10 - ����, 11 - ���, 12 - ���ݼ���, 13 - ����, 14 - ��, 15 - ���, 16 - �鼺, 17 - ����
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
                    Debug.Log("Reputation�� 100�� ����");
                }
                break;
            // MyPosition
            case CharacterStatType.MyPositon:
                CharacterStat._myMapNumber = StatTypeString;
                break;
            // ActivePoint
            case CharacterStatType.ActivePoint:
                Debug.Log("���� ActivePoint = " + CharacterStat._activePoint);
                CharacterStat._activePoint += StatType;
                Debug.Log("����� ActivePoint = " + CharacterStat._activePoint);
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

                
                // �߰�
                if (StatTypeString[4] != '-')
                {
                    ItemCount = int.Parse(StatTypeString.Substring((int)ItemType.Plus, (StatTypeString.Length - (int)ItemType.Plus)));

                    //Debug.Log("�߰�");
                    // ������ �������� ���ٸ�
                    if (!MyItemManager.IsExistItem(ItemNumber))
                    {
                        // ������ �߰�
                        //Debug.Log("������ �߰� - " + ItemNumber);
                        CharacterStat._myItem.Insert(ItemOrder, ItemNumber);
                        // ���� ����
                        //Debug.Log("���� ���� - " + (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount.Insert(ItemOrder, ItemCount);
                        // ����Ʈ������ ��ġ ����
                        if (ItemNumber[0] - '0' < 7)
                        {
                            CharacterStat._questItemOrder++;
                        }
                        else
                        {
                            //return;
                        }
                    }
                    // ������ �������� �ִٸ�
                    else
                    {
                        // ������ ����
                        //Debug.Log("�ش� ������ " + _myItemCount[ItemOrder] + "���� " + (int)(StatTypeString[4] - '0') + "��ŭ ����");
                        //CharacterStat._myItemCount.Insert(ItemOrder, (int)(StatTypeString[4] - '0'));
                        CharacterStat._myItemCount[ItemOrder] += ItemCount;
                    }

                }
                // �����ϱ� ���� 
                else
                {
                    ItemCount = int.Parse(StatTypeString.Substring((int)ItemType.Minus, (StatTypeString.Length - (int)ItemType.Minus)));

                    // �ش� �������� �ִ��� Ȯ��
                    if (CharacterStat._myItem[ItemOrder] == ItemNumber)
                    {
                        // �Ϻθ� ����
                        Debug.Log("������ �ִ� ���� " + CharacterStat._myItemCount[ItemOrder] + " ������ ���� " + ItemCount);
                        if (CharacterStat._myItemCount[ItemOrder] > ItemCount)
                        {
                            //Debug.Log("�ش� ������ " + MyItemCount[ItemOrder] + "���� " + (int)(StatTypeString[5] - '0') + "��ŭ ����");
                            CharacterStat._myItemCount[ItemOrder] = MyItemCount[ItemOrder] - ItemCount;
                        }
                        // ��ü ����
                        else if (CharacterStat._myItemCount[ItemOrder] == ItemCount)
                        {
                            // ����Ʈ������ ��ġ ����
                            if (CharacterStat._questItemOrder != 0)
                            {
                                // �Ϲ� �������� ���
                                if (CharacterStat._myItem[ItemOrder][0] - '0' < 7)
                                {
                                    // ������ ����
                                    //Debug.Log("�ش� ������ ���� - " + _myItem[ItemOrder]);
                                    CharacterStat._myItem.RemoveAt(ItemOrder);
                                    // ���� ����
                                    //Debug.Log("�ش� ������ ���� ���� - " + _myItemCount[ItemOrder]);
                                    CharacterStat._myItemCount.RemoveAt(ItemOrder);

                                    CharacterStat._questItemOrder--;
                                }
                                // ����Ʈ �������� �߰��� ���
                                else
                                {
                                    // ������ ����
                                    //Debug.Log("�ش� ������ ���� - " + _myItem[ItemOrder]);
                                    CharacterStat._myItem.RemoveAt(ItemOrder);
                                    // ���� ����
                                    //Debug.Log("�ش� ������ ���� ���� - " + _myItemCount[ItemOrder]);
                                    CharacterStat._myItemCount.RemoveAt(ItemOrder);

                                    //Debug.Log("_questItemOrder�� ��ġ�� ������ �ʿ䰡 ����");
                                    return;
                                }
                            }
                            else
                            {
                                // ������ ����
                                //Debug.Log("�ش� ������ ���� - " + _myItem[ItemOrder]);
                                CharacterStat._myItem.RemoveAt(ItemOrder);
                                // ���� ����
                                //Debug.Log("�ش� ������ ���� ���� - " + _myItemCount[ItemOrder]);
                                CharacterStat._myItemCount.RemoveAt(ItemOrder);

                                //Debug.Log("_questItemOrder�� ��ġ�� ������ �ʿ䰡 ����");
                                return;
                            }
                        }
                        // �ʰ� ����
                        else
                        {
                            Debug.Log("������ �ִ� �������� �� �����Ϸ��� ��");
                        }
                    }
                    else
                    {
                        Debug.Log("���� �������� �����Ϸ��� ��");
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
        // ���� ����
        // 1. �̸� ����
        CharacterStat._myName = "���ΰ�";
        // 2. ���/���� ����
        CharacterStat._mySocialClass = SocialClass.Helot;
        CharacterStat._myJob = Job.Slayer;
        // 3. ���� ����
        CharacterStat._myAge = 10;
        // 4. ���൵ ����
        CharacterStat._reputation = 0;
        // 5. ��ġ ����
        CharacterStat._myMapNumber = "0007";
        // 6. Ȱ���� ����
        CharacterStat._activePoint = 100;
        // 7. ���� ����
        InitializeStack();
        // 8. �۾� �ӵ� ����
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

        // 1. �̸� ����
        CharacterStat._myName = "���ΰ�";

        // 3. ���� ����
        CharacterStat._myAge = 10;
        // 4. ���൵ ����
        CharacterStat._reputation = 0;
        // 5. ��ġ ����
        CharacterStat._myMapNumber = "0" + ((int)CharacterStat._myJob).ToString() + "02";

        // 7. ���� ����
        InitializeStack();
        // 8. �۾� �ӵ� ����
        CharacterStat.MyWorkSpeed = 1.0f;
    }

    public void InitializeCharacter()
    {
        //InitializeMapNumber
        CharacterStat._myMapNumber = "0" + ((int)CharacterStat._myJob).ToString() + "02";
        // 3. ���� ����
        CharacterStat._myAge = 10;
        // 6. Ȱ���� ����
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
