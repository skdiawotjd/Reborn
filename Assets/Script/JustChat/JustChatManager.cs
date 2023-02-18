using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class JustChatManager : MonoBehaviour
{
    private GameObject MainCanvas;
    public ConversationManager ConversationManager;

    private WaitForFixedUpdate WaitFixedUpdate;
    private WaitForSeconds WaitSeconds;

    private BasicNpc JustChatNpc;
    private List<BasicNpc> NpcList;
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
        WaitFixedUpdate = YieldCache.WaitForFixedUpdate;
        WaitSeconds = new WaitForSeconds(0.1f);
        MainCanvas = GameObject.Find("Main Canvas");
        NpcList = new List<BasicNpc>();

        ConversationManager = MainCanvas.transform.GetChild(0).GetChild(4).GetComponent<ConversationManager>();

        _isWorking = false;
    }
    void Start()
    {
        //Character.instance.SetCharacterInput(false, false, false);
        SetJustChat();
    }
    public void SetJustChatNPC(BasicNpc npc)
    {
        JustChatNpc = npc;
    }
    public void SetJustChatPortal(Portal portal)
    {
        JustChatPortal = portal;
    }
    /*private void asd()
    {
        Character.instance.MyPlayerController.EndDie();
    }*/
    private void SetJustChat()
    {
        switch (Character.instance.MyMapNumber)
        {
            case "0002":
                /*JustChatNpc = Instantiate(JustChatNpc, new Vector3(5.5f, 0f, 0f), Quaternion.identity);
                JustChatPortal = Instantiate(JustChatPortal, new Vector3(6.5f, 0f, 0f), Quaternion.identity);
                Character.instance.SetCharacterInput(true, true, false);
                JustChatPortal.SceneName = "0001";*/

                /*Character.instance.MyPlayerController.StartDie();
                Invoke("asd", 2f);*/
                break;
            case "0102":
                Character.instance.MyPlayerController.SetPlayerPosition(-1);
                ChangeNpcNumberChatType("2-0");
                Character.instance.MyPlayerController.InvokeEventConversation();
                StartCoroutine(WaitChat());
                StartCoroutine(StartMove());
                break;
            case "0202":

                break;
            case "0302":
                ChangeNpcNumberChatType("2-0");
                Character.instance.MyPlayerController.InvokeEventConversation();
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
            yield return YieldCache.WaitForFixedUpdate;
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
        if(!ConversationManager)
        {
            //Debug.Log("ConversationManager가 없음");
            MainCanvas = GameObject.Find("Main Canvas");
            ConversationManager = MainCanvas.transform.GetChild(0).GetChild(4).GetComponent<ConversationManager>();
        }
        ConversationManager.NpcNumberChatType = NewNpcNumberChatType;
    }

    public int CreateNpc(Vector3 CharacterPos, int NpcNumber, int ChatType, int CharacterNumber)
    {
        NpcList.Add(Instantiate(JustChatNpc, CharacterPos, Quaternion.identity));
        NpcList.Last().SetNpcNumber(NpcNumber);
        NpcList.Last().SetChatType(ChatType);
        // Npc의 CharacterNumber 설정하기

        return NpcList.Count;
    }

    public void SetCollider()
    {
        for(int i = 0; i < NpcList.Count; i++)
        {
            NpcList[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    public void CreatePortal(Vector3 PortalPos, string MapNumber)
    {
        JustChatPortal = Instantiate(JustChatPortal, PortalPos, Quaternion.identity);
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
}