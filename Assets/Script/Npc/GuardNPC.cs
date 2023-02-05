using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardNPC : BasicNpc
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        
    }

    protected override void FunctionStart()
    {
        ChatType = 0;
        ConversationManager.CurNpc = this;
        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
    }
    public override void FunctionEnd()
    {

    }
}
