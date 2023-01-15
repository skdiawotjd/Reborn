using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButlerNPC : BasicNpc
{
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
            switch(Character.instance.MyMapNumber)
            {
                case "0000":
                    ChatType = Random.Range(0, 2);
                    break;
                case "0100":
                case "0200":
                    ChatType = 0;
                    break;
            }
            
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            if (ChatType == 0)
            {
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, "99991"/*�߰� �� ������ �ѹ��� ����*/);
            }
            else
            {
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, "97971"/*�߰� �� ������ �ѹ��� ����*/);
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
            Debug.Log("����Ʈ ����. ���� �� TP : " + Character.instance.Reputation);
            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 2); // todoProgress + 2
            Debug.Log("TodoProgress +2. ���� TP : " + Character.instance.Reputation);
            Character.instance.MyPlayerController.EventConversation.Invoke();
            QuestManager.instance.subQuestStart = false;

        }
    }
}
