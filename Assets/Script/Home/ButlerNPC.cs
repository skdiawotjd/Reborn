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

        //Debug.Log("대사 시작 1 - 콜리전 충돌(NPC 넘버 " + ConversationManager.NpcNumberChatType + " )");
        if (QuestManager.instance.subQuestStart)
        {
            // 퀘스트 확인
            ChatType = 2;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // 퀘스트 부여
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
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, "99991"/*추가 할 아이템 넘버와 개수*/);
            }
            else
            {
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, "97971"/*추가 할 아이템 넘버와 개수*/);
            }
            
        }
    }
    public override void FunctionEnd()
    {
        if (ChatType == 2)
        {
            for (int i = 0; i < Character.instance.MyItem.Count; i++)
            {
                if (Character.instance.MyItem[i] == "9999" || Character.instance.MyItem[i] == "9797") // 주문 서류가 있다면
                {
                    ChatType = 3;
                    ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
                    Character.instance.MyPlayerController.EventConversation.Invoke();
                    return;
                }
            }

            ChatType = 4;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
            Debug.Log("퀘스트 성공. 변경 전 TP : " + Character.instance.Reputation);
            Character.instance.SetCharacterStat(CharacterStatType.Reputation, 2); // todoProgress + 2
            Debug.Log("TodoProgress +2. 현재 TP : " + Character.instance.Reputation);
            Character.instance.MyPlayerController.EventConversation.Invoke();
            QuestManager.instance.subQuestStart = false;

        }
    }
}
