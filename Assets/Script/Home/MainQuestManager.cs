using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestManager : BasicNpc
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FunctionStart()
    {
        Debug.Log("��� ���� 1 - �ݸ��� �浹(NPC �ѹ� " + ConversationManager.NpcNumberChatType + " )");
        if (QuestManager.instance.questEnd)
        {
            // ����Ʈ �Ϸ� ����
            ChatType = 0;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // ����Ʈ Ȯ��
            ChatType = 1;
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
                if (Character.instance.MyItem[i] == QuestManager.instance.todayQuest.Substring(0,4))
                {
                    if (Character.instance.MyItemCount[i] >= int.Parse(QuestManager.instance.todayQuest.Substring(4)) )
                    {
                        ChatType = 2;
                        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                        Character.instance.SetCharacterStat(CharacterStatType.MyItem, (QuestManager.instance.questDeleteNumber)/*���� ������ �ѹ��� ����*/);
                        Character.instance.SetCharacterStat(CharacterStatType.TodoProgress, 5); // TodoProgress 5 ����
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
