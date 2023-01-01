using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustChatNpc : BasicNpc
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

    public override void FunctionEnd()
    {
        //Character.instance.SetCharacterStat(7, -Character.instance.ActivePoint);
    }
}
