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
                Debug.Log("대화 시작 - 우선 할 말이 없어 퀘스트 완료 대사");
                _chatType = 0;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.QuestStand:
                Debug.Log("대화 시작 - 퀘스트 대기 대사");
                _chatType = 6;
                Debug.Log("AddSelectEvent");
                ConversationManager.AddSelectEvent(SelectQuest);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.QuestStart:
                Debug.Log("대화 시작 - 퀘스트 받기");
                break;
            case QuestState.QuestProgress:
            case QuestState.QuestEnd:
                Debug.Log("대화 시작 - 퀘스트 중 " + !Processing);
                Processing = !Processing;
                if (Processing)
                {
                    _chatType = 2;
                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                }
                else
                {
                    Debug.Log("대화 시작 - 퀘스트 완료");
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
                Debug.Log("대화 대화 끝 - 퀘스트 수주 대사");
                _chatType = 5;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                Debug.Log("대사 다시 시작");
                break;
            case QuestState.QuestStart:
                Debug.Log("대화 끝 - AcceptQuest에서 선택된 것에 따라 퀘스트를 수주하거나 거절");
                base.FunctionEnd();
                QuestNpcState = QuestState.QuestStand;
                break;
            case QuestState.QuestProgress:
                Debug.Log("대화 끝 - 퀘스트 확인");
                CheckQuest();
                if (Processing)
                {
                    //CheckQuest();
                    Debug.Log("Processing가 " + Processing + "라서" + ChatType + " 대사");

                    Processing = !Processing;

                    ConversationManager.CurNpc = this;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.MyPlayerController.InvokeEventConversation();
                    Debug.Log("대사 다시 시작");
                }
                else
                {
                    Debug.Log("Processing가 " + Processing + "라서 FunctionEnd");
                    base.FunctionEnd();
                }
                break;
            case QuestState.QuestEnd:
                Debug.Log("대화 끝 - 퀘스트 대기로 이동");
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
                
                Debug.Log("퀘스트를 받은 적이 있음");
                if (Character.instance.MyItemCount[i] >= int.Parse(QuestDataList[(int)QuestData.ClearCount]))
                {
                    Debug.Log("퀘스트오브젝트를 전부 가지고 있음");

                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestDataList[(int)QuestData.QuestObjectNumber] + "-" + QuestDataList[(int)QuestData.ClearCount]);
                    QuestManager.instance.RemoveQuest(QuestDataList[(int)QuestData.QuestNumber]);
                    QuestNpcState = QuestState.QuestEnd;
                    _chatType = 0;
                    return;
                }
                else
                {
                    Debug.Log("퀘스트오브젝트를 전부 가지고 있지 않음");
                    QuestNpcState = QuestState.QuestProgress;
                    _chatType = 1;
                    return;
                }
            }
        }
        Debug.Log("퀘스트를 받은 적이 없음");
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
                Debug.Log("퀘스트 수락 버튼");
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, QuestDataList[(int)QuestData.QuestObjectNumber] + "0");
                QuestManager.instance.AddQuest(QuestDataList[(int)QuestData.QuestNumber]);
                QuestNpcState = QuestState.QuestProgress;
                _chatType = 4;
                break;
            case 2:
                Debug.Log("퀘스트 거절 버튼");
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
 * 6,퀘스트 내용
 * 5,퀘스트 선택지
 * 4,퀘스트 수락 대사
 * 3,퀘스트 거절 대사
 * 2,퀘스트 진행 대사
 * 1,퀘스트 진행 중 미완료 대사
 * 0,퀘스트 진행 중 완료 대사
 */