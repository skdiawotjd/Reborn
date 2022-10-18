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
        Debug.Log("��� ���� 1 - �ݸ��� �浹(NPC �ѹ� " + ConversationManager.NpcNumberChatType + " )");
        if (QuestStart)
        {
            // ����Ʈ Ȯ��
            ChatType = 1;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // ����Ʈ �ο�
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
            // ����Ʈ ���� �Ǵ�
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