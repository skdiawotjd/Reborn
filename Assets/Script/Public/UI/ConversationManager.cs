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
    private bool _isCanChat; // 채팅을 시작할 수 있는지
    [SerializeField]
    private bool IsCatting; // 채팅이 진행 중인지
    private Coroutine TypingCourtine;
    private bool _conversationPanelStillOpen;
    private int _chatCount; // CSV 상 대화가 몇번 째에 있는지
    private int _conversationCount; // 대화가 몇번 진행 되었는지
    private int _totalCount; // 대화가 몇번 진행 되었는지
    public string _npcNumberChatType; // "NPC넘버-대화넘버"

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
            //Debug.Log("대사 시작 3 ConversationCount가 (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") 이면");
            if (ConversationCount < ChatList[_chatCount].Count - 1)
            {
                if (!ConversationPanel.gameObject.activeSelf)
                {
                    ConversationPanel.gameObject.SetActive(true);
                }
                //Debug.Log("대사 시작 4 - " + "ChatList[" + _chatCount + "][Context" + ConversationCount + "] 출력");
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
                //Debug.Log("대사 시작 5 - ConversationCount가 ChatList[NpcToChat].Count보다 크므로 대사 끝");
                ConversationPanel.gameObject.SetActive(ConversationPanelStillOpen);

                if (ConversationCount == ChatList[_chatCount].Count - 1)
                {
                    //Debug.Log("대사 시작 6 - 대사 끝");
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

        //Debug.Log("대사 시작 3 ConversationCount가 (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") 이면");
        if (ConversationCount < ChatList[_chatCount].Count - 1)
        {
            if (!ConversationPanel.gameObject.activeSelf)
            {
                ConversationPanel.gameObject.SetActive(true);
            }
            //Debug.Log("대사 시작 4 - " + "ChatList[" + _chatCount + "][Context" + ConversationCount + "] 출력");
            /*ContentText.text = ChatList[_chatCount]["Context" + ConversationCount].ToString();
            _conversationCount++;
            StartCoroutine(SetConversationNext(true, 0.1f));*/
            StartCoroutine(Typing());
        }
        else
        {
            //Debug.Log("대사 시작 5 - ConversationCount가 ChatList[NpcToChat].Count보다 크므로 대사 끝");
            ConversationPanel.gameObject.SetActive(ConversationPanelStillOpen);

            if(CurNpc)
            {
                CurNpc.FunctionEnd();
            }
            if (ConversationCount == ChatList[_chatCount].Count - 1)
            {
                //Debug.Log("대사 시작 6 - 대사 끝");
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
        //Debug.Log("ConversationCount 현재 수치 : " + ConversationCount);
        //Debug.Log("받은 값 " + Next + " Conversation의 NextConversation 함수 " + Character.instance.MyPlayerController.ConversationNext);
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
