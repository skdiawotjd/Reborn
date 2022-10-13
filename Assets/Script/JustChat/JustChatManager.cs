using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustChatManager : MonoBehaviour
{
    private GameObject MainCanvas;
    private ConversationManager ConversationManager;

    void Awake()
    {
        MainCanvas = GameObject.Find("Main Canvas");

        ConversationManager = MainCanvas.transform.GetChild(0).GetChild(1).GetComponent<ConversationManager>();
    }
    void Start()
    {
        MainCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        ConversationManager.NpcToChat = 0;
        Character.instance.SetCharacterInput(false, false);
        Character.instance.MyPlayerController.EventConversation.Invoke();

        StartCoroutine(CompleteChat());
    }

    IEnumerator CompleteChat()
    {
        while(ConversationManager.ConversationCount != 1)
        {
            yield return new WaitForSeconds(0.1f);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
