using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryNPC : BasicNpc
{
    private string OrderString;
    private bool QuestStart;
    protected override void Start()
    {
        base.Start();
        QuestStart = false;
    }
    public void SetOrderString(string str)
    {
        OrderString = str;
    }

    protected override void FunctionStart()
    {
        if (QuestStart)
        {
            ChatType = 1;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
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
            for (int i = 0; i < Character.instance.MyItem.Count; i++)
            {
                if (Character.instance.MyItem[i] == OrderString) // 주문 서류가 있다면
                {
                    ChatType = 3;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, OrderString + "-1");
                    Character.instance.MyPlayerController.EventConversation.Invoke();
                    QuestStart = false;
                    return;
                }
            }
            ChatType = 2;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            Character.instance.MyPlayerController.EventConversation.Invoke();
            QuestStart = false;
        }
    }
}
