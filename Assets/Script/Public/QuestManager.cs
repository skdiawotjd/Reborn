using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    private List<string> MainQuest;
    public List<string> SubQuest;
    public Dictionary<string, Quest> MyQuest;
    private Quest temQuest;
    private List<string> MyQuest2;
    private List<string> MyQuestOrder;
    public bool questChanges;
    List<Dictionary<string, object>> TodoNumberlist;
    private string TodoMapNumber;

    private List<Dictionary<string, object>> UniqueQuestList;
    private List<Dictionary<string, object>> QuestNumberList;
    public string todayQuest;
    public string questDeleteNumber;
    public bool questEnd;
    public bool subQuestStart = false;
    public bool moveBG;
    private int temNumber;
    private string itemNumberString;
    private string itemNumberChar;
    //private QuestUIManager QUIManager;

    public static QuestManager instance = null;
    public UnityEvent EventCountChange;

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
        UniqueQuestList = CSVReader.Read("UniqueQuest");
        QuestNumberList = CSVReader.Read("QuestNumberList");
        temQuest = new Quest("0", "0", "0");
        GiveQuest();
        questChanges = false;
        //QUIManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(4).GetComponent<QuestUIManager>();
        moveBG = true;
    }

    void Start()
    {
        MainQuest = new List<string>();
        SubQuest = new List<string>();
        MyQuest = new Dictionary<string, Quest>();
        //QUIManager.questTextGenerate();
        GameManager.instance.AddDayStart(GiveQuest);
        QuestLoad();
    }

    private void GiveQuest()
    {
        temNumber = UnityEngine.Random.Range(0, 2) * 2;
        //Debug.Log("temNumber : " + temNumber + ", Job : " + Character.instance.MyJob.ToString());
        //Debug.Log(UniqueQuestList[temNumber][Character.instance.MyJob.ToString()].ToString());
        todayQuest = UniqueQuestList[temNumber][Character.instance.MyJob.ToString()].ToString();
        questDeleteNumber = UniqueQuestList[temNumber + 1][Character.instance.MyJob.ToString()].ToString();
        questEnd = false;
        
    }
    public void QuestGive()
    {
        TodoNumberlist = CSVReader.Read("QuestNumber");
        
        for(int i = 1; i <= int.Parse(string.Format("{0}", TodoNumberlist[0][Character.instance.MyJob.ToString()])); i++)
        {
            Debug.Log(Character.instance.MyJob.ToString() + " " + TodoNumberlist[i][Character.instance.MyJob.ToString()]);
            MainQuest.Add(TodoNumberlist[i][Character.instance.MyJob.ToString()].ToString());
        }
        for (int i = 1; i <= int.Parse(string.Format("{0}", TodoNumberlist[0]["Add" + Character.instance.MyJob.ToString()])); i++)
        {
            Debug.Log("Add" + Character.instance.MyJob.ToString() + " " + TodoNumberlist[i]["Add" + Character.instance.MyJob.ToString()]);
            SubQuest.Add(TodoNumberlist[i][Character.instance.MyJob.ToString()].ToString());
        }
    }

    private void QuestLoad()
    {
        Debug.Log("����Ʈ �ε� �Լ� ����" + Character.instance.MyItem.Count);
        for(int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            itemNumberString = Character.instance.MyItem[i].Substring(0, 4);
            itemNumberChar = itemNumberString.Substring(0,1);
            if(int.Parse(itemNumberChar) >= 7)
            {
                AddQuest();
            }
        }
    }
    private int AddQuest() // ���� ���� �� ó�� �ε��� ��
    {
        for (int j = 0; j < QuestNumberList.Count; j++)
        {
            if (itemNumberString == QuestNumberList[j]["ItemNumber"].ToString())
            {
                temQuest.itemNumber = itemNumberString;
                temQuest.questNumber = QuestNumberList[j]["QuestNumber"].ToString();
                temQuest.questContents = QuestNumberList[j]["QuestContents"].ToString();
                MyQuest.Add(itemNumberString, temQuest);
                SubQuest.Add(itemNumberString);
                //MyQuestOrder.Add(QuestNumberList[j]["Number"].ToString());
                //MyQuest.Add(QuestNumberList[j]["QuestContents"].ToString());
                return 1;
            }
        }
        return 0;
    }
    public void AddQuest(string number) // ���� ���� �� ���� �߰��� �� ��
    {
        for (int j = 0; j < QuestNumberList.Count; j++)
        {
            if (number == QuestNumberList[j]["ItemNumber"].ToString())
            {
                temQuest.itemNumber = number;
                temQuest.questNumber = QuestNumberList[j]["QuestNumber"].ToString();
                temQuest.questContents = QuestNumberList[j]["QuestContents"].ToString();
                MyQuest.Add(number, temQuest);
                SubQuest.Add(number);
                questChanges = true;
            }
        }
    }
    public void RemoveQuest(string number) // ����Ʈ Ŭ���� Ȥ�� ����
    {
        MyQuest.Remove(number);
        questChanges = true;
    }
    public void QuestClear(bool clear)
    {
        // ����Ʈ�� Ŭ�����ϰ� �θ� �Լ�
        Debug.Log("ActivePoint 10 �����ϱ� ��, ���� ��ġ : " + Character.instance.ActivePoint);
        switch(Character.instance.MyMapNumber)
        {
            // �뿹
            case "0004":
            case "0008":
            case "0009":
                Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -20);
                break;
            // ��������
            case "0003":
            case "0104":
            case "0005":
            case "0108":
            case "0109":
            // ����
            case "0103":
            case "0204":
            case "0105":
            case "0208":
            case "0209":
                Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -10); // Ȱ���� -10
                break;
        }
        
        Debug.Log("ActivePoint 10 ����, ���� ��ġ : " + Character.instance.ActivePoint);

        if (clear)
        {
            Debug.Log("����Ʈ ����");
            switch(Character.instance.MyJob.ToString())
            {
                case "Slayer":
                    switch(Character.instance.MyMapNumber)
                    {
                        case "0004":
                        case "0008":
                        case "0009":
                            Debug.Log("����Ʈ ����. ���� �� TP : " + Character.instance.Reputation);
                            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 20); // todoProgress + 20
                            Debug.Log("TodoProgress +2. ���� TP : " + Character.instance.Reputation);
                            break;
                    }
                    break;
                case "Smith":
                    switch (Character.instance.MyMapNumber)
                    {
                        case "0003":
                        case "0104":
                        case "0005":
                        case "0108":
                        case "0109":
                            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 10); // todoProgress + 10
                            break;
                    }
                    break;
                case "bania":
                    switch (Character.instance.MyMapNumber)
                    {
                        case "0103":
                        case "0204":
                        case "0105":
                        case "0208":
                        case "0209":
                            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 10); // todoProgress + 2
                            break;
                    }
                    break;
                case "Knight":
                case "Scholar":
                case "LowNobility":
                case "MiddleNobility":
                case "HighNobility":
                case "King":
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("����Ʈ ����");
        }
    }


    public void BoxCount()
    {
        EventCountChange.Invoke();
    }

    public void SetPreMapNumber(string PreMapNumber)
    {
        TodoMapNumber = PreMapNumber;
    }

    public bool CompareMapNumber(string MapNumber)
    {
        for(int i = 0; i < MainQuest.Count; i++)
        {
            if(MainQuest[i] == MapNumber)
                return true;
        }
        for (int i = 0; i < SubQuest.Count; i++)
        {
            if (SubQuest[i] == MapNumber)
                return true;
        }
        return false;
    }
    public void ChangeMoveBG(bool move) // Ž�迡�� ����� ������ �� �� ���������� �Ǵ��ϴ� ������ �ٲ��ش�.
    {
        moveBG = move;
    }
}
public class Quest {
    public string itemNumber;
    public string questNumber;
    public string questContents;

    public Quest(string _itemNumber, string _questNumber, string _questContents)
    {
        this.itemNumber = _itemNumber;
        this.questContents = _questContents;
        this.questNumber = _questNumber;
    }
}
