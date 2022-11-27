using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButlerNPC : BasicNpc
{
    private bool QuestStart = false;
    protected override void Start()
    {
        base.Start();
    }


    protected override void FunctionStart()
    {

        //Debug.Log("��� ���� 1 - �ݸ��� �浹(NPC �ѹ� " + ConversationManager.NpcNumberChatType + " )");
        if (QuestManager.instance.subQuestStart)
        {
            // ����Ʈ Ȯ��
            ChatType = 2;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // ����Ʈ �ο�
            QuestManager.instance.subQuestStart = true;
            ChatType = Random.Range(0,1);
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            if (ChatType == 0)
            {
                Character.instance.SetCharacterStat(8, "99991"/*�߰� �� ������ �ѹ��� ����*/);
            }
            else
            {
                Character.instance.SetCharacterStat(8, "97971"/*�߰� �� ������ �ѹ��� ����*/);
            }
            
        }
    }
    public override void FunctionEnd()
    {
        if (ChatType == 2)
        {
            for (int i = 0; i < Character.instance.MyItem.Count; i++)
            {
                if (Character.instance.MyItem[i] == "9999" || Character.instance.MyItem[i] == "9797") // �ֹ� ������ �ִٸ�
                {
                    ChatType = 3;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.MyPlayerController.EventConversation.Invoke();
                    return;
                }
            }

            ChatType = 4;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            Debug.Log("����Ʈ ����. ���� �� TP : " + Character.instance.TodoProgress);
            Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
            Debug.Log("TodoProgress +2. ���� TP : " + Character.instance.TodoProgress);
            Character.instance.MyPlayerController.EventConversation.Invoke();
            QuestManager.instance.subQuestStart = false;

        }
    }
}