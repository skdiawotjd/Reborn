using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierNpc : BasicNpc
{
    private bool CheckCashier;

    protected override void Start()
    {
        base.Start();
        SetNpcNumber(3);
        ChatType = 0;
        CheckCashier = true;
    }

    

    protected override void FunctionStart()
    {
        // 콜리전 충돌 시
        
        if(CheckCashier)
        {
            CheckCashier = false;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
    }

    public override void FunctionEnd()
    {
        
    }
}
