using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public List<string> QuestOrder;
    public Dictionary<string, Quest> MyQuest;
    private Quest temQuest;
    public bool questChanges;

    private List<Dictionary<string, object>> UniqueQuestList;
    private List<Dictionary<string, object>> QuestNumberList;
    public string todayQuest;
    public string questDeleteNumber;
    public bool questEnd;
    public bool subQuestStart = false;
    public bool moveBG;
    private string itemNumberString;
    private string itemNumberChar;
    private string nextQuestString;
    private string[] rewards;

    // 모험 게임의 난이도
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
        QuestOrder = new List<string>();
        MyQuest = new Dictionary<string, Quest>();
        Character.instance.SetCharacterStat(CharacterStatType.MyItem, "70000");
        QuestLoad();
    }

    private void QuestLoad()
    {
        Debug.Log("퀘스트 로드 함수 진입" + Character.instance.MyItem.Count);
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
    public void AddQuest(string QuestObjectNumber) // 게임 진행 중 따로 추가를 할 때
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
    public void RemoveQuest(string number) // 퀘스트 클리어 혹은 제거
    {
        MyQuest.Remove(number);
        QuestOrder.Remove(number);
        questChanges = true;
    }
    public void MinigameClear(bool clear)
    {
        if (clear)
        {
            Debug.Log("퀘스트 성공");
            switch (Character.instance.MyJob.ToString())
            {
                case "Slayer":
                    switch (Character.instance.MyMapNumber)
                    {
                        case "0003":
                        case "0004":
                        case "0008":
                        case "0009":
                            Debug.Log("퀘스트 성공. 변경 전 TP : " + Character.instance.Reputation);
                            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 50); // todoProgress + 20
                            Debug.Log("TodoProgress +2. 현재 TP : " + Character.instance.Reputation);
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
            Debug.Log("퀘스트 실패");
        }
        // 퀘스트를 클리어하고 부를 함수
        switch(Character.instance.MySocialClass)
        {
            case SocialClass.Helot:
                switch (Character.instance.MyMapNumber.Substring(2, 2))
                {
                    case "03": // DDR일 때 ex) 주조
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -2); // 활동력 -2
                        break;
                    case "04": // 타이밍일 때 ex) 제작
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -3); // 활동력 -3
                        break;
                    case "05": // 퀴즈일 때
                        break;
                    case "08": // 탐험일 때
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -2); // 활동력 -2
                        break;
                    case "09": // 모험일 때
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -3); // 활동력 -3
                        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0013");
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                        break;
                }
                break;
            case SocialClass.Commons: // 내 계급이 평민이라면
                switch(Character.instance.MyMapNumber.Substring(2,2))
                {
                    case "03": // DDR일 때 ex) 주조
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -2); // 활동력 -2
                        break;
                    case "04": // 타이밍일 때 ex) 제작
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -3); // 활동력 -3
                        break;
                    case "05": // 퀴즈일 때
                        break;
                    case "08": // 탐험일 때
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -2); // 활동력 -2
                        break;
                    case "09": // 모험일 때
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -3); // 활동력 -3
                        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0013");
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                        break;
                }
                break;
        }
        Debug.Log("ActivePoint 감소, 현재 수치 : " + Character.instance.ActivePoint);

        
    }
    private void GetReward(string itemNumberString)
    {
        Debug.Log("겟 리워드 진입");
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
            Debug.Log("보상 획득 : " + rewards[j].Substring(0,4) + " 아이템 " + rewards[j][4] + "개 획득");
        }
        if(Character.instance.Reputation >= 100) // 평판이 100이거나 100을 넘었다면
        {
            // 스택 증가
            switch(Character.instance.MySocialClass)
            {
                case SocialClass.Commons:
                    Character.instance.SetCharacterStat(CharacterStatType.Knight, 10);
                    break;
                case SocialClass.SemiNoble:
                    Character.instance.SetCharacterStat(CharacterStatType.Noble, 10);
                    break;
                case SocialClass.Noble:
                    Character.instance.SetCharacterStat(CharacterStatType.King, 10);
                    break;
                case SocialClass.King:
                    Character.instance.SetCharacterStat(CharacterStatType.King, 10);
                    break;
            }
            
        }
        else // 평판이 100 이하라면
        {
            // 평판 증가
            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 13);
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
                    // 메인 퀘스트 클리어
                    // 메인 퀘스트 아이템 제거
                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, itemNumberString.Substring(0, 4) + "-" + itemNumberString.Substring((int)ItemType.Plus, (itemNumberString.Length - (int)ItemType.Plus)));
                    //Character.instance.SetCharacterStat(CharacterStatType.MyItem, itemNumberString.Substring(0, 4) + "-" + itemNumberString.Substring(4, (itemNumberString.Length - 4)));
                    //Character.instance.SetCharacterStat(CharacterStatType.MyItem, itemNumberString.Substring(0, 4) + "-" + itemNumberString[4]);
                    RemoveQuest(itemNumberString.Substring(0, 4));
                    // 메인 퀘스트 보상 수령
                    Character.instance.SetCharacterStat(CharacterStatType.Reputation, 20);
                    GetReward(itemNumberString);
                    // 다음 메인 퀘스트 아이템 획득
                    nextQuestString = (int.Parse(itemNumberString.Substring(0,4)) + 1).ToString() + "0";
                    Debug.Log("nextQuestString : " + nextQuestString);
                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, nextQuestString);
                    AddQuest(nextQuestString.Substring(0,4));
                } else
                {
                    // 서브 퀘스트 클리어
                    Character.instance.SetCharacterStat(CharacterStatType.Proficiency, 1);
                }

            }
        }
    }


    public void BoxCount()
    {
        EventCountChange.Invoke();
    }
    public void ChangeMoveBG(bool move) // 탐험에서 배경이 움직일 지 안 움직일지를 판단하는 변수를 바꿔준다.
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
