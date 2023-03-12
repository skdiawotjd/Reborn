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
    private bool questAcceptButton = true;

    protected override void Start()
    {
        base.Start();
        newPos = new Vector3(0f, 0f, 0f);
        temQuestPanel = new GameObject[10];
        mainQuestPanelText = MainQuestContentsPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    protected override void StartUI()
    {
        //Debug.Log("UIQuest ���� �� �ؾ��� �� ����");
        //questTextGenerate();
        LoadQuestList();
    }

    protected override void EndUI()
    {
        //Debug.Log("UIQuest ���� �� �ؾ��� �� ����");
    }
    public override void SetActivePanel()
    {
        if(!this.Panel.activeSelf)
        {
            Debug.Log("Q ������ ����Ʈâ ����.");
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
        if (questAcceptButton)
        {
            Debug.Log("����Ʈ ����");
            questButton.gameObject.SetActive(false);
            Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].itemNumber + 1);
            //Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "????");
            //UnityEngine.SceneManagement.SceneManager.LoadScene("JustChat");
        }
        else
        {
            QuestManager.instance.QuestClear(QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].itemNumber + QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].clearCount);
        }
    }
    public void LoadQuestList()
    {
        Debug.Log(QuestManager.instance.MyQuest.Count);
        for (int i = 0; i < QuestManager.instance.MyQuest.Count; i++)
        {
            
            Debug.Log("����Ʈ ���� : " + QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].job + " ���� ���� : " + Character.instance.MyJob.ToString());
            if (QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].job == Character.instance.MyJob.ToString())
            {
                mainQuestOrder = i;

                MainQuestContentsPanel.SetActive(true);
                mainQuestPanelText.text = QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].questContents;
                // ���� ����Ʈ �������� 0����� ���� ��ư Ȱ��ȭ
                // if (���� ����Ʈ �������� 0�����)
                if(Character.instance.MyItemManager.CountItem(QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].itemNumber) == 0)
                {
                    questButton.gameObject.SetActive(true);
                    questAcceptButton = true;
                    questButtonText.text = "����Ʈ ����";
                } else if (Character.instance.MyItemManager.CanDeleteItem(QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].itemNumber + "-" + Character.instance.MyItemManager.CountItem(QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].itemNumber)))
                {
                    // else if (���� ����Ʈ �����ۿ��� Ŭ���� ����, �� QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[mainQuestOrder]].clearCount�� �� �� �ִٸ�
                    // ��, if(Character.instance.MyItemManager.CanDeleteItem(itemNumber + clearCount)) �� ���̸�
                    questButton.gameObject.SetActive(true);
                    questAcceptButton = false;
                    questButtonText.text = "����Ʈ �Ϸ�";
                }
                else
                {
                    questButton.gameObject.SetActive(false);
                }

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
                // ���� ����Ʈ �г� ����
                temQuestPanel[i] = Instantiate(addSubQuestPanel, newPos, Quaternion.identity) as GameObject;
                subQuestPanelText = temQuestPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                subQuestPanelText.text = QuestManager.instance.MyQuest[QuestManager.instance.QuestOrder[i]].questContents;
                temQuestPanel[i].transform.SetParent(SubQuestPanel.transform);
            }
        }
    }
}
