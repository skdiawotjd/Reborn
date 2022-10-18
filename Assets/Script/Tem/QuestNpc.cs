using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpc : BasicNpc
{
    private bool QuestStart;

    protected override void Start()
    {
        base.Start();
        QuestStart = false;
    }
    
    protected override void FunctionStart()
    {
        Debug.Log("대사 시작 1 - 콜리전 충돌(NPC 넘버 " + ConversationManager.NpcNumberChatType + " )");
        if (QuestStart)
        {
            // 퀘스트 확인
            ChatType = 1;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // 퀘스트 부여
            QuestStart = true;
            ChatType = 0;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
    }

    public override void FunctionEnd()
    {
        if (ChatType == 1)
        {
            // 퀘스트 조건 판단
            for (int i = 0; i < Character.instance.MyItem.Count; i++)
            {
                if (Character.instance.MyItem[i] == "0001")
                {
                    if (Character.instance.MyItemCount[i] >= 2)
                    {
                        ChatType = 2;
                        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        return;
                    }
                }
            }
            ChatType = 3;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            Character.instance.MyPlayerController.EventConversation.Invoke();
        }
    }
}