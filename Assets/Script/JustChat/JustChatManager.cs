using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JustChatManager : MonoBehaviour
{
    private GameObject MainCanvas;
    public ConversationManager ConversationManager;

    private WaitForFixedUpdate WaitFixedUpdate;
    private WaitForSeconds WaitSeconds;
    private bool Trigger;

    private BasicNpc JustChatNpc;
    private Portal JustChatPortal;

    private bool _isWorking;


    public bool IsWorking
    {
        set
        {
            _isWorking = value;
        }
        get
        {
            return _isWorking;
        }
    }

    void Awake()
    {
        WaitFixedUpdate = new WaitForFixedUpdate();
        WaitSeconds = new WaitForSeconds(0.1f);
        Trigger = true;
        MainCanvas = GameObject.Find("Main Canvas");

        ConversationManager = MainCanvas.transform.GetChild(0).GetChild(4).GetComponent<ConversationManager>();

        _isWorking = false;
    }
    void Start()
    {
        /*Character.instance.SetCharacterInput(true, true, false);
        JustChatPortal = (Instantiate(Resources.Load("Prefabs/Object/Door"), new Vector3(6.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<Portal>();
        JustChatPortal.SceneName = "0004";*/



        SetJustChat();
        Character.instance.SetCharacterInput(false, false, false);
    }

    private void SetJustChat()
    {
        switch (Character.instance.MyMapNumber)
        {
            case "0002":
                JustChatNpc = (Instantiate(Resources.Load("Prefabs/NPC/JustChatNPC"), new Vector3(5.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<BasicNpc>();
                JustChatNpc.SetNpcName("펜던트 소녀");
                JustChatNpc.SetChatType(8);
                JustChatPortal = (Instantiate(Resources.Load("Prefabs/Object/Door"), new Vector3(6.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<Portal>();
                JustChatPortal.SceneName = "0004";
                break;
            case "0102":
                ChangeNpcNumberChatType("1-0");
                Character.instance.MyPlayerController.EventConversation.Invoke();
                StartCoroutine(WaitChat());
                StartCoroutine(StartMove());
                break;
            case "0202":

                break;
            case "0302":
                ChangeNpcNumberChatType("2-0");
                Character.instance.MyPlayerController.EventConversation.Invoke();
                StartCoroutine(WaitChat());
                StartCoroutine(StartMove());
                break;
            case "0402":

                break;
            case "0502":

                break;
            case "0602":

                break;
            case "0702":

                break;
            case "0802":

                break;

        }
    }

    public IEnumerator MoveCharacterToInterActive(float XPosition, int XDirection, bool InterActive)
    {
        while(_isWorking)
        {
            yield return WaitFixedUpdate;
        }

        _isWorking = true;
        switch (XDirection)
        {
            case 0:
                while (Character.instance.transform.position.x <= XPosition)
                {
                    Character.instance.MyPlayerController.SetPlayerPosition(XDirection);

                    yield return WaitFixedUpdate;
                }
                break;
            case 1:
                while (Character.instance.transform.position.x >= XPosition)
                {
                    Character.instance.MyPlayerController.SetPlayerPosition(XDirection);

                    yield return WaitFixedUpdate;
                }
                break;
        }
        Character.instance.SetCharacterInput(false, false, false);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);

        if (InterActive)
        {
            Character.instance.MyPlayerController.SetInputX();
            while (Character.instance.MyPlayerController.CharacterControllable != true)
            {
                yield return WaitFixedUpdate;
            }
            Character.instance.SetCharacterInput(false, false, false);
        }
        _isWorking = false;
    }

    public IEnumerator WaitChat()
    {
        while (_isWorking)
        {
            yield return WaitFixedUpdate;
        }

        _isWorking = true;
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false, false);
        _isWorking = false;
    }

    public IEnumerator Wait(int Count)
    {
        while (_isWorking)
        {
            yield return WaitFixedUpdate;
        }

        _isWorking = true;
        for (int i = 0; i < Count; i++)
        {
            yield return WaitSeconds;
        }
        _isWorking = false;
    }

    public void ChangeNpcNumberChatType(string NewNpcNumberChatType)
    {
        ConversationManager.NpcNumberChatType = NewNpcNumberChatType;
    }

    public void CreateNpc(Vector3 CharacterPos, int NpcNumber, int ChatType, string NpcName)
    {
        JustChatNpc = (Instantiate(Resources.Load("Prefabs/NPC/JustChatNPC"), CharacterPos, Quaternion.identity) as GameObject).GetComponent<BasicNpc>();
        JustChatNpc.SetNpcNumber(NpcNumber);
        JustChatNpc.SetChatType(ChatType);
        JustChatNpc.SetNpcName(NpcName);
    }

    public void SetCollider()
    {
        JustChatNpc.GetComponent<Collider2D>().enabled = true;
    }

    public void CreatePortal(Vector3 PortalPos, string MapNumber)
    {
        JustChatPortal = (Instantiate(Resources.Load("Prefabs/Object/Door"), PortalPos, Quaternion.identity) as GameObject).GetComponent<Portal>();
        JustChatPortal.SceneName = MapNumber;
    }

    public IEnumerator StartMove()
    {
        while (_isWorking)
        {
            yield return WaitFixedUpdate;
        }

        Character.instance.SetCharacterInput(true, true, false);
    }

    IEnumerator Tutorial()
    {
        Character.instance.SetCharacterInput(false, false, false);
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
        Character.instance.SetCharacterInput(false, false, false);

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
        Character.instance.SetCharacterInput(false, false, false);
        
        // 3 대사 / 대기
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.MyPlayerController.EventConversation.Invoke();

        while (ConversationManager.ConversationCount != -1f)
        {
            //Character.instance.MyPlayerController.SetPlayerPosition(-1);
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false, false);

        // 4 오브젝트 생성
        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(4.5f, 0.5f, 0f), Quaternion.identity);
        //Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false, false);
        yield return WaitSeconds;
        yield return WaitSeconds;
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
        Character.instance.SetCharacterInput(false, false, false);
        
        // 6 이동 / 대사 / 대기
        while (Character.instance.transform.position.x <= 3f)
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
        Character.instance.SetCharacterInput(false, false, false);
        
        // 7 공격
        //Character.instance.SetCharacterInput(false, false);
        Character.instance.MyPlayerController.SetPlayerPosition(0);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.MyPlayerController.SetInputX();
        while (Character.instance.MyPlayerController.CharacterControllable != true)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false, false);

        // 8 오브젝트 생성
        JustChatNpc = (Instantiate(Resources.Load("Prefabs/NPC/JustChatNPC"), new Vector3(5.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<BasicNpc>();
        JustChatNpc.SetChatType(8);
        
        Character.instance.MyPlayerController.SetPlayerPosition(0);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false, false);
        yield return WaitSeconds;
        Character.instance.MyPlayerController.SetPlayerPosition(0);
        Character.instance.MyPlayerController.SetPlayerPosition(-1);
        Character.instance.SetCharacterInput(false, false, false);

        // 9 대사 / 대기
        ConversationManager.NpcNumberChatType = "0-3";
        //ConversationManager.ConversationPanelStillOpen = true;
        Character.instance.MyPlayerController.EventConversation.Invoke();
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false, false);
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        // 10 대사 / 대기
        ConversationManager.NpcNumberChatType = "0-4";
        Character.instance.MyPlayerController.EventConversation.Invoke();
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false, false);

        // 11 대사 / 대기
        ConversationManager.NpcNumberChatType = "0-5";
        Character.instance.MyPlayerController.EventConversation.Invoke();
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false, false);

        // 12 상인 대장장이 선택지 보여주기


        // 13  대사 / 대기
        ConversationManager.NpcNumberChatType = "0-7";
        //ConversationManager.ConversationPanelStillOpen = false;
        Character.instance.MyPlayerController.EventConversation.Invoke();
        while (ConversationManager.ConversationCount != -1f)
        {
            yield return WaitFixedUpdate;
        }
        Character.instance.SetCharacterInput(false, false, false);



        JustChatNpc.GetComponent<Collider2D>().enabled = true;
        Character.instance.SetCharacterInput(true, true, false);

        // 14 선택한 직업에 맞는 위치로 보내주기
        JustChatPortal = (Instantiate(Resources.Load("Prefabs/Object/Door"), new Vector3(6.5f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<Portal>();
        JustChatPortal.SceneName = "0004";
    }
}