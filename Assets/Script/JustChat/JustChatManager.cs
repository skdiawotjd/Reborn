using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustChatManager : MonoBehaviour
{
    private GameObject MainCanvas;
    private ConversationManager ConversationManager;

    private WaitForFixedUpdate WaitFixedUpdate;
    private WaitForSeconds WaitSeconds;
    private bool Trigger;

    void Awake()
    {
        WaitFixedUpdate = new WaitForFixedUpdate();
        WaitSeconds = new WaitForSeconds(0.44f);
        Trigger = true;
        MainCanvas = GameObject.Find("Main Canvas");

        ConversationManager = MainCanvas.transform.GetChild(0).GetChild(4).GetComponent<ConversationManager>();
    }
    void Start()
    {
        Character.instance.SetCharacterInput(true, false);

        //SetJustChat();
    }

    private void SetJustChat()
    {
        switch (Character.instance.MyMapNumber)
        {
            case "0007":
                StartCoroutine(Tutorial());
                break;
        }
    }


    IEnumerator Tutorial()
    {
        // 0 ������Ʈ ����
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-4f, 0.5f, 0f), Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-7f, 0.5f, 0f), Quaternion.identity);

        // 1 �̵� / ����
        while (Character.instance.transform.position.x >= -3f)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(1);

            yield return WaitFixedUpdate;
        }

        Character.instance.MyPlayerController.SetInputX();
        while (Character.instance.MyPlayerController.CharacterControllable != true)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);

        // 2 �̵� / ����
        while (Character.instance.transform.position.x >= -6f)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(1);

            yield return WaitFixedUpdate;
        }

        Character.instance.MyPlayerController.SetInputX();
        while (Character.instance.MyPlayerController.CharacterControllable != true)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);
        /*Character.instance.MyPlayerController.SetInputX();
        yield return WaitSeconds;
        Character.instance.SetCharacterInput(false, false);*/

        // 3 ��� / ���
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(-1);
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);

        // 4 ������Ʈ ����
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(5f, 0.5f, 0f), Quaternion.identity);
        yield return WaitSeconds;
        yield return WaitSeconds;

        // 5 ��� / ���
        ConversationManager.NpcNumberChatType = "0-1";
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            if (Trigger && ConversationManager.TotalCount - ConversationManager.ConversationCount == 1)
            {
                Character.instance.MyPlayerController.SetPlayerPosition(0);
                Character.instance.MyPlayerController.SetPlayerPosition(-1);
                Trigger = false;
            }

            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);

        // 6 �̵� / ��� / ���
        while (Character.instance.transform.position.x <= 4f)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(0);

            yield return WaitFixedUpdate;
        }
        Character.instance.MyPlayerController.SetPlayerPosition(-1);

        ConversationManager.NpcNumberChatType = "0-2";
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(-1);
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);

        // 7 ����
        Character.instance.MyPlayerController.SetInputX();
        while (Character.instance.MyPlayerController.CharacterControllable != true)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false);

        // 8 ������Ʈ ����
        Instantiate(Resources.Load("Prefabs/NPC/JustChatNPC"), new Vector3(5.5f, 0f, 0f), Quaternion.identity);
        yield return WaitSeconds;

        // 6 ��� / ���
        ConversationManager.NpcNumberChatType = "0-3";
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(true, false);
    }
}