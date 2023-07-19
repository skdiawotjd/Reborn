using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Mathematics;
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
    List<Dictionary<string, object>> NowList;
    [SerializeField]
    private BasicNpc _curNpc;                   // 대화 할 Npc
    [SerializeField]
    private bool IsCanChat;                     // 채팅을 시작할 수 있는지
    [SerializeField]
    private bool IsCatting;                     // 채팅이 진행 중인지
    [SerializeField]
    private Coroutine TypingCourtine;           // 채팅 코루틴을 정지하기 위한 변수
    [SerializeField]
    private bool _conversationPanelStillOpen;   // 채팅 패널이 계속 열려 있어야 하는지
    [SerializeField]
    private bool _selectPanelStillOpen;         // 선택 패널이 계속 열려 있어야 하는지
    /// <summary>
    /// CSV에서 몇번 째 줄인지
    /// </summary> 
    [SerializeField]
    private int ChatCount;
    /// <summary>
    /// 대화가 몇번 진행 되었는지
    /// </summary> 
    [SerializeField]
    private int _conversationCount;
    /// <summary>
    /// CSV에서 Content0에 있는 대사 이름 스트링
    /// </summary> 
    [SerializeField]
    private string ChatName;
    [SerializeField]
    private bool OutsideSelect;                 // 외부에서 선택을 사용하는 경우
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
        OutsideSelect = false;
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
    }

    protected override void Start()
    {
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



    //public void SetSelect(ref int ConversationCount, ref List<Dictionary<string, object>> SelectList)
    public void SetSelect(ref List<Dictionary<string, object>> SelectList)
    {
        // 최초로 외부 선택을 이용하는 것인지 확인
        OutsideSelect = true;
        //_selectPanelStillOpen = true;

        // 외부 데이터 가져오기
        //ChatCount = ConversationCount;
        //ChatCount = 0;
        NowList = SelectList;

        // 내부 데이터 셋
        _conversationCount = 1;
        SetConversationCount(0);
        //ChatName = NowList[ChatCount]["Context0"].ToString();
        IsCanChat = true;
    }

    public void SetConversationCount(int ConversationCount)
    {
        ChatCount = ConversationCount;
        ChatName = NowList[ChatCount]["Context0"].ToString();
        
    }

    private void NextConversation()
    {
        if (IsCanChat)
        {
            //Debug.Log("대사 시작 3 ConversationCount가 (" + ConversationCount + " < " + (ChatList[ChatCount].Count - 1) + ") 이면");
            if (_conversationCount < NowList[ChatCount].Count - 1)
            {
                //Debug.Log("대사 시작 4 - " + "ChatList[" + ChatCount + "][Context" + ConversationCount + "] 출력");
                if (IsCatting)
                {
                    StopCoroutine(TypingCourtine);
                    EndTyping();
                }
                else
                {
                    if (ChatName.Length != 0)
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
                //Debug.Log("1");
                //Debug.Log("대사 시작 5 - ConversationCount가 ChatList[NpcToChat].Count보다 크므로 대사 끝");
                SetActivePanel(_conversationPanelStillOpen);
                _conversationPanelStillOpen = false;

                //Debug.Log("대사 시작 6 - 대사 끝");
                if (_conversationCount == NowList[ChatCount].Count - 1)
                {
                    //Debug.Log("2");
                    //Debug.Log("대사 시작 6 - 대사 끝");
                    if (_curNpc != null)
                    {
                        //Debug.Log("3");
                        StartCoroutine(EndConversation(false, 0.15f));
                    }
                    else
                    {
                        //Debug.Log("4");
                        if (OutsideSelect)
                        {
                            //Debug.Log("5");
                            _conversationCount = 1;
                        }
                        else
                        {
                            //Debug.Log("6");
                            _conversationCount = -1;
                            IsCanChat = false;
                        }

                        Character.instance.MyPlayerController.ConversationNext = false;
                    }
                }
            }
        }
    }

    private void SetChatName()
    {
        try
        {
            NameText.text = CharacterNameList[ChatName[_conversationCount - 1] - '0']["CharacterName"].ToString();
        }
        catch
        {
            NameText.text = CharacterNameList[1]["CharacterName"].ToString();
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
            //Debug.Log("Typing - " + Character.instance.MyPlayerController.ConversationNext);
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
            

            //Debug.Log(ContentText.text + " != " + ChatList[ChatCount]["Context" + ConversationCount].ToString());
            while (ContentText.text != NowList[ChatCount]["Context" + _conversationCount].ToString())
            {
                ContentText.text += NowList[ChatCount]["Context" + _conversationCount].ToString()[WordCount];
                yield return YieldCache.WaitForSeconds(0.075f);
                WordCount++;
            }

            EndTyping();
        }
    }

    private void EndTyping()
    {
        if (ContentText.text != NowList[ChatCount]["Context" + _conversationCount].ToString())
        {
            ContentText.text = NowList[ChatCount]["Context" + _conversationCount].ToString();
        }

        IsCatting = false;
        _conversationCount++;
    }

    IEnumerator EndConversation(bool Next, float WaitTime)
    {
        _conversationCount = -1;
        IsCanChat = false;
        yield return new WaitForSeconds(WaitTime);

        
        ChatName = "";
        Character.instance.MyPlayerController.ConversationNext = Next;
        //Debug.Log("EndConversation - " + Character.instance.MyPlayerController.ConversationNext);
        _curNpc.FunctionEnd();
    }

    public void StartSelect()
    {
        if (!SelectPanel.gameObject.activeSelf)
        {
            _conversationCount = 1;
            SelectPanel.gameObject.SetActive(true);
            
            try
            {
                if (NowList[ChatCount]["Context2"].ToString().Length == 0)
                {
                    //Debug.Log("새로 들어온 보기가 1열인 경우");
                    if (SelectButtonList[1].GetComponent<TextMeshProUGUI>().text.Length != 0)
                    {
                        //Debug.Log("이전에는 2열 보기를 생성함");
                        SelectPanel.constraintCount = 2;
                        
                    }
                    //Debug.Log("이전에도 1열 보기를 생성함");
                }
                else
                {
                    //Debug.Log("새로 들어온 보기가 2열인 경우");
                    if (SelectButtonList[1].GetComponent<TextMeshProUGUI>().text.Length != 0)
                    {
                        //Debug.Log("이전에는 1열 보기를 생성함");
                        SelectPanel.constraintCount = 1;
                    }
                    //Debug.Log("이전에도 2열 보기를 생성함");
                }
            }
            catch
            {
                //Debug.Log("이전에 만든 보기가 없는 경우 " + NowList[ChatCount]["Context2"].ToString().Length);
                if (NowList[ChatCount]["Context2"].ToString().Length == 0)
                {
                    //Debug.Log("1열 보기를 생성함");
                    SelectPanel.constraintCount = 1;
                }
                else
                {
                    //Debug.Log("2열 보기를 생성함");
                    SelectPanel.constraintCount = 2;
                }
            }

            //Debug.Log((NowList[ChatCount].Count - 2) + "만큼 for문 반복");
            for (int i = 0; i < NowList[ChatCount].Count - 2; i++)
            {
                if (NowList[ChatCount]["Context" + _conversationCount].ToString().Length != 0)
                {
                    //Debug.Log(i + "번 1-1 csv의" + i + "번에 버튼 내용이 있음");
                    // 널이 아니면
                    if (SelectButtonList[i])
                    {
                        //Debug.Log(i + "번 2-1 SelectButtonList[" + i + "] 가 널이 아니어서 버튼 생성하지 않음");
                        SelectButtonList[i].onClick.RemoveAllListeners();
                    }
                    // 널이면
                    else
                    {
                        //Debug.Log(i + "번 2-2 SelectButtonList[" + i + "] 가 널이어서 버튼 생성");
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
                    //Debug.Log(i + "번 1-2 csv의" + i + "번에 버튼 내용이 없음");
                    if (SelectButtonList[i])
                    {
                        //Debug.Log(i + "번 2-3 SelectButtonList[" + i + "] 에" + SelectButtonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text + "가 있는 버튼 지움");
                        Destroy(SelectButtonList[i].gameObject);
                        SelectButtonList[i] = null;
                    }
                }
                
                _conversationCount++;
            }

            /*Debug.Log(NowList[ChatCount].Count - 3);
            SelectButtonList[NowList[ChatCount].Count - 3].onClick.AddListener(() => { OutsideSelect = false; });*/
            //Debug.Log(NowList[ChatCount].Count - 4);
            SelectButtonList[NowList[ChatCount].Count - 4].onClick.AddListener(() => { OutsideSelect = false; });

            //Debug.Log("TotalCount " + TotalCount + " SelectButtonList.Count" + SelectButtonList.Count);
            for (int RemainButton = TotalCount; RemainButton < SelectButtonList.Count; RemainButton++)
            {
                if (SelectButtonList[RemainButton])
                {
                    //Debug.Log("SelectButtonList[" + RemainButton + "]이 널이 아니라서 지움");
                    //Debug.Log(SelectButtonList[RemainButton].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
                    
                    Destroy(SelectButtonList[RemainButton].gameObject);
                    SelectButtonList[RemainButton] = null;
                }
                else
                {
                    //Debug.Log("SelectButtonList[" + RemainButton + "]이 널이라서 넘어감");
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
                //Debug.Log("처음 선택 SelectedButton " + SelectedButton);
            }
            else
            {
                //Debug.Log("이전 SelectedButton " + SelectedButton);
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
            Debug.Log("현재 SelectedButton " + SelectedButton);
        }
        catch
        {
            try
            {
                //Debug.Log(SelectedButton + AddOrder + " ? " + RealSelectCount);
                // 아래로 넘어감
                if (SelectedButton + AddOrder >= RealSelectCount)
                {
                    //Debug.Log(SelectedButton + AddOrder + "아래로 넘어감");
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
                // 위로 넘어감
                else
                {
                    //Debug.Log(SelectedButton + AddOrder + "위로 넘어감");
                    int LineCount = (TotalCount / 2 + TotalCount % 2) - 1;
                    int FinalSelect = SelectedButton + (math.abs(AddOrder) * LineCount);
                    //Debug.Log("LineCount " + LineCount);
                    //Debug.Log(FinalSelect);
                    SelectButtonList[FinalSelect].image.color = SelectColor;
                    SelectedButton = FinalSelect;

                }
                Debug.Log("현재 SelectedButton " + SelectedButton);
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
            Debug.Log("현재 SelectedButton " + SelectedButton);
        }
        catch
        {
            //Debug.Log(SelectedButton + AddOrder + " ? " + TotalCount);
            try
            {
                if (SelectedButton + AddOrder >= RealSelectCount)
                {
                    //Debug.Log(SelectedButton + AddOrder - TotalCount + "앞으로 넘어감");
                    SelectButtonList[SelectedButton + AddOrder - TotalCount].image.color = SelectColor;
                    SelectedButton = SelectedButton + AddOrder - TotalCount;
                }
                else
                {
                    //Debug.Log(SelectedButton + AddOrder + "뒤로 넘어감");
                    SelectButtonList[TotalCount - 1].image.color = SelectColor;
                    SelectedButton = TotalCount - 1;
                }
                Debug.Log("현재 SelectedButton " + SelectedButton);
            }
            catch
            {
                SelectButtonList[SelectedButton].image.color = SelectColor;
            }
        }
    }
}