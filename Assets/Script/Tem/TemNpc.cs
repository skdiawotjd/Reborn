using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemNpc : MonoBehaviour
{
    List<Dictionary<string, object>> chattingList;
    List<string> chatList;
    int chatListCount;
    private Image ChatPanel;
    private TextMeshProUGUI chatText;
    private bool chatActive = false;

    // Start is called before the first frame update
    void Start()
    {
/*        ChatPanel = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Image>();
        chatText = ChatPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        chattingList = CSVReader.Read("Chatting");
        chatList = new List<string>();
        ChatGenerate();*/
    }

    // Update is called once per frame
    void Update()
    {
/*        if(chatActive)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(WaitingTime(2f));
            }
        }*/
    }

    private void ChatGenerate()
    {
        for(int i = 1; i < chattingList[0].Count; i++)
        {
            if(chattingList[0]["Context" + i].ToString() == "")
            {
                i = chattingList[0].Count;
            } else
            {
                chatList.Add(chattingList[0]["Context" + i].ToString());
            }
            
        }  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Npc에서 충돌체크 " + collision.gameObject.name);
        if (collision.gameObject.name == "R_Weapon" )
        {
            /*            ChatPanel.gameObject.SetActive(true);
                        Character.instance.SetCharacterInput(false, false);
                        chatActive = true;
                        Character.instance.MyPlayerController.ConversationNext = true;
                        chatText.text = chatList[Random.Range(0, chatList.Count)];*/
            //Character.instance.MyPlayerController.ConversationNext = true;
            //Character.instance.MyPlayerController.EventConversation.Invoke();
            Debug.Log("대사 시작 2 - 콜리전 충돌(NPC)");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    IEnumerator WaitingTime(float delayTime)
    {
        yield return delayTime;
        ChatPanel.gameObject.SetActive(false);
        Character.instance.SetCharacterInput(true, true);
        chatActive = false;
        Character.instance.MyPlayerController.ConversationNext = false;
    }
}
