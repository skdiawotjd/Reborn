using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidenceNPC : QuestNPC
{
    protected override void Start()
    {
        base.Start();

        SetNpcNumber(3);
        SetChatType(3);
        QuestNpcState = QuestState.None;
        //SetStory();
        SetQuestData("00010", "7010", "1");
    }

    protected override void FunctionStart()
    {
        switch (QuestNpcState)
        {
            case QuestState.None:
                QuestNpcState = QuestState.SelectResidence;
                _chatType = 3;
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
            case QuestState.Chat:

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
                _chatType = 2;
                ConversationManager.AddSelectEvent(SelectResidence);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                break;
            case QuestState.QuestStart:
                QuestNpcState = QuestState.None;
                _npcNumber = 3;
                _chatType = 3;
                base.BaseFunctionEnd();
                break;
            case QuestState.QuestStand:
            case QuestState.QuestProgress:
            case QuestState.QuestEnd:
                base.FunctionEnd();
                break;
            case QuestState.Chat:
                _chatType = 1;
                ConversationManager.AddSelectEvent(SelectResidence);
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                Character.instance.MyPlayerController.InvokeEventConversation();
                QuestNpcState = QuestState.None;
                break;
        }
    }

    private void SetStory()
    {
        SetNpcNumber(3);
        SetChatType(0);
        QuestNpcState = QuestState.Chat;
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
                SetNpcNumber(11);
                SetChatType(6);
                QuestNpcState = QuestState.QuestStand;
                base.FunctionStart();
                break;
            case 4:
                Debug.Log("도움말 선택");
                break;
            case 6:
                Debug.Log("닫기 선택");
                QuestNpcState = QuestState.None;
                break;

        }
    }
}
