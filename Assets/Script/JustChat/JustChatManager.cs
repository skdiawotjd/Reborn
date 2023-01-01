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

    private BasicNpc JustChatNpc;
    private Portal JustChatPortal;

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
        SetJustChat();
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
        Character.instance.SetCharacterInput(false, false);
        // 0 오브젝트 생성
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-4f, 0.5f, 0f), Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-7f, 0.5f, 0f), Quaternion.identity);

        // 1 이동 / 공격
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

        // 2 이동 / 공격
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

        // 3 대사 / 대기
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            Character.instance.MyPlayerController.SetPlayerPosition(-1);
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);

        // 4 오브젝트 생성
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(5f, 0.5f, 0f), Quaternion.identity);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false);
        yield return WaitSeconds;
        yield return WaitSeconds;
        

        // 5 대사 / 대기
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

        // 6 이동 / 대사 / 대기
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

        // 7 공격
        Character.instance.MyPlayerController.SetInputX();
        while (Character.instance.MyPlayerController.CharacterControllable != true)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false);

        // 8 오브젝트 생성
        JustChatNpc = (Instantiate(Resources.Load("Prefabs/NPC/JustChatNPC"), new Vector3(5.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<BasicNpc>();

        Character.instance.MyPlayerController.SetPlayerPosition(0);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false);
        yield return WaitSeconds;
        Character.instance.MyPlayerController.SetPlayerPosition(0);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false);

        // 9 대사 / 대기
        ConversationManager.NpcNumberChatType = "0-3";
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            Debug.Log("wait");
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(true, false);

        // 10 캐릭터와 대사 / 대기
        /*JustChatNpc.SetChatType(4);

        Character.instance.MyPlayerController.SetInputX();
        Debug.Log("ConversationManager.ConversationCount" + ConversationManager.ConversationCount);
        while (ConversationManager.ConversationCount != 0f)
        {
            Debug.Log("ConversationManager.ConversationCount" + ConversationManager.ConversationCount);
            Character.instance.MyPlayerController.ConversationNext = true;
            yield return WaitFixedUpdate;
        }
        Debug.Log("2");
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.MyPlayerController.ConversationNext = false;
        Character.instance.SetCharacterInput(true, false);*/

        /*// 11 
        JustChatNpc.SetChatType(5);
        Character.instance.MyPlayerController.SetInputX();
        while (ConversationManager.ConversationCount != 0f)
        {
            Character.instance.MyPlayerController.ConversationNext = true;
            yield return WaitFixedUpdate;
        }
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);

        // 12 상인 대장장이 선택지 보여주기

        // 13  캐릭터와 대사 / 대기
        JustChatNpc.SetChatType(7);
        Character.instance.MyPlayerController.SetInputX();
        while (ConversationManager.ConversationCount != 0f)
        {
            Character.instance.MyPlayerController.ConversationNext = true;
            yield return WaitFixedUpdate;
        }
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false);*/

        // 14 선택한 직업에 맞는 위치로 보내주기
        JustChatPortal = (Instantiate(Resources.Load("Prefabs/Object/Door"), new Vector3(6.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<Portal>();
        JustChatPortal.SceneName = 14;
    }
}