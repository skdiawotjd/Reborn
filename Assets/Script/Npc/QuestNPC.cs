using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestNPC : BasicNpc
{
    [SerializeField]
    private TextMeshPro QusetStateText;
    [SerializeField]
    private List<string> QuestDataList;
    [SerializeField]
    private QuestState _questNpcState;

    private bool Processing;

    public QuestState QuestNpcState
    {
        set
        {
            _questNpcState = value;
            SettingQusetState();
        }
        get
        {
            return _questNpcState;
        }
    }

    protected override void Start()
    {
        base.Start();

        Processing = false;
        QuestDataList = new List<string>(new string[3]);

        SetNpcNumber(11);
        SetChatType(0);
        SetNpcName("����Ʈ NPC");
        SetQuestData("00010", "7010", "1");

        CheckQuest();
    }

    protected override void FunctionStart()
    {
        switch (_questNpcState)
        {
            case QuestState.Start:
                Debug.Log("��ȭ ���� - ����Ʈ �ޱ�");
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestDataList[(int)QuestData.QuestObjectNumber] + QuestDataList[(int)QuestData.ClearCount]);
                QuestManager.instance.AddQuest(QuestDataList[(int)QuestData.QuestNumber]);

                _chatType = 0;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.Progress:
                Debug.Log("��ȭ ���� - ����Ʈ ��");
                Processing = !Processing;
                if (Processing)
                {
                    _chatType = 1;
                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                }
                break;
            case QuestState.End:
                Debug.Log("��ȭ ���� - ����Ʈ �Ϸ�");
                _chatType = 3;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.Stand:
                Debug.Log("��ȭ ���� - ����Ʈ ���");
                _chatType = 3;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
        }
    }

    public override void FunctionEnd()
    {
        switch (_questNpcState)
        {
            case QuestState.Start:
                Debug.Log("��ȭ �� - ����Ʈ ������ �̵�");
                QuestNpcState = QuestState.Progress;
                base.FunctionEnd();
                break;
            case QuestState.Progress:
                Debug.Log("��ȭ �� - ����Ʈ Ȯ��");
                CheckQuest();
                if (Processing)
                {
                    Processing = !Processing;

                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.MyPlayerController.EventConversation.Invoke();
                }
                else
                {
                    base.FunctionEnd();
                }
                break;
            case QuestState.End:
                Debug.Log("��ȭ �� - ����Ʈ ���� �̵�");
                QuestNpcState = QuestState.Stand;
                base.FunctionEnd();
                break;
            case QuestState.Stand:
                Debug.Log("��ȭ �� - ����Ʈ ���");
                base.FunctionEnd();
                break;
        }
    }

    protected void BaseFunctionEnd()
    {
        base.FunctionEnd();
    }

    private void CheckQuest()
    {
        Debug.Log("CheckQuest");
        for (int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            if(Character.instance.MyItem[i] == QuestDataList[(int)QuestData.QuestObjectNumber])
            {
                
                Debug.Log("����Ʈ�� ���� ���� ����");
                if (Character.instance.MyItemCount[i] == int.Parse(QuestDataList[(int)QuestData.ClearCount]))
                {
                    Debug.Log("����Ʈ������Ʈ�� ���� ������ ����");
                    QuestNpcState = QuestState.End;

                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestDataList[(int)QuestData.QuestObjectNumber] + "-" + QuestDataList[(int)QuestData.ClearCount]);
                    QuestManager.instance.RemoveQuest(QuestDataList[(int)QuestData.QuestNumber]);

                    _chatType = 3;
                    return;
                }
                else
                {
                    Debug.Log("����Ʈ������Ʈ�� ���� ������ ���� ����");
                    QuestNpcState = QuestState.Progress;
                    _chatType = 2;
                    return;
                }
            }
        }
        Debug.Log("����Ʈ�� ���� ���� ����");
    }

    private void SettingQusetState()
    {
        switch(_questNpcState)
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
            case QuestState.Story:
                QusetStateText.text = "";
                break;
        }
    }

    public void SetQuestData(string QuestNumber, string QuestObjectNumber, string ClearCount)
    {
        if(QuestDataList.Count != 0)
        {
            QuestDataList[(int)QuestData.QuestNumber] = QuestNumber;
            QuestDataList[(int)QuestData.QuestObjectNumber] = QuestObjectNumber;
            QuestDataList[(int)QuestData.ClearCount] = ClearCount;
        }
        else
        {
            QuestDataList = new List<string>(new string[3]);
            QuestDataList[(int)QuestData.QuestNumber] = QuestNumber;
            QuestDataList[(int)QuestData.QuestObjectNumber] = QuestObjectNumber;
            QuestDataList[(int)QuestData.ClearCount] = ClearCount;
        }
        
    }
}
