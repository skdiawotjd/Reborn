using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    private List<string> MainQuest;
    public List<string> QuestOrder;
    public Dictionary<string, Quest> MyQuest;
    private Quest temQuest;
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
    private string nextQuestString;
    private string[] rewards;

    // ���� ������ ���̵�
    private int adventureLevel;

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
        temQuest = new Quest("0", "0", "0", "0", 0, 0);
        questChanges = false;
        moveBG = true;
    }

    void Start()
    {
        MainQuest = new List<string>();
        QuestOrder = new List<string>();
        MyQuest = new Dictionary<string, Quest>();
        Character.instance.SetCharacterStat(CharacterStatType.MyItem, "70000");
        QuestLoad();
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
                AddQuest(itemNumberString);
            }
        }
    }
    public void AddQuest(string QuestObjectNumber) // ���� ���� �� ���� �߰��� �� ��
    {
        for (int j = 0; j < QuestNumberList.Count; j++)
        {
            if (QuestObjectNumber == QuestNumberList[j]["ItemNumber"].ToString())
            {
                temQuest.itemNumber = QuestObjectNumber;
                temQuest.questNumber = QuestNumberList[j]["QuestNumber"].ToString();
                temQuest.questContents = QuestNumberList[j]["QuestContents"].ToString();
                temQuest.job = QuestNumberList[j]["Job"].ToString();
                temQuest.clearCount = int.Parse(QuestNumberList[j]["ClearCount"].ToString());
                temQuest.proficiency = int.Parse(QuestNumberList[j]["Proficiency"].ToString());
                MyQuest.Add(QuestObjectNumber, temQuest);
                QuestOrder.Add(QuestObjectNumber);
                questChanges = true;
                return;
            }
        }
    }
    public void RemoveQuest(string number) // ����Ʈ Ŭ���� Ȥ�� ����
    {
        MyQuest.Remove(number);
        QuestOrder.Remove(number);
        questChanges = true;
    }
    public void MinigameClear(bool clear)
    {
        // ����Ʈ�� Ŭ�����ϰ� �θ� �Լ�
        switch(Character.instance.MyMapNumber)
        {
            // �뿹
            case "0003":
            case "0004":
            case "0008":
            case "0009":
                Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -20);
                break;
            // ��������
            case "0103":
            case "0104":
            case "0005":
            case "0108":
            case "0109":
            // ����
            case "0203":
            case "0204":
            case "0105":
            case "0208":
            case "0209":
                Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -10); // Ȱ���� -10
                break;
        }
        Debug.Log("ActivePoint ����, ���� ��ġ : " + Character.instance.ActivePoint);

        if (clear)
        {
            Debug.Log("����Ʈ ����");
            switch(Character.instance.MyJob.ToString())
            {
                case "Slayer":
                    switch(Character.instance.MyMapNumber)
                    {
                        case "0003":
                        case "0004":
                        case "0008":
                        case "0009":
                            Debug.Log("����Ʈ ����. ���� �� TP : " + Character.instance.Reputation);
                            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 50); // todoProgress + 20
                            Debug.Log("TodoProgress +2. ���� TP : " + Character.instance.Reputation);
                            break;
                    }
                    break;
                case "Smith":
                    switch (Character.instance.MyMapNumber)
                    {
                        case "0103":
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
                        case "0203":
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
    private void GetReward(string itemNumberString)
    {
        Debug.Log("�� ������ ����");
        for (int i = 0; i < QuestNumberList.Count; i++)
        {
            if (itemNumberString.Substring(0,4) == QuestNumberList[i]["ItemNumber"].ToString())
            {
                rewards = QuestNumberList[i]["Reward"].ToString().Split(',');
            }
        }
        for(int j = 0; j < rewards.Length; j++)
        {
            Character.instance.SetCharacterStat(CharacterStatType.MyItem, rewards[j]);
            Debug.Log("���� ȹ�� : " + rewards[j].Substring(0,4) + " ������ " + rewards[j][4] + "�� ȹ��");
        }
    }
    public void QuestClear(string itemNumberString)
    {
        for(int i = 0; i < MyQuest.Count; i++)
        {
            if(MyQuest[QuestOrder[i]].itemNumber == itemNumberString.Substring(0,4))
            {
                Debug.Log(itemNumberString);
                Debug.Log(itemNumberString[0]);
                Debug.Log(itemNumberString.Length);
                Debug.Log(itemNumberString.Substring(4, (itemNumberString.Length - 4)));
                if (itemNumberString[0] == '7')
                {
                    // ���� ����Ʈ Ŭ����
                    // ���� ����Ʈ ������ ����
                    //Character.instance.SetCharacterStat(CharacterStatType.MyItem, itemNumberString.Substring(0, 4) + "-" + itemNumberString.Substring((int)ItemType.Plus, (itemNumberString.Length - (int)ItemType.Plus)));
                    //Character.instance.SetCharacterStat(CharacterStatType.MyItem, itemNumberString.Substring(0, 4) + "-" + itemNumberString.Substring(4, (itemNumberString.Length - 4)));
                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, itemNumberString.Substring(0, 4) + "-" + itemNumberString[4]);
                    RemoveQuest(itemNumberString.Substring(0, 4));
                    // ���� ����Ʈ ���� ����
                    Character.instance.SetCharacterStat(CharacterStatType.Reputation, 20);
                    GetReward(itemNumberString);
                    // ���� ���� ����Ʈ ������ ȹ��
                    nextQuestString = (int.Parse(itemNumberString.Substring(0,4)) + 1).ToString() + "0";
                    Debug.Log("nextQuestString : " + nextQuestString);
                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, nextQuestString);
                    AddQuest(nextQuestString.Substring(0,4));
                } else
                {
                    // ���� ����Ʈ Ŭ����
                    Character.instance.SetCharacterStat(CharacterStatType.Proficiency, 1);
                }

            }
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
        /*for (int i = 0; i < SubQuest.Count; i++)
        {
            if (SubQuest[i] == MapNumber)
                return true;
        }*/
        return false;
    }
    public void ChangeMoveBG(bool move) // Ž�迡�� ����� ������ �� �� ���������� �Ǵ��ϴ� ������ �ٲ��ش�.
    {
        moveBG = move;
    }
    public void SetAdventureLevel(int level)
    {
        adventureLevel = level;
    }
    public int GetAdventureLevel()
    {
        return adventureLevel;
    }
}
public class Quest {
    public string itemNumber;
    public string questNumber;
    public string questContents;
    public string job;
    public int clearCount;
    public int proficiency;

    public Quest(string _itemNumber, string _questNumber, string _questContents, string _job, int _count, int _proficiency)
    {
        this.itemNumber = _itemNumber;
        this.questContents = _questContents;
        this.questNumber = _questNumber;
        this.job = _job;
        this.clearCount = _count;
        this.proficiency = _proficiency;
    }
}
