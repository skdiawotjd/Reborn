using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    private List<string> MainQuest;
    private List<string> SubQuest;
    List<Dictionary<string, object>> TodoNumberlist;
    private string TodoMapNumber;

    private List<Dictionary<string, object>> UniqueQuestList;
    public string todayQuest;
    public string questDeleteNumber;
    public bool questEnd;
    private int temNumber;
    private QuestUIManager QUIManager;

    /*public int questType
    {
        get
        {
            return _questType;
        }
    }
    public int QuestLevel
    {
        get
        {
            return _questLevel;
        }
    }*/

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
        GiveQuest();
        QUIManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(5).GetComponent<QuestUIManager>();
    }

    void Start()
    {
        MainQuest = new List<string>();
        SubQuest = new List<string>();
        

        QUIManager.questTextGenerate();
        GameManager.instance.DayStart.AddListener(GiveQuest);
    }

    private void GiveQuest()
    {
        temNumber = Random.Range(0, 2) * 2;
        //Debug.Log("temNumber : " + temNumber);
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

    public void QuestClear(int type, bool clear)
    {
        // 퀘스트를 클리어하고 부를 함수
        Debug.Log("ActivePoint 10 감소하기 전, 현재 수치 : " + Character.instance.ActivePoint);
        switch(TodoMapNumber)
        {
            case "0002":
            case "0005":
            case "0105":
                Character.instance.SetCharacterStat(7, -10); // 활동력 -10
                break;
            case "0003":
            case "0205":
                Character.instance.SetCharacterStat(7, -20);
                break;
        }
        
        Debug.Log("ActivePoint 10 감소, 현재 수치 : " + Character.instance.ActivePoint);

        if (clear)
        {
            Debug.Log("퀘스트 성공");
            switch(Character.instance.MyJob.ToString())
            {
                case "Slayer":
                    switch(Character.instance.MyPosition)
                    {
                        case "0002": // ex) 직업 Slayer의 전용퀘스트일 경우
                        case "0005":
                        case "0105":
                            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                            break;
                        default:
                            Character.instance.SetCharacterStat(type + 9, 2); // 스택 업
                            break;
                    }
                    break;
                case "Smith":
                    switch (Character.instance.MyPosition)
                    {
                        case "0003": 
                        case "0104":
                        case "0208":
                            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                            break;
                        default:
                            Character.instance.SetCharacterStat(type + 9, 2); // 스택 업
                            break;
                    }
                    break;
                case "bania":
                    switch (Character.instance.MyPosition)
                    {
                        case "0004": 
                        case "0108":
                        case "0205":
                            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                            break;
                        default:
                            Character.instance.SetCharacterStat(type + 9, 2); // 스택 업
                            break;
                    }
                    break;
                case "MasterSmith":
                    switch (Character.instance.MyPosition)
                    {
                        case "0007": 
                        case "0102":
                        case "0103":
                            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                            break;
                        default:
                            Character.instance.SetCharacterStat(type + 9, 2); // 스택 업
                            break;
                    }
                    break;
                case "Merchant":
                    switch (Character.instance.MyPosition)
                    {
                        case "0202": 
                        case "0305":
                        case "0107":
                            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                            break;
                        default:
                            Character.instance.SetCharacterStat(type + 9, 2); // 스택 업
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("퀘스트 실패");
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
}

