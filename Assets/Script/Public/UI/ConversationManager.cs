using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConversationManager : UIManager
{
    // UI
    [SerializeField]
    private TextMeshProUGUI NameText;
    [SerializeField]
    private TextMeshProUGUI ContentText;
    [SerializeField]
    private GameObject SelectPanel;
    [SerializeField]
    private Button SelectButton;

    List<Dictionary<string, object>> ChatList;
    //[SerializeField]
    private BasicNpc _curNpc;
    //[SerializeField]
    private bool _isCanChat; // 채팅을 시작할 수 있는지
    //[SerializeField]
    private bool IsCatting; // 채팅이 진행 중인지
    private Coroutine TypingCourtine;
    private bool _conversationPanelStillOpen;
    private int _chatCount; // CSV 상 대화가 몇번 째에 있는지
    private int _conversationCount; // 대화가 몇번 진행 되었는지
    private int _totalCount; // 대화가 몇번 진행 되었는지
    public string _npcNumberChatType; // "NPC넘버-대화넘버"

    private UnityEvent<int> SelectEvent;

    public BasicNpc CurNpc
    {
        set 
        {
            _curNpc = value;
            if(_curNpc != null)
            {
                SetChatName(_curNpc.NpcName);
            }
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

    public void AddSelectEvent(UnityAction<int> AddEvent)
    {
        SelectEvent.AddListener(AddEvent);
    }

    void Awake()
    {
        ChatList = CSVReader.Read("Chatting");
        SelectEvent = new UnityEvent<int>();

        InitializeConversationManager();

        GameManager.instance.AddGameStartEvent(InitializeNpcNumberChatType);
        GameManager.instance.AddSceneMoveEvent(ClearSelectEvent);
    }

    protected override void Start()
    {
        Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
    }

    protected override void StartUI()
    {
        
    }

    protected override void EndUI()
    {
        InitializeConversationManager();
    }

    private void InitializeConversationManager()
    {
        _isCanChat = false;
        IsCatting = false;
        _chatCount = -1;
        _conversationCount = -1;
        ConversationPanelStillOpen = false;
        _npcNumberChatType = "0-0";
        SetActivePanel(false);
        //ConversationPanel.gameObject.SetActive(false);
    }

    private void InitializeNpcNumberChatType()
    {
        NpcNumberChatType = ((int)Character.instance.MyJob).ToString() + "-0";
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

                break;
            }
        }
    }

    public void SetChatName(string Name)
    {
        NameText.text = Name;
    }

    private void ClearSelectEvent()
    {
        SelectEvent.RemoveAllListeners();
    }

    private void NextConversation()
    {
        //StartCoroutine(ChatCoroutine());
        if(_isCanChat)
        {
            //Debug.Log("대사 시작 3 ConversationCount가 (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") 이면");
            if (ConversationCount < ChatList[_chatCount].Count - 1)
            {
                /*if (!ConversationPanel.gameObject.activeSelf)
                {
                    ConversationPanel.gameObject.SetActive(true);
                }*/
                if (!Panel.activeSelf)
                {
                    SetActivePanel(true);
                }
                //Debug.Log("대사 시작 4 - " + "ChatList[" + _chatCount + "][Context" + ConversationCount + "] 출력");
                if (IsCatting)
                {
                    StopCoroutine(TypingCourtine);
                    EndTyping();
                }
                else
                {
                    if(ChatList[_chatCount]["Context" + ConversationCount].ToString().Length != 0)
                    {
                        TypingCourtine = StartCoroutine(Typing());
                    }
                    else
                    {
                        StartSelect();
                    }
                }
            }
            else
            {
                //Debug.Log("대사 시작 5 - ConversationCount가 ChatList[NpcToChat].Count보다 크므로 대사 끝");
                //ConversationPanel.gameObject.SetActive(ConversationPanelStillOpen);
                SetActivePanel(ConversationPanelStillOpen);

                if (ConversationCount == ChatList[_chatCount].Count - 1)
                {
                    _conversationCount = -1;
                    _isCanChat = false;
                    //Debug.Log("대사 시작 6 - 대사 끝");
                    if (_curNpc != null)
                    {
                        SetChatName("");

                        StartCoroutine(EndConversation(false, 0.2f));
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

    IEnumerator Typing()
    {
        if(!IsCatting)
        {
            IsCatting = true;

            int WordCount = 0;
            ContentText.text = "";
            Character.instance.MyPlayerController.ConversationNext = true;

            //Debug.Log(ContentText.text + " != " + ChatList[_chatCount]["Context" + ConversationCount].ToString());
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

    IEnumerator EndConversation(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        Character.instance.MyPlayerController.ConversationNext = Next;
        _curNpc.FunctionEnd();
    }

    public void StartSelect()
    {
        SelectPanel.SetActive(true);

        for(int i = 0; i < ChatList[_chatCount].Count -2; i++)
        {
            _conversationCount++;

            Button TemButton;
            TemButton = Instantiate(SelectButton, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Button;
            TemButton.transform.SetParent(SelectPanel.transform, false);
            TemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ChatList[_chatCount]["Context" + ConversationCount].ToString();
            int TemType = i;
            TemButton.onClick.AddListener(() => 
            { 
                SelectEvent.Invoke(TemType);
                NextConversation();
                SelectPanel.SetActive(false);
            });
        }
        _conversationCount++;
    }
}
