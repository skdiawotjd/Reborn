using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustChatNPC : BasicNpc
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FunctionStart()
    {
        //Debug.Log("��� ���� 1 - �ݸ��� �浹(NPC �ѹ� " + ConversationManager.NpcNumberChatType + " )");
        ConversationManager.CurNpc = this;
        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
    }
}
