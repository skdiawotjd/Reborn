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
        Debug.Log("대사 시작 1 - 콜리전 충돌(NPC 넘버 " + ConversationManager.NpcNumberChatType + " )");
        if (QuestManager.instance.questEnd)
        {
            // 퀘스트 완료 이후
            ChatType = 0;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // 퀘스트 확인
            ChatType = 1;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
    }

    public override void FunctionEnd()
    {
        if (ChatType == 1)
        {
            // 퀘스트 조건 판단
            for (int i = 0; i < Character.instance.MyItem.Count; i++)
            {
                if (Character.instance.MyItem[i] == QuestManager.instance.todayQuest.Substring(0,4))
                {
                    if (Character.instance.MyItemCount[i] >= int.Parse(QuestManager.instance.todayQuest.Substring(4)) )
                    {
                        ChatType = 2;
                        ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                        Character.instance.SetCharacterStat(CharacterStatType.MyItem, (QuestManager.instance.questDeleteNumber)/*없앨 아이템 넘버와 개수*/);
                        Character.instance.SetCharacterStat(CharacterStatType.TodoProgress, 5); // TodoProgress 5 증가
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
