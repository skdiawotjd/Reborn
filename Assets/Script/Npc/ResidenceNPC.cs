using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidenceNPC : QuestNPC
{
    protected override void Start()
    {
        base.Start();

        SetNpcNumber(3);
        SetChatType(0);
        SetStory();
        SetQuestData("00010", "7010", "1");
    }

    protected override void FunctionStart()
    {
        base.FunctionStart();

        switch(QuestNpcState)
        {
            case QuestState.Story:
                ConversationManager.CurNpc = this;
                ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                break;
        }
    }

    public override void FunctionEnd()
    {
        base.FunctionEnd();

        switch (QuestNpcState)
        {
            case QuestState.Start:

                break;
            case QuestState.Progress:

                break;
            case QuestState.End:

                break;
            case QuestState.Stand:
                Debug.Log("asd");
                SetStory();

                base.BaseFunctionEnd();
                break;
            case QuestState.Story:
                SetNpcNumber(11);
                QuestNpcState = QuestState.Start;

                base.BaseFunctionEnd();
                break;
        }
    }

    private void SetStory()
    {
        SetNpcNumber(3);
        SetChatType(0);
        QuestNpcState = QuestState.Story;
    }
}
