using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNPC : BasicNpc
{
    [SerializeField]
    private string questNumber;
    [SerializeField]
    private bool onlyChat;
    [SerializeField]
    private TownManager townManager;

    private bool QuestStart;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        QuestStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetquestNumber(string number)
    {
        questNumber = number;
    }
    protected override void FunctionStart()
    {

        //Debug.Log("��� ���� 1 - �ݸ��� �浹(NPC �ѹ� " + ConversationManager.NpcNumberChatType + " )");
        if (QuestStart)
        {
            // ����Ʈ Ȯ��
            ChatType = 1;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
        else
        {
            // ����Ʈ �ο�
            QuestStart = true;
            ChatType = 0;
            ConversationManager.CurNpc = this;
            ConversationManager.NpcNumberChatType = NpcNumber.ToString() + "-" + ChatType.ToString();
        }
    }
    public override void FunctionEnd()
    {
        if(ChatType == 1)
        {
            if (onlyChat)
            {

            }
            else
            {
                Character.instance.SetCharacterStat(6, questNumber);
                townManager.TownSceneMove(questNumber);
            }
        }
    }
}
