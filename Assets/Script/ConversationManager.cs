using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConversationManager : MonoBehaviour
{
    private TextMeshProUGUI NameText;
    private TextMeshProUGUI ContentText;
    private GameObject ConversationPanel;

    private int ConversationCount;

    List<Dictionary<string, object>> chattingList;
    List<string> chatList;
    int chatListCount;
    private Image ChatPanel;
    private TextMeshProUGUI chatText;
    private bool chatActive = false;

    private void Awake()
    {
        
        ConversationCount = 5;

        ChatPanel = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Image>();
        chatText = ChatPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        chattingList = CSVReader.Read("Chatting");
        chatList = new List<string>();
        ChatGenerate();
    }

    // Start is called before the first frame update
    void Start()
    {
        Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
    }

    private void ChatGenerate()
    {
        for (int i = 1; i < chattingList[0].Count; i++)
        {
            if (chattingList[0]["Context" + i].ToString() == "")
            {
                i = chattingList[0].Count;
            }
            else
            {
                chatList.Add(chattingList[0]["Context" + i].ToString());
            }
        }
    }

    private void NextConversation()
    {
        if (ConversationCount > 1)
        {
            StartCoroutine(SetConversationNext(true, 0.1f));
            ChatPanel.gameObject.SetActive(true);
            chatText.text = chatList[Random.Range(0, chatList.Count)];
            ConversationCount--;
            Debug.Log("ConversationCount 현재 수치 : " + ConversationCount);
            Debug.Log("대사 시작 3 - Conversation의 NextConversation 함수 " + Character.instance.MyPlayerController.ConversationNext);
        }
        else
        {
            Debug.Log("ConversationCount가 1보다 크지 않음");
            ChatPanel.gameObject.SetActive(false);
            Character.instance.MyPlayerController.ConversationNext = true;
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
