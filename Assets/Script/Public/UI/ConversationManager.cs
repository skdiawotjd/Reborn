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
    
    List<Dictionary<string, object>> ChatList;
    public BasicNpc CurNpc;
    private bool IsCanChat; // ��ȭ�� ������ �� �ִ���
    private int _chatCount; // CSV �� ��ȭ�� ��� °�� �ִ���
    private int _conversationCount; // ��ȭ�� ��� ���� �Ǿ�����
    private int _totalCount; // ��ȭ�� ��� ���� �Ǿ�����
    public string _npcNumberChatType; // "NPC�ѹ�-��ȭ�ѹ�"

    public int ConversationCount
    {
        get
        {
            return _conversationCount;
        }
    }
    public int TotalCount
    {
        get
        {
            return _totalCount;
        }
    }
    public string NpcNumberChatType
    {
        set
        {
            SetChat(value);
        }
        get
        {
            return _npcNumberChatType;
        }
    }

    
    void Awake()
    {
        ChatList = CSVReader.Read("Chatting");

        InitializeConversationManager();

        GameManager.instance.AddGameStartEvent(InitializeNpcNumberChatType);
        GameManager.instance.AddDayEnd(DayEnd);
    }

    void Start()
    {
        Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
    }

    private void InitializeConversationManager()
    {
        IsCanChat = false;
        _chatCount = -1;
        _conversationCount = -1;
        NpcNumberChatType = "0-0";
        ConversationPanel.gameObject.SetActive(false);
    }

    private void SetChat(string NNN)
    {
        _npcNumberChatType = NNN;
        for (int i = 0; i < ChatList.Count; i++)
        {
            if (ChatList[i]["NpcNumber"].ToString() == NNN)
            {
                IsCanChat = true;
                _chatCount = i;
                _totalCount = ChatList[_chatCount].Count;
                _conversationCount = 0;
                break;
            }
        }
    }

    private void NextConversation()
    {
        StartCoroutine(ChatCoroutine());
        if(false)
        {
            /*Debug.Log("��� ���� 3 (" + ConversationCount + " < " + ChatList[NpcToChat].Count + ") �̸�");
            if (ConversationCount < ChatList[NpcToChat].Count)
            {
                if (!ConversationPanel.gameObject.activeSelf)
                {
                    ConversationPanel.gameObject.SetActive(true);
                }
                Debug.Log("��� ���� 4 - " + "ChatList[" + NpcToChat + "][Context" + ConversationCount + "]");
                ContentText.text = GetChat();
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
            }*/
        }
    }

    IEnumerator ChatCoroutine()
    {
        while(!IsCanChat)
        {
            yield return new WaitForSeconds(0.016f);
        }

        //Debug.Log("��� ���� 3 ConversationCount�� (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") �̸�");
        if (ConversationCount < ChatList[_chatCount].Count - 1)
        {
            if (!ConversationPanel.gameObject.activeSelf)
            {
                ConversationPanel.gameObject.SetActive(true);
            }
            //Debug.Log("��� ���� 4 - " + "ChatList[" + _chatCount + "][Context" + ConversationCount + "] ���");
            ContentText.text = ChatList[_chatCount]["Context" + ConversationCount].ToString();
            _conversationCount++;
            StartCoroutine(SetConversationNext(true, 0.1f));
        }
        else
        {
            //Debug.Log("��� ���� 5 - ConversationCount�� ChatList[NpcToChat].Count���� ũ�Ƿ� ��� ��");
            ConversationPanel.gameObject.SetActive(false);

            if(CurNpc)
            {
                CurNpc.FunctionEnd();
            }
            if (ConversationCount == ChatList[_chatCount].Count - 1)
            {
                //Debug.Log("��� ���� 6 - ��� ��");
                StartCoroutine(SetConversationNext(false, 0.4f));
                Character.instance.SetCharacterInput(true, true);
                _conversationCount = -1;
                IsCanChat = false;
            }
        }
    }

    IEnumerator SetConversationNext(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Character.instance.MyPlayerController.ConversationNext = Next;
        //Debug.Log("ConversationCount ���� ��ġ : " + ConversationCount);
        //Debug.Log("���� �� " + Next + " Conversation�� NextConversation �Լ� " + Character.instance.MyPlayerController.ConversationNext);
    }

    private void DayEnd()
    {
        InitializeConversationManager();
    }

    private void InitializeNpcNumberChatType()
    {
        NpcNumberChatType = ((int)Character.instance.MyJob).ToString() + "-0";
    }
}
