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
                Debug.Log("��ȭ ���� - ����Ʈ �ޱ�");
                SettingQuest();
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.Progress:
                Debug.Log("��ȭ ���� - ����Ʈ ��");
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.End:
                Debug.Log("��ȭ ���� - ����Ʈ �Ϸ�");
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
                Debug.Log("��ȭ �� - ����Ʈ ������ �̵�");
                ResidenceqQuestState = QuestState.Progress;
                break;
            case QuestState.Progress:
                Debug.Log("��ȭ �� - ����Ʈ ������ �̵�");
                ResidenceqQuestState = QuestState.End;
                break;
            case QuestState.End:
                Debug.Log("��ȭ �� - ����Ʈ ���� �̵�");
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
        Debug.Log("QuestDataList ����");
        SetNpcNumber(0);
        SetChatType(0);

        QuestDataList[(int)QuestData.QuestNumber] = "1234";
        QuestDataList[(int)QuestData.QuestObjectNumber] = "5678";
        QuestDataList[(int)QuestData.ClearCount] = "3";

        
        for (int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            if(Character.instance.MyItem[i] == QuestDataList[(int)QuestData.QuestObjectNumber])
            {
                
                Debug.Log("����Ʈ�� ���� ���� ����");
                if (Character.instance.MyItemCount[i] == int.Parse(QuestDataList[(int)QuestData.ClearCount]))
                {
                    Debug.Log("����Ʈ������Ʈ�� ���� ������ ����");
                    ResidenceqQuestState = QuestState.End;
                    return;
                }
                ResidenceqQuestState = QuestState.Progress;
                return;
            }
        }
        Debug.Log("����Ʈ�� ���� ���� ����");
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
        Debug.Log("����Ʈ ����");
    }
}
