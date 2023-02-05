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
    private GameObject addSubQuestPanel;
    [SerializeField]
    private GameObject MainQuestContentsPanel;
    [SerializeField]
    private Image lockImage;
    [SerializeField]
    private Button questButton;
    [SerializeField]
    private TextMeshProUGUI questButtonText;
    private GameObject[] temQuestPanel;
    private TextMeshProUGUI subQuestPanelText;
    private TextMeshProUGUI mainQuestPanelText;
    private int mainQuestOrder;
    private Vector3 newPos;

    protected override void Start()
    {
        base.Start();
        newPos = new Vector3(0f, 0f, 0f);
        temQuestPanel = new GameObject[10];
        mainQuestPanelText = MainQuestContentsPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
        for(int i = 0; i < temQuestPanel.Length; i++)
        {
            if (temQuestPanel[i] != null)
            {
                Destroy(temQuestPanel[i].gameObject);
            }
        }
        LoadQuestList();
    }
    public void QuestAccept()
    {
        Debug.Log("퀘스트 수락");
        questButton.gameObject.SetActive(false);
        Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].itemNumber + "1");
        //Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "????");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("JustChat");
    }
    public void LoadQuestList()
    {
        Debug.Log(QuestManager.instance.MyQuest.Count);
        for (int i = 0; i < QuestManager.instance.MyQuest.Count; i++)
        {
            
            Debug.Log("퀘스트 직업 : " + QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].job + " 현재 직업 : " + Character.instance.MyJob.ToString());
            if (QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].job == Character.instance.MyJob.ToString())
            {
                mainQuestOrder = i;
                // 메인 퀘스트 패널 생성
                Debug.Log("메인 퀘스트 패널 생성");
                //temQuestPanel[index] = Instantiate(addMainQuestPanel, newPos, Quaternion.identity) as GameObject;
                //temQuestPanel[index].transform.SetParent(MainQuestPanel.transform);
                //temQuestPanel[index].transform.position = newPos;

                MainQuestContentsPanel.SetActive(true);
                mainQuestPanelText.text = QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].questContents;
                // 메인 퀘스트 아이템이 0개라면 수락 버튼 활성화
                // if (메인 퀘스트 아이템이 0개라면)
                questButton.gameObject.SetActive(true);
                questButtonText.text = "퀘스트 수락";
                // else if (메인 퀘스트 아이템에서 클리어 개수, 즉 QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].clearCount를 뺄 수 있다면
                // 즉, if(Character.instance.MyItemManager.CanDeleteItem(itemNumber + clearCount)) 가 참이면
                // questButton.gameObject.SetActive(true);
                // questButtonText.text = "퀘스트 완료";
                // else
                //   questButton.gameObject.SetActive(false);


                if (QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].proficiency > Character.instance.Proficiency)
                {
                    MainQuestContentsPanel.SetActive(false);
                    lockImage.gameObject.SetActive(true);
                }
                else
                {
                    MainQuestContentsPanel.SetActive(true);
                    lockImage.gameObject.SetActive(false);
                }
            }
            else
            {
                // 서브 퀘스트 패널 생성
                temQuestPanel[i] = Instantiate(addSubQuestPanel, newPos, Quaternion.identity) as GameObject;
                subQuestPanelText = temQuestPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                subQuestPanelText.text = QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].questContents;
                temQuestPanel[i].transform.SetParent(SubQuestPanel.transform);
            }
        }
    }
}
