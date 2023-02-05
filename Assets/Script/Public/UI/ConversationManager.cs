using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class ConversationManager : UIManager
{
    // UI
    [SerializeField]
    private TextMeshProUGUI NameText;
    [SerializeField]
    private TextMeshProUGUI ContentText;
    [SerializeField]
    private GridLayoutGroup SelectPanel;
    [SerializeField]
    private Button SelectButton;

    List<Dictionary<string, object>> ChatList;
    List<Dictionary<string, object>> CharacterNameList;
    [SerializeField]
    private BasicNpc _curNpc;                   // ��ȭ �� Npc
    [SerializeField]
    private bool IsCanChat;                     // ä���� ������ �� �ִ���
    [SerializeField]
    private bool IsCatting;                     // ä���� ���� ������
    private Coroutine TypingCourtine;           // ä�� �ڷ�ƾ�� �����ϱ� ���� ����
    [SerializeField]
    private bool _conversationPanelStillOpen;    // ä�� �г��� ��� ���� �־�� �ϴ���
    [SerializeField]
    private int ChatCount;                      // CSV �� ��ȭ�� ��� °�� �ִ���
    [SerializeField]
    private int _conversationCount;             // ��ȭ�� ��� ���� �Ǿ�����
    [SerializeField]
    private string ChatName;                    // CSV 0���� ��縦 ġ�� ĳ���� �̸�
    private int SelectedButton;
    private List<Button> SelectButtonList;
    Color SelectColor;

    private UnityEvent<int> SelectEvent;

    public BasicNpc CurNpc
    {
        set { _curNpc = value; }
        get { return _curNpc; }
    }
    public bool ConversationPanelStillOpen
    {
        set { _conversationPanelStillOpen = value; }
        get { return _conversationPanelStillOpen; }
    }
    public int ConversationCount
    {
        get { return _conversationCount; }
    }
    public string NpcNumberChatType
    {
        set { SetChat(value); }
    }

    public void AddSelectEvent(UnityAction<int> AddEvent)
    {
        SelectEvent.AddListener(AddEvent);
    }

    void Awake()
    {
        ChatList = CSVReader.Read("Chatting");
        CharacterNameList = CSVReader.Read("CharacterName");
        SelectEvent = new UnityEvent<int>();
        SelectButtonList = new List<Button>();
        SelectColor = new Color();
        SelectColor.r = 0.3f;
        SelectColor.g = 0.3f;
        SelectColor.b = 0.3f;
        SelectColor.a = 1f;

        InitializeConversationManager();

        GameManager.instance.AddGameStartEvent(InitializeNpcNumberChatType);
        GameManager.instance.AddSceneMoveEvent(ClearSelectEvent);
    }

    protected override void Start()
    {
        //Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
        Character.instance.MyPlayerController.AddEventConversation(NextConversation);
        Character.instance.MyPlayerController.AddEventSelect(MiddleSelect);
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
        IsCanChat = false;
        IsCatting = false;
        ChatCount = -1;
        _conversationCount = -1;
        _conversationPanelStillOpen = false;
        SetActivePanel(false);
        SelectedButton = -1;
    }

    private void InitializeNpcNumberChatType()
    {
        //NpcNumberChatType = ((int)Character.instance.MyJob).ToString() + "-0";
    }

    private void SetChat(string NNN)
    {
        for (int i = 0; i < ChatList.Count; i++)
        {
            if (NNN[0] == ChatList[i]["NpcNumber"].ToString()[0])
            {
                if (ChatList[i]["NpcNumber"].ToString() == NNN)
                {
                    IsCanChat = true;
                    ChatCount = i;
                    _conversationCount = 1;
                    ChatName = ChatList[i]["Context0"].ToString();
                    break;
                }
            }
            else
            {
                i += (ChatList[i]["NpcNumber"].ToString()[2] - '0');
            }
        }
    }

    private void ClearSelectEvent()
    {
        SelectEvent.RemoveAllListeners();
    }

    private void NextConversation()
    {
        if(IsCanChat)
        {
            //Debug.Log("��� ���� 3 ConversationCount�� (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") �̸�");
            if (_conversationCount < ChatList[ChatCount].Count - 1)
            {
                if (!Panel.activeSelf)
                {
                    SetActivePanel(true);
                }
                //Debug.Log("��� ���� 4 - " + "ChatList[" + _chatCount + "][Context" + ConversationCount + "] ���");
                if (IsCatting)
                {
                    StopCoroutine(TypingCourtine);
                    EndTyping();
                }
                else
                {
                    if(ChatName.Length != 0)
                    {
                        SetChatName();
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
                //Debug.Log("��� ���� 5 - ConversationCount�� ChatList[NpcToChat].Count���� ũ�Ƿ� ��� ��");
                SetActivePanel(_conversationPanelStillOpen);

                if (_conversationCount == ChatList[ChatCount].Count - 1)
                {
                    _conversationCount = -1;
                    IsCanChat = false;
                    _conversationPanelStillOpen = false;

                    //Debug.Log("��� ���� 6 - ��� ��");
                    if (_curNpc != null)
                    {
                        StartCoroutine(EndConversation(false, 0.2f));
                    }
                    else
                    {
                        _conversationCount = -1;
                        IsCanChat = false;

                        Character.instance.MyPlayerController.ConversationNext = false;
                    }
                }
            }
        }
    }

    private void SetChatName()
    {
        for (int i = 0; i < CharacterNameList.Count; i++)
        {
            if (CharacterNameList[i]["CharacterNumber"].ToString() == ChatName[_conversationCount - 1].ToString())
            {
                NameText.text = CharacterNameList[i]["CharacterName"].ToString();
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

            try
            {
                if (ChatName[_conversationCount - 2] != ChatName[_conversationCount - 1])
                {
                    SetChatName();
                }
            }
            catch
            {

            }

            //Debug.Log(ContentText.text + " != " + ChatList[_chatCount]["Context" + ConversationCount].ToString());
            while (ContentText.text != ChatList[ChatCount]["Context" + _conversationCount].ToString())
            {
                ContentText.text += ChatList[ChatCount]["Context" + _conversationCount].ToString()[WordCount];
                yield return new WaitForSeconds(0.075f);
                WordCount++;
            }

            EndTyping();
        }
    }

    private void EndTyping()
    {
        if(ContentText.text != ChatList[ChatCount]["Context" + _conversationCount].ToString())
        {
            ContentText.text = ChatList[ChatCount]["Context" + _conversationCount].ToString();
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
        if (!SelectPanel.gameObject.activeSelf)
        {
            SelectPanel.gameObject.SetActive(true);

            if(ChatList[ChatCount].Count  == 4)
            {
                SelectPanel.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            }
            else
            {
                SelectPanel.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            }

            for (int i = 0; i < ChatList[ChatCount].Count - 2; i++)
            {
                if(ChatList[ChatCount]["Context" + _conversationCount].ToString().Length != 0)
                {
                    SelectButtonList.Add(Instantiate(SelectButton, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Button);
                    SelectButtonList.Last().transform.SetParent(SelectPanel.transform, false);
                    SelectButtonList.Last().transform.SetParent(SelectPanel.transform, false);
                    SelectButtonList.Last().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ChatList[ChatCount]["Context" + _conversationCount].ToString();
                    int TemType = i;
                    SelectButtonList.Last().onClick.AddListener(() =>
                    {
                        _conversationCount++;
                        SelectEvent.Invoke(TemType);
                        NextConversation();
                        SelectPanel.gameObject.SetActive(false);
                    });
                }
                else
                {
                    SelectButtonList.Add(null);
                }
                _conversationCount++;
            }

            _conversationCount--;
            Character.instance.MyPlayerController.ConversationNext = true;
        }
        else
        {
            Debug.Log("���� " + SelectedButton);
            if(SelectedButton != -1)
            {
                SelectButtonList[SelectedButton].onClick.Invoke();
            }
        }
    }

    private void MiddleSelect(KeyDirection Direction)
    {
        if(SelectPanel.gameObject.activeSelf)
        {
            if(SelectedButton == -1)
            {
                SelectedButton = 0;
                SelectButtonList[0].image.color = SelectColor;
                Debug.Log("ó�� ���� SelectedButton " + SelectedButton);
            }
            else
            {
                Debug.Log("���� SelectedButton " + SelectedButton);
                SelectButtonList[SelectedButton].image.color = Color.white;
                if (Direction == KeyDirection.Down)
                {
                    SelectOrder(2);
                }
                else if (Direction == KeyDirection.Up)
                {
                    SelectOrder(-2);
                }
                else if (Direction == KeyDirection.Right)
                {
                    SelectOrder(1);
                }
                else if (Direction == KeyDirection.Left)
                {
                    SelectOrder(-1);
                }
            }
        }
    }
    private void SelectOrder(int AddOrder)
    {
        try
        {
            SelectButtonList[SelectedButton + AddOrder].image.color = SelectColor;
            SelectedButton += AddOrder;
            Debug.Log("���� SelectedButton " + SelectedButton);
        }
        catch
        {
            try
            {
                SelectButtonList[SelectedButton - AddOrder].image.color = SelectColor;
                SelectedButton -= AddOrder;
                Debug.Log("���� SelectedButton " + SelectedButton);
            }
            catch
            {
                SelectButtonList[SelectedButton].image.color = SelectColor;
            }
        }
    }
}