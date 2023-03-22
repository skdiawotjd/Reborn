using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidenceNPC : QuestNPC
{
    void Awake()
    {
        QuestNpcState = QuestState.None;
        SetNpcNumber(3);
        SetQuestNpcNumber(11);
        SetChatType(3);
        SetQuestData("00010", "7020", "1");
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void FunctionStart()
    {
        switch (QuestNpcState)
        {
            case QuestState.None:
                QuestNpcState = QuestState.SelectResidence;
                SetNpcNumber(3);
                SetChatType(3);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.Chat:

                break;
            case QuestState.QuestProgress:
            case QuestState.QuestEnd:
                //SetNpcNumber(11);
                base.FunctionStart();
                break;
            /*case QuestState.QuestStand:
            case QuestState.QuestStart:
            case QuestState.QuestProgress:
            case QuestState.QuestEnd:

                break;*/

            case QuestState.Help:

                break;

        }

        //base.FunctionStart();
    }

    public override void FunctionEnd()
    {
        //base.FunctionEnd();

        switch (QuestNpcState)
        {
            case QuestState.None:
                base.BaseFunctionEnd();
                break;
            case QuestState.SelectResidence:
                SetChatType(2);
                ConversationManager.AddSelectEvent(SelectResidence);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                break;
            case QuestState.QuestStart:
                QuestNpcState = QuestState.None;
                base.BaseFunctionEnd();
                break;
            case QuestState.QuestStand:
            case QuestState.QuestProgress:
            case QuestState.QuestEnd:
                base.FunctionEnd();
                break;
            case QuestState.Chat:
                SetChatType(1);
                ConversationManager.AddSelectEvent(SelectResidence);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                QuestNpcState = QuestState.None;
                break;
            case QuestState.Help:
                SetChatType(0);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                QuestNpcState = QuestState.None;
                break;
        }
    }

    private void SetQuest()
    {
        SetNpcNumber(11);
        SetChatType(6);
    }

    private void SelectResidence (int ButtonCount)
    {
        switch (ButtonCount)
        {
            case 0:
                Debug.Log("대화 선택");
                QuestNpcState = QuestState.Chat;
                break;
            case 2:
                Debug.Log("퀘스트 수주 선택");
                SetQuest();
                QuestNpcState = QuestState.QuestStand;
                base.FunctionStart();
                break;
            case 4:
                Debug.Log("도움말 선택");
                QuestNpcState = QuestState.Help;
                break;
            case 6:
                Debug.Log("닫기 선택");
                QuestNpcState = QuestState.None;
                break;

        }
    }
}
