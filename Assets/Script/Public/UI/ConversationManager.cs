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
    private bool IsCanChat; // 대화를 시작할 수 있는지
    private int ChatCount; // CSV 상 대화가 몇번 째에 있는지
    private int _conversationCount; // 대화가 몇번 진행 되었는지
    private string _npcNumberChatType; // "NPC넘버-대화넘버"

    public int ConversationCount
    {
        get
        {
            return _conversationCount;
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
    }

    void Start()
    {
        Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
        GameManager.instance.AddGameStartEvent(InitializeNpcNumberChatType);
        GameManager.instance.AddDayEnd(DayEnd);
    }

    private void InitializeConversationManager()
    {
        IsCanChat = false;
        ChatCount = -1;
        _conversationCount = -1;
        NpcNumberChatType = "0-0";
        ConversationPanel.gameObject.SetActive(false);
    }

    private void SetChat(string NNN)
    {
        for (int i = 0; i < ChatList.Count; i++)
        {
            if (ChatList[i]["NpcNumber"].ToString() == NNN)
            {
                IsCanChat = true;
                ChatCount = i;
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
            /*Debug.Log("대사 시작 3 (" + ConversationCount + " < " + ChatList[NpcToChat].Count + ") 이면");
            if (ConversationCount < ChatList[NpcToChat].Count)
            {
                if (!ConversationPanel.gameObject.activeSelf)
                {
                    ConversationPanel.gameObject.SetActive(true);
                }
                Debug.Log("대사 시작 4 - " + "ChatList[" + NpcToChat + "][Context" + ConversationCount + "]");
                ContentText.text = GetChat();
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
            }*/
        }
    }

    IEnumerator ChatCoroutine()
    {
        while(!IsCanChat)
        {
            yield return new WaitForSeconds(0.016f);
        }

        //Debug.Log("대사 시작 3 (" + ConversationCount + " < " + (ChatList[ChatCount].Count - 1) + ") 이면");
        if (ConversationCount < ChatList[ChatCount].Count - 1)
        {
            if (!ConversationPanel.gameObject.activeSelf)
            {
                ConversationPanel.gameObject.SetActive(true);
            }
            //Debug.Log("대사 시작 4 - " + "ChatList[" + ChatCount + "][Context" + ConversationCount + "]");
            ContentText.text = ChatList[ChatCount]["Context" + ConversationCount].ToString();
            _conversationCount++;
            StartCoroutine(SetConversationNext(true, 0.1f));
        }
        else
        {
            //Debug.Log("대사 시작 5 - ConversationCount가 ChatList[NpcToChat].Count보다 크므로 대사 끝");
            ConversationPanel.gameObject.SetActive(false);

            if(CurNpc)
            {
                CurNpc.FunctionEnd();
            }
            if (ConversationCount == ChatList[ChatCount].Count - 1)
            {
                //Debug.Log("대사 시작 6 - 추가로 이어질 대사 없음");
                Character.instance.SetCharacterInput(true, true);
                StartCoroutine(SetConversationNext(false, 0.4f));
                IsCanChat = false;
                _conversationCount = -1;
            }
        }
    }

    IEnumerator SetConversationNext(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Character.instance.MyPlayerController.ConversationNext = Next;
        //Debug.Log("ConversationCount 현재 수치 : " + ConversationCount);
        //Debug.Log("받은 값 " + Next + " Conversation의 NextConversation 함수 " + Character.instance.MyPlayerController.ConversationNext);
    }

    private void DayEnd()
    {
        InitializeConversationManager();
    }

    private void InitializeNpcNumberChatType()
    {
        NpcNumberChatType = ((int)Character.instance.MyJob + 1).ToString() + "-0";
    }
}
