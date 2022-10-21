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
        Debug.Log("temNumber : " + temNumber);
        Debug.Log(UniqueQuestList[temNumber][Character.instance.MyJob.ToString()].ToString());
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
        // ����Ʈ�� Ŭ�����ϰ� �θ� �Լ�
        Debug.Log("ActivePoint 10 �����ϱ� ��, ���� ��ġ : " + Character.instance.ActivePoint);
        switch(TodoMapNumber)
        {
            case "0002":
            case "0005":
            case "0105":
                Character.instance.SetCharacterStat(7, -10); // Ȱ���� -10
                break;
            case "0003":
            case "0205":
                Character.instance.SetCharacterStat(7, -20);
                break;
        }
        
        Debug.Log("ActivePoint 10 ����, ���� ��ġ : " + Character.instance.ActivePoint);

        if (clear)
        {
            Debug.Log("����Ʈ ����");
            switch(Character.instance.MyJob.ToString())
            {
                case "Slayer":
                    switch(Character.instance.MyPosition)
                    {
                        case "0001": // ex) ���� Slayer�� ��������Ʈ�� �ƴ�, �߰� ����Ʈ�� ���
                        case "0002":
                        case "0003":
                            Character.instance.SetCharacterStat(type + 9, 2); // ���� ��
                            break;
                        default:
                            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                            break;
                    }
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
}

