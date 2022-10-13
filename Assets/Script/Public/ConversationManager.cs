using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ConversationManager : MonoBehaviour
{
    // UI
    [SerializeField]
    private GameObject ConversationPanel;
    [SerializeField]
    private TextMeshProUGUI NameText;
    [SerializeField]
    private TextMeshProUGUI ContentText;
    // Chat
    List<Dictionary<string, object>> ChatList;
    private int _conversationCount;
    public int ConversationCount
    {
        get
        {
            return _conversationCount;
        }
    }
    // Npc
    public UnityEvent EventNpcNumber;
    private int _npcToChat;
    public int NpcToChat
    {
        set
        {
            _npcToChat = value;
        }
        get
        {
            return _npcToChat;
        }
    }

    void Awake()
    {
        _npcToChat = 0;
        _conversationCount = 1;
        ChatList = CSVReader.Read("Chatting");
    }

    void Start()
    {
        Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
    }

    private void NextConversation()
    {
        Debug.Log ("��� ���� 3 (" + ConversationCount + " < " + ChatList[NpcToChat].Count + ") �̸�");
        if (ConversationCount < ChatList[NpcToChat].Count)
        {
            if(!ConversationPanel.gameObject.activeSelf)
            {
                ConversationPanel.gameObject.SetActive(true);
            }
            Debug.Log("��� ���� 4 - " + "ChatList[" + NpcToChat + "][Context" + ConversationCount + "]");
            ContentText.text = ChatList[NpcToChat]["Context" + ConversationCount].ToString();
            _conversationCount++;
            StartCoroutine(SetConversationNext(true, 0.1f));
            Debug.Log("ConversationCount ���� ��ġ : " + ConversationCount);
            Debug.Log("��� ���� 5 - Conversation�� NextConversation �Լ� " + Character.instance.MyPlayerController.ConversationNext);
        }
        else
        {
            Debug.Log("��� ���� 6 - ConversationCount�� ChatList[NpcToChat].Count���� ũ�Ƿ� ��� ��");
            ConversationPanel.gameObject.SetActive(false);
            _conversationCount = 1;
            Debug.Log("ConversationCount ���� " + ConversationCount);
            Character.instance.SetCharacterInput(true, true);
            StartCoroutine(SetConversationNext(false, 0.5f));
        }
    }

    IEnumerator SetConversationNext(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Character.instance.MyPlayerController.ConversationNext = Next;
    }
}
