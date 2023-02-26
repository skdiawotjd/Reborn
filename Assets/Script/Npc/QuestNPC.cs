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
        get { return _questNpcState; }
    }

    protected override void Start()
    {
        base.Start();

        Processing = false;
        QuestDataList = new List<string>(new string[3]);
        _questNpcState = QuestState.QuestStand;
        SetNpcNumber(11);
        SetChatType(6);
        SetQuestData("00010", "7010", "1");

        CheckQuest();
    }

    protected override void FunctionStart()
    {
        switch (_questNpcState)
        {
            case QuestState.None:
                Debug.Log("��ȭ ���� - �켱 �� ���� ���� ����Ʈ �Ϸ� ���");
                _chatType = 0;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.QuestStand:
                Debug.Log("��ȭ ���� - ����Ʈ ��� ���");
                _chatType = 6;
                Debug.Log("AddSelectEvent");
                ConversationManager.AddSelectEvent(SelectQuest);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.QuestStart:
                Debug.Log("��ȭ ���� - ����Ʈ �ޱ�");
                break;
            case QuestState.QuestProgress:
            case QuestState.QuestEnd:
                Debug.Log("��ȭ ���� - ����Ʈ �� " + !Processing);
                Processing = !Processing;
                if (Processing)
                {
                    _chatType = 2;
                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                }
                else
                {
                    Debug.Log("��ȭ ���� - ����Ʈ �Ϸ�");
                    _chatType = 0;
                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                }
                break;
        }
    }

    public override void FunctionEnd()
    {
        switch (_questNpcState)
        {
            case QuestState.None:
                base.FunctionEnd();
                break;
            case QuestState.QuestStand:
                Debug.Log("��ȭ ��ȭ �� - ����Ʈ ���� ���");
                _chatType = 5;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                Debug.Log("��� �ٽ� ����");
                break;
            case QuestState.QuestStart:
                Debug.Log("��ȭ �� - AcceptQuest���� ���õ� �Ϳ� ���� ����Ʈ�� �����ϰų� ����");
                base.FunctionEnd();
                QuestNpcState = QuestState.QuestStand;
                break;
            case QuestState.QuestProgress:
                Debug.Log("��ȭ �� - ����Ʈ Ȯ��");
                CheckQuest();
                if (Processing)
                {
                    //CheckQuest();
                    Debug.Log("Processing�� " + Processing + "��" + ChatType + " ���");

                    Processing = !Processing;

                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.MyPlayerController.InvokeEventConversation();
                    Debug.Log("��� �ٽ� ����");
                }
                else
                {
                    Debug.Log("Processing�� " + Processing + "�� FunctionEnd");
                    base.FunctionEnd();
                }
                break;
            case QuestState.QuestEnd:
                Debug.Log("��ȭ �� - ����Ʈ ���� �̵�");
                if (Processing)
                {
                    Processing = !Processing;

                    _chatType = 0;
                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.MyPlayerController.InvokeEventConversation();
                }
                else
                {
                    QuestNpcState = QuestState.None;
                    base.FunctionEnd();
                }
                break;
        }
    }

    protected void BaseFunctionEnd()
    {
        base.FunctionEnd();
    }

    private void CheckQuest()
    {
        //Debug.Log("CheckQuest");
        for (int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            if(Character.instance.MyItem[i] == QuestDataList[(int)QuestData.QuestObjectNumber])
            {
                
                Debug.Log("����Ʈ�� ���� ���� ����");
                if (Character.instance.MyItemCount[i] >= int.Parse(QuestDataList[(int)QuestData.ClearCount]))
                {
                    Debug.Log("����Ʈ������Ʈ�� ���� ������ ����");

                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestDataList[(int)QuestData.QuestObjectNumber] + "-" + QuestDataList[(int)QuestData.ClearCount]);
                    QuestManager.instance.RemoveQuest(QuestDataList[(int)QuestData.QuestNumber]);
                    QuestNpcState = QuestState.QuestEnd;
                    _chatType = 0;
                    return;
                }
                else
                {
                    Debug.Log("����Ʈ������Ʈ�� ���� ������ ���� ����");
                    QuestNpcState = QuestState.QuestProgress;
                    _chatType = 1;
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
            case QuestState.QuestStand:
            case QuestState.QuestStart:
                QusetStateText.text = "?";
                break;
            case QuestState.QuestProgress:
                QusetStateText.text = "...";
                break;
            case QuestState.QuestEnd:
                QusetStateText.text = "!";
                break;
            case QuestState.None:
            case QuestState.Chat:
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

    private void SelectQuest(int ButtonCount)
    {
        QuestNpcState = QuestState.QuestStart;
        switch (ButtonCount)
        {
            case 0:
                Debug.Log("����Ʈ ���� ��ư");
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestDataList[(int)QuestData.QuestObjectNumber] + "0");
                QuestManager.instance.AddQuest(QuestDataList[(int)QuestData.QuestNumber]);
                QuestNpcState = QuestState.QuestProgress;
                _chatType = 4;
                break;
            case 2:
                Debug.Log("����Ʈ ���� ��ư");
                _chatType = 3;
                break;

        }
        ConversationManager.CurNpc = this;
        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        Character.instance.MyPlayerController.InvokeEventConversation();
    }
}
/*
 * _chatType
 * 6,����Ʈ ����
 * 5,����Ʈ ������
 * 4,����Ʈ ���� ���
 * 3,����Ʈ ���� ���
 * 2,����Ʈ ���� ���
 * 1,����Ʈ ���� �� �̿Ϸ� ���
 * 0,����Ʈ ���� �� �Ϸ� ���
 */