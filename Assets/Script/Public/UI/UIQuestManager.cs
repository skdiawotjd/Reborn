using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestManager : UIManager
{
    [SerializeField]
    private Object questText;
    [SerializeField]
    private GameObject MainQuestPanel;
    [SerializeField]
    private GameObject SubQuestPanel;
    [SerializeField]
    private GameObject addQuestPanel;
    private GameObject[] temQuestPanel;
    private TextMeshProUGUI QuestPanelText;
    private Vector3 newPos;

    protected override void Start()
    {
        base.Start();
        newPos = new Vector3(0f, 0f, 0f);
        temQuestPanel = new GameObject[10];
    }

    protected override void StartUI()
    {
        //Debug.Log("UIQuest 시작 시 해야할 일 실행");
        //questTextGenerate();
        LoadQuestList();
    }

    protected override void EndUI()
    {
        //Debug.Log("UIQuest 끝날 시 해야할 일 실행");
    }
    public override void SetActivePanel()
    {
        if(!this.Panel.activeSelf)
        {
            Debug.Log("Q 눌러서 퀘스트창 오픈.");
            if (QuestManager.instance.questChanges)
            {
                ReloadQuestList();
            }
        }
        base.SetActivePanel();
    }
    public void ReloadQuestList()
    {
        Debug.Log("퀘스트 재생성 함수 진입" + QuestManager.instance.MyQuest.Count);
        for(int i = 0; i < temQuestPanel.Length; i++)
        {
            if (temQuestPanel[i] != null)
            {
                Destroy(temQuestPanel[i].gameObject);
            }
        }
        for (int i = 0; i < QuestManager.instance.MyQuest.Count; i++)
        {
            temQuestPanel[i] = Instantiate(addQuestPanel, newPos, Quaternion.identity) as GameObject;
            QuestPanelText = temQuestPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            QuestPanelText.text = QuestManager.instance.MyQuest[QuestManager.instance.SubQuest[i]].questContents;
            temQuestPanel[i].transform.SetParent(SubQuestPanel.transform);
        }
    }
    public void LoadQuestList()
    {
        Debug.Log("퀘스트 생성 함수 진입" + QuestManager.instance.MyQuest.Count);
        for(int i=0; i < QuestManager.instance.MyQuest.Count; i++)
        {
            temQuestPanel[i] = Instantiate(addQuestPanel, newPos, Quaternion.identity) as GameObject;
            QuestPanelText = temQuestPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            QuestPanelText.text = QuestManager.instance.MyQuest[QuestManager.instance.SubQuest[i]].questContents;
            temQuestPanel[i].transform.SetParent(SubQuestPanel.transform);
        }
    }

/*    public void questTextGenerate()
    {
        GameObject a = Instantiate(questText, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        MainText = a.GetComponent<TextMeshProUGUI>();

        MainText.text = QuestManager.instance.todayQuest;
        MainText.transform.SetParent(Panel.transform);
    }*/
}
