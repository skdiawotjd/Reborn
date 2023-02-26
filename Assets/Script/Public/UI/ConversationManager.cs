using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Mathematics;

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
    List<Dictionary<string, object>> NowList;
    [SerializeField]
    private BasicNpc _curNpc;                   // ��ȭ �� Npc
    [SerializeField]
    private bool IsCanChat;                     // ä���� ������ �� �ִ���
    [SerializeField]
    private bool IsCatting;                     // ä���� ���� ������
    [SerializeField]
    private Coroutine TypingCourtine;           // ä�� �ڷ�ƾ�� �����ϱ� ���� ����
    [SerializeField]
    private bool _conversationPanelStillOpen;   // ä�� �г��� ��� ���� �־�� �ϴ���
    [SerializeField]
    private bool _selectPanelStillOpen;         // ���� �г��� ��� ���� �־�� �ϴ���
    [SerializeField]
    private int ChatCount;                      // CSV �� ��ȭ�� ��� °�� �ִ���
    [SerializeField]
    private int _conversationCount;             // ��ȭ�� ��� ���� �Ǿ�����
    [SerializeField]
    private string ChatName;                    // CSV 0���� ��縦 ġ�� ĳ���� �̸�
    private bool _selectGame;                   // �ܺο��� ������ ����ϴ� ���
    private int SelectedButton;
    [SerializeField]
    private List<Button> SelectButtonList;
    private int RealSelectCount;
    private int TotalCount;
    private Color SelectColor;

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
    public bool SelectPanelStillOpen
    {
        set { _selectPanelStillOpen = value; }
        get { return _selectPanelStillOpen; }
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
        SelectEvent.RemoveAllListeners();

        SelectEvent.AddListener(AddEvent);
    }

    void Awake()
    {
        ChatList = CSVReader.Read("Chatting");
        CharacterNameList = CSVReader.Read("CharacterName");
        SelectEvent = new UnityEvent<int>();
        SelectButtonList = new List<Button>();
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectButtonList.Add(null);
        SelectColor = new Color();
        SelectColor.r = 0.3f;
        SelectColor.g = 0.3f;
        SelectColor.b = 0.3f;
        SelectColor.a = 1f;

        InitializeConversationManager();

        GameManager.instance.AddGameStartEvent(InitializeNpcNumberChatType);
        //GameManager.instance.AddSceneMoveEvent(ClearSelectEvent);
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
        _selectPanelStillOpen = false;
        SetActivePanel(false);
        ChatName = "";
        SelectedButton = -1;
        RealSelectCount = 0;
        TotalCount = 0;
    }

    private void InitializeNpcNumberChatType()
    {
        //NpcNumberChatType = ((int)Character.instance.MyJob).ToString() + "-0";
    }

    private void SetChat(string NNN)
    {
        NowList = ChatList;

        for (int i = 0; i < NowList.Count; i++)
        {
            if (NNN[0] == NowList[i]["NpcNumber"].ToString()[0])
            {
                if (NowList[i]["NpcNumber"].ToString() == NNN)
                {
                    _selectPanelStillOpen = false;

                    IsCanChat = true;
                    ChatCount = i;
                    _conversationCount = 1;
                    ChatName = NowList[i]["Context0"].ToString();
                    break;
                }
            }
            else
            {
                i += (NowList[i]["NpcNumber"].ToString()[2] - '0');
            }
        }
    }

    private void NextConversation()
    {
        if(IsCanChat)
        {
            //Debug.Log("��� ���� 3 ConversationCount�� (" + ConversationCount + " < " + (ChatList[_chatCount].Count - 1) + ") �̸�");
            if (_conversationCount < NowList[ChatCount].Count - 1)
            {
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

                if (_conversationCount == NowList[ChatCount].Count - 1)
                {
                    _conversationCount = -1;
                    IsCanChat = false;
                    _conversationPanelStillOpen = false;
                    ChatName = "";

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
        if (!Panel.activeSelf)
        {
            SetActivePanel(true);
        }

        if (!IsCatting)
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
            while (ContentText.text != NowList[ChatCount]["Context" + _conversationCount].ToString())
            {
                ContentText.text += NowList[ChatCount]["Context" + _conversationCount].ToString()[WordCount];
                yield return new WaitForSeconds(0.075f);
                WordCount++;
            }

            EndTyping();
        }
    }

    private void EndTyping()
    {
        if(ContentText.text != NowList[ChatCount]["Context" + _conversationCount].ToString())
        {
            ContentText.text = NowList[ChatCount]["Context" + _conversationCount].ToString();
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

    public void SetSelect(int SelectCount, ref List<Dictionary<string, object>> SelectList)
    {
        _selectPanelStillOpen = true;
        ChatCount = SelectCount;
        _conversationCount = 1;
        NowList = SelectList;
        IsCanChat = true;
    }

    public void StartSelect()
    {
        if (!SelectPanel.gameObject.activeSelf)
        {
            SelectPanel.gameObject.SetActive(true);

            try
            {
                if (NowList[ChatCount]["Context2"].ToString().Length == 0)
                {
                    //Debug.Log("���� ���� ���Ⱑ 1���� ���");
                    if (SelectButtonList[1].GetComponent<TextMeshProUGUI>().text.Length != 0)
                    {
                        //Debug.Log("�������� 2�� ���⸦ ������");
                        SelectPanel.constraintCount = 2;
                        
                    }
                    //Debug.Log("�������� 1�� ���⸦ ������");
                }
                else
                {
                    //Debug.Log("���� ���� ���Ⱑ 2���� ���");
                    if (SelectButtonList[1].GetComponent<TextMeshProUGUI>().text.Length != 0)
                    {
                        //Debug.Log("�������� 1�� ���⸦ ������");
                        SelectPanel.constraintCount = 1;
                    }
                    //Debug.Log("�������� 2�� ���⸦ ������");
                }
            }
            catch
            {
                //Debug.Log("������ ���� ���Ⱑ ���� ��� " + NowList[ChatCount]["Context2"].ToString().Length);
                if (NowList[ChatCount]["Context2"].ToString().Length == 0)
                {
                    //Debug.Log("1�� ���⸦ ������");
                    SelectPanel.constraintCount = 1;
                }
                else
                {
                    //Debug.Log("2�� ���⸦ ������");
                    SelectPanel.constraintCount = 2;
                }
            }

            for (int i = 0; i < NowList[ChatCount].Count - 2; i++)
            {
                if (NowList[ChatCount]["Context" + _conversationCount].ToString().Length != 0)
                {
                    if (!SelectButtonList[i])
                    {
                        SelectButtonList[i] = Instantiate(SelectButton, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Button;
                        SelectButtonList[i].transform.SetParent(SelectPanel.transform, false);
                        SelectButtonList[i].transform.SetParent(SelectPanel.transform, false);
                    }

                    SelectButtonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = NowList[ChatCount]["Context" + _conversationCount].ToString();
                    int TemType = i;
                    SelectButtonList[i].onClick.AddListener(() =>
                    {
                        _conversationCount++;
                        SelectEvent.Invoke(TemType);
                        NextConversation();
                        SelectPanel.gameObject.SetActive(false);
                    });
                    RealSelectCount++;
                    if (i % 2 == 0)
                    {
                        TotalCount += 2;
                    }
                }
                else
                {
                    if(SelectButtonList[i])
                    {
                        Destroy(SelectButtonList[i].gameObject);
                        SelectButtonList[i] = null;
                    }
                }
                
                _conversationCount++;
            }

            //Debug.Log("TotalCount " + TotalCount);
            for (int RemainButton = TotalCount; RemainButton < SelectButtonList.Count; RemainButton++)
            {
                if (SelectButtonList[RemainButton])
                {
                    //Debug.Log("SelectButtonList[" + RemainButton + "]�� ���� �ƴ϶� ����");
                    Destroy(SelectButtonList[RemainButton].gameObject);
                    SelectButtonList[RemainButton] = null;
                }
                else
                {
                    //Debug.Log("SelectButtonList[" + RemainButton + "]�� ���̶� �Ѿ");
                }

            }
            _conversationCount--;
            Character.instance.MyPlayerController.ConversationNext = true;
        }
        else
        {
            if (SelectedButton != -1)
            {
                SelectButtonList[SelectedButton].image.color = Color.white;
                SelectButtonList[SelectedButton].onClick.Invoke();

                SelectedButton = -1;
                TotalCount = 0;
                RealSelectCount = 0;
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
                //Debug.Log("ó�� ���� SelectedButton " + SelectedButton);
            }
            else
            {
                //Debug.Log("���� SelectedButton " + SelectedButton);
                SelectButtonList[SelectedButton].image.color = Color.white;
                if (Direction == KeyDirection.Down)
                {
                    SelectUpDown(2);
                }
                else if (Direction == KeyDirection.Up)
                {
                    SelectUpDown(-2);
                }
                else if (Direction == KeyDirection.Right)
                {
                    SelectLeftRight(1);
                }
                else if (Direction == KeyDirection.Left)
                {
                    SelectLeftRight(-1);
                }
            }
        }
    }
    private void SelectUpDown(int AddOrder)
    {
        try
        {
            //Debug.Log(SelectedButton + " + " + AddOrder);
            SelectButtonList[SelectedButton + AddOrder].image.color = SelectColor;
            SelectedButton += AddOrder;
            Debug.Log("���� SelectedButton " + SelectedButton);
        }
        catch
        {
            try
            {
                //Debug.Log(SelectedButton + AddOrder + " ? " + RealSelectCount);
                // �Ʒ��� �Ѿ
                if (SelectedButton + AddOrder >= RealSelectCount)
                {
                    //Debug.Log(SelectedButton + AddOrder + "�Ʒ��� �Ѿ");
                    switch (SelectedButton % 2)
                    {
                        case 0:
                            SelectButtonList[0].image.color = SelectColor;
                            SelectedButton = 0;
                            break;
                        case 1:
                            SelectButtonList[1].image.color = SelectColor;
                            SelectedButton = 1;
                            break;
                    }
                }
                // ���� �Ѿ
                else
                {
                    //Debug.Log(SelectedButton + AddOrder + "���� �Ѿ");
                    int LineCount = (TotalCount / 2 + TotalCount % 2) - 1;
                    int FinalSelect = SelectedButton + (math.abs(AddOrder) * LineCount);
                    //Debug.Log("LineCount " + LineCount);
                    //Debug.Log(FinalSelect);
                    SelectButtonList[FinalSelect].image.color = SelectColor;
                    SelectedButton = FinalSelect;

                }
                Debug.Log("���� SelectedButton " + SelectedButton);
            }
            catch
            {
                SelectButtonList[SelectedButton].image.color = SelectColor;
            }
        }
    }
    private void SelectLeftRight(int AddOrder)
    {
        try
        {
            //Debug.Log(SelectedButton + " + " + AddOrder);
            SelectButtonList[SelectedButton + AddOrder].image.color = SelectColor;
            SelectedButton += AddOrder;
            Debug.Log("���� SelectedButton " + SelectedButton);
        }
        catch
        {
            //Debug.Log(SelectedButton + AddOrder + " ? " + TotalCount);
            try
            {
                if (SelectedButton + AddOrder >= RealSelectCount)
                {
                    //Debug.Log(SelectedButton + AddOrder - TotalCount + "������ �Ѿ");
                    SelectButtonList[SelectedButton + AddOrder - TotalCount].image.color = SelectColor;
                    SelectedButton = SelectedButton + AddOrder - TotalCount;
                }
                else
                {
                    //Debug.Log(SelectedButton + AddOrder + "�ڷ� �Ѿ");
                    SelectButtonList[TotalCount - 1].image.color = SelectColor;
                    SelectedButton = TotalCount - 1;
                }
                Debug.Log("���� SelectedButton " + SelectedButton);
            }
            catch
            {
                SelectButtonList[SelectedButton].image.color = SelectColor;
            }
        }
    }
}