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
        Debug.Log ("대사 시작 3 (" + ConversationCount + " < " + ChatList[NpcToChat].Count + ") 이면");
        if (ConversationCount < ChatList[NpcToChat].Count)
        {
            if(!ConversationPanel.gameObject.activeSelf)
            {
                ConversationPanel.gameObject.SetActive(true);
            }
            Debug.Log("대사 시작 4 - " + "ChatList[" + NpcToChat + "][Context" + ConversationCount + "]");
            ContentText.text = ChatList[NpcToChat]["Context" + ConversationCount].ToString();
            _conversationCount++;
            StartCoroutine(SetConversationNext(true, 0.1f));
            Debug.Log("ConversationCount 현재 수치 : " + ConversationCount);
            Debug.Log("대사 시작 5 - Conversation의 NextConversation 함수 " + Character.instance.MyPlayerController.ConversationNext);
        }
        else
        {
            Debug.Log("대사 시작 6 - ConversationCount가 ChatList[NpcToChat].Count보다 크므로 대사 끝");
            ConversationPanel.gameObject.SetActive(false);
            _conversationCount = 1;
            Debug.Log("ConversationCount 값은 " + ConversationCount);
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
