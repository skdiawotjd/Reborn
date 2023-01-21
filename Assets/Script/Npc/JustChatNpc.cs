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
        //Debug.Log("대사 시작 1 - 콜리전 충돌(NPC 넘버 " + ConversationManager.NpcNumberChatType + " )");
        ConversationManager.CurNpc = this;
        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
    }
}
