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
        Character.instance.SetCharacterInput(false, false);

        Character.instance.MyPlayerController.EventConversation.Invoke();

        //StartCoroutine(CompleteChat());
    }


    private void CharacterMove()
    {
        Character.instance.MyPlayerController.SetPlayerPosition(0);
    }

    IEnumerator CompleteChat()
    {
        while (true)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(0);

            yield return new WaitForFixedUpdate();
        }
    }
}
