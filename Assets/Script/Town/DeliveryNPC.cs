using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryNPC : BasicNpc
{
    private string OrderString;
    protected override void Start()
    {
        base.Start();
    }
    public void SetOrderString(string str)
    {
        OrderString = str;
    }

    protected override void FunctionStart()
    {
        ChatType = 0;
        ConversationManager.CurNpc = this;
        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
    }
    public override void FunctionEnd()
    {
        if (ChatType == 0)
        {
            for (int i = 0; i < Character.instance.MyItem.Count; i++)
            {
                if (Character.instance.MyItem[i] == OrderString) // �ֹ� ������ �ִٸ�
                {
                    ChatType = 2;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.SetCharacterStat(8, OrderString + "-1");
                    Character.instance.MyPlayerController.EventConversation.Invoke();
                    return;
                }
            }
            ChatType = 1;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            Character.instance.MyPlayerController.EventConversation.Invoke();
        }

    }


}
