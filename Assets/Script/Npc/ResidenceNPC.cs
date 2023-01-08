using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResidenceNPC : BasicNpc
{
    [SerializeField]
    private TextMeshPro QusetStateText;
    [SerializeField]
    private List<string> QuestDataList;

    [SerializeField]
    private QuestState _residenceqQuestState;
    

    public QuestState ResidenceqQuestState
    {
        set
        {
            _residenceqQuestState = value;
            SettingQusetState();
        }
        get
        {
            return _residenceqQuestState;
        }
    }

    protected override void Start()
    {
        base.Start();
        ShopNpcSetting();
    }


    protected override void FunctionStart()
    {
        switch (_residenceqQuestState)
        {
            case QuestState.Start:
                Debug.Log("대화 시작 - 퀘스트 받기");
                SettingQuest();
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.Progress:
                Debug.Log("대화 시작 - 퀘스트 중");
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.End:
                Debug.Log("대화 시작 - 퀘스트 완료");
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
        }
    }

    public override void FunctionEnd()
    {
        switch (_residenceqQuestState)
        {
            case QuestState.Start:
                Debug.Log("대화 끝 - 퀘스트 중으로 이동");
                ResidenceqQuestState = QuestState.Progress;
                break;
            case QuestState.Progress:
                Debug.Log("대화 끝 - 퀘스트 끝으로 이동");
                ResidenceqQuestState = QuestState.End;
                break;
            case QuestState.End:
                Debug.Log("대화 끝 - 퀘스트 대기로 이동");
                ResidenceqQuestState = QuestState.Stand;
                break;
        }
    }

    IEnumerator ads()
    {
        while(ConversationManager.IsCanChat)
        {
            yield return new WaitForSeconds(0.01f);
            ResidenceqQuestState = QuestState.Start;
        }
    }

    private void ShopNpcSetting()
    {
        Debug.Log("QuestDataList 세팅");
        SetNpcNumber(0);
        SetChatType(0);

        QuestDataList[(int)QuestData.QuestNumber] = "1234";
        QuestDataList[(int)QuestData.QuestObjectNumber] = "5678";
        QuestDataList[(int)QuestData.ClearCount] = "3";

        
        for (int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            if(Character.instance.MyItem[i] == QuestDataList[(int)QuestData.QuestObjectNumber])
            {
                
                Debug.Log("퀘스트를 받은 적이 있음");
                if (Character.instance.MyItemCount[i] == int.Parse(QuestDataList[(int)QuestData.ClearCount]))
                {
                    Debug.Log("퀘스트오브젝트를 전부 가지고 있음");
                    ResidenceqQuestState = QuestState.End;
                    return;
                }
                ResidenceqQuestState = QuestState.Progress;
                return;
            }
        }
        Debug.Log("퀘스트를 받은 적이 없음");
    }

    private void SettingQusetState()
    {
        switch(_residenceqQuestState)
        {
            case QuestState.Start:
                QusetStateText.text = "?";
                break;
            case QuestState.Progress:
                QusetStateText.text = "...";
                break;
            case QuestState.End:
                QusetStateText.text = "!";
                break;
            case QuestState.Stand:
                QusetStateText.text = "";
                break;
        }
    }

    private void SettingQuest()
    {
        Debug.Log("퀘스트 수주");
    }
}
