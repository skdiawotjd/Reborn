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
    private BasicNpc _curNpc;
    [SerializeField]
    private bool _isCanChat; // ä���� ������ �� �ִ���
    [SerializeField]
    private bool IsCatting; // ä���� ���� ������
    private Coroutine TypingCourtine;
    private bool _conversationPanelStillOpen;
    private int _chatCount; // CSV �� ��ȭ�� ��� °�� �ִ���
    private int _conversationCount; // ��ȭ�� ��� ���� �Ǿ�����
    private int _totalCount; // ��ȭ�� ��� ���� �Ǿ�����
    public string _npcNumberChatType; // "NPC�ѹ�-��ȭ�ѹ�"

    public BasicNpc CurNpc
    {
        set 
        {
            _curNpc = value;
            SetChatName(_curNpc.NpcName);
        }
        get
        {
            return _curNpc;
        }
    }
    public bool IsCanChat
    {
        get 
        { 
            return _isCanChat; 
        }
    }
    public bool ConversationPanelStillOpen
    {
        set
        {
            _conversationPanelStillOpen = value;
        }
        get
        {
            return _conversationPanelStillOpen;
        }
    }
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
        _isCanChat = false;
        IsCatting = false;
        _chatCount = -1;
        _conversationCount = -1;
        ConversationPanelStillOpen = false;
        _npcNumberChatType = "0-0";
        ConversationPanel.gameObject.SetActive(false);
    }

    private void SetChat(string NNN)
    {
        _npcNumberChatType = NNN;
        for (int i = 0; i < ChatList.Count; i++)
        {
            if (ChatList[i]["NpcNumber"].ToString() == NNN)
            {
                _isCanChat = true;
                _chatCount = i;
                _totalCount = ChatList[_chatCount].Count;
                _conversationCount = 0;

                if (_curNpc != null)
                {

                }

                break;
            }
        }
    }

    public void SetChatName(string Name)
    {
        NameText.text = Name;
    }

    private void NextConversation()
    {
        //StartCoroutine(ChatCoroutine());
        if(_isCanChat)
        {
            //Debug.Log("��� ���� 3 ConversationCount�� (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") �̸�");
            if (ConversationCount < ChatList[_chatCount].Count - 1)
            {
                if (!ConversationPanel.gameObject.activeSelf)
                {
                    ConversationPanel.gameObject.SetActive(true);
                }
                //Debug.Log("��� ���� 4 - " + "ChatList[" + _chatCount + "][Context" + ConversationCount + "] ���");
                if (IsCatting)
                {
                    StopCoroutine(TypingCourtine);
                    EndTyping();
                }
                else
                {
                    TypingCourtine = StartCoroutine(Typing());
                }
            }
            else
            {
                //Debug.Log("��� ���� 5 - ConversationCount�� ChatList[NpcToChat].Count���� ũ�Ƿ� ��� ��");
                ConversationPanel.gameObject.SetActive(ConversationPanelStillOpen);

                if (ConversationCount == ChatList[_chatCount].Count - 1)
                {
                    //Debug.Log("��� ���� 6 - ��� ��");
                    //StartCoroutine(EndConversation(false, 0.2f));
                    if (_curNpc != null)
                    {
                        Character.instance.SetCharacterInput(true, true, true);

                        _conversationCount = -1;
                        _isCanChat = false;
                        SetChatName("");

                        StartCoroutine(EndConversation(false, 0.5f));
                    }
                    else
                    {
                        _conversationCount = -1;
                        _isCanChat = false;

                        Character.instance.MyPlayerController.ConversationNext = false;
                    }

                }
            }
        }
    }

    IEnumerator ChatCoroutine()
    {
        while(!IsCanChat)
        {
            Debug.Log("Asad");
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
            /*ContentText.text = ChatList[_chatCount]["Context" + ConversationCount].ToString();
            _conversationCount++;
            StartCoroutine(SetConversationNext(true, 0.1f));*/
            StartCoroutine(Typing());
        }
        else
        {
            //Debug.Log("��� ���� 5 - ConversationCount�� ChatList[NpcToChat].Count���� ũ�Ƿ� ��� ��");
            ConversationPanel.gameObject.SetActive(ConversationPanelStillOpen);

            if(CurNpc)
            {
                CurNpc.FunctionEnd();
            }
            if (ConversationCount == ChatList[_chatCount].Count - 1)
            {
                //Debug.Log("��� ���� 6 - ��� ��");
                StartCoroutine(EndConversation(false, 0.2f));
            }
        }
    }

    IEnumerator Typing()
    {
        if(!IsCatting)
        {
            IsCatting = true;

            int WordCount = 0;
            ContentText.text = "";
            Character.instance.MyPlayerController.ConversationNext = true;

            Debug.Log(ContentText.text + " != " + ChatList[_chatCount]["Context" + ConversationCount].ToString());
            while (ContentText.text != ChatList[_chatCount]["Context" + ConversationCount].ToString())
            {
                ContentText.text += ChatList[_chatCount]["Context" + ConversationCount].ToString()[WordCount];
                yield return new WaitForSeconds(0.075f);
                WordCount++;
            }

            EndTyping();
        }
    }

    private void EndTyping()
    {
        if(ContentText.text != ChatList[_chatCount]["Context" + ConversationCount].ToString())
        {
            ContentText.text = ChatList[_chatCount]["Context" + ConversationCount].ToString();
        }

        _conversationCount++;
        IsCatting = false;
    }

    IEnumerator SetConversationNext(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Character.instance.MyPlayerController.ConversationNext = Next;
        //Debug.Log("ConversationCount ���� ��ġ : " + ConversationCount);
        //Debug.Log("���� �� " + Next + " Conversation�� NextConversation �Լ� " + Character.instance.MyPlayerController.ConversationNext);
    }

    IEnumerator EndConversation(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        //_conversationCount = -1;
        _curNpc.FunctionEnd();
        _curNpc = null;
        Character.instance.MyPlayerController.ConversationNext = Next;
        /*IsCanChat = Next;

        if(_curNpc != null)
        {
            Character.instance.SetCharacterInput(true, true, true);
        }

        _curNpc = null;*/
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
