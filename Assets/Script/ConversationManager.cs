using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ConversationManager : MonoBehaviour
{
    private TextMeshProUGUI NameText;
    private TextMeshProUGUI ContentText;
    private GameObject ConversationPanel;

    private int ConversationCount;

    private void Awake()
    {
        ConversationPanel = gameObject.transform.GetChild(0).gameObject;
        NameText = ConversationPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ContentText = ConversationPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        NameText.text = "admin";
        ConversationCount = 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        Character.instance.MyPlayerController.EventConversation.AddListener(() => { NextConversation(); });
    }

    private void NextConversation()
    {
        if (ConversationCount > 1)
        {
            //Invoke("asd", 1f);
            StartCoroutine(SetConversationNext(true, 0.1f));
            ContentText.text += ContentText.text;
            ConversationCount--;
            Debug.Log("대사 시작 3 - Conversation의 NextConversation 함수 " + Character.instance.MyPlayerController.ConversationNext);
        }
        else
        {
            Debug.Log("ConversationCount가 1보다 작지 않음");
            ConversationPanel.SetActive(false);
            Character.instance.MyPlayerController.ConversationNext = true;
            Character.instance.SetCharacterInput(true, true);

            ConversationCount = 3;
            //Invoke("qwe", 1f);
            StartCoroutine(SetConversationNext(false, 0.5f));
        }
    }

    IEnumerator SetConversationNext(bool Next, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Character.instance.MyPlayerController.ConversationNext = Next;
    }
}
