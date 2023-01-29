using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int _step;
    private int _stepDetail;

    private string NextMapNumber;

    private int Step
    {
        set
        {
            _step = value;
            CheckStep();
        }
        get
            { return _step; }
    }
    public int StepDetail
    {
        set
        {
            _stepDetail = value;
            CheckStep();
        }
        get { return _stepDetail; }
    }

    [SerializeField]
    private JustChatManager JustChatManager;

    void Awake()
    {
        DontDestroyOnLoad(this);

        _step = 0;
        _stepDetail = 0;

    }

    void Start()
    {
        GameManager.instance.AddSceneMoveEvent(SetTutorial);
    }

    private void SetTutorial()
    {
        if ((int)Character.instance.MySocialClass < 1)
        {
            ///
            JustChatManager = GameObject.Find("JustChatManager").GetComponent<JustChatManager>();
            _step = 2;
            _stepDetail = 1;
            ///
            GameManager.instance.RemoveSceneMoveEvent(SetTutorial);
            GameManager.instance.AddSceneMoveEvent(CheckStep);
            CheckStep();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void CheckStep()
    {
        /*Debug.Log("Step - " + Step);
        Debug.Log("StepDetail - " + StepDetail);*/
        switch (Step)
        {
            case 0:
                // 저챗에서 대사
                switch (StepDetail)
                {
                    case 0:
                        JustChatManager = GameObject.Find("JustChatManager").GetComponent<JustChatManager>();
                        StepDetail++;
                        break;
                    case 1:
                        // 1 오브젝트 생성/이동/공격 Debug.Log("");
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-4f, 0.5f, 0f), Quaternion.identity);
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-7f, 0.5f, 0f), Quaternion.identity);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(-3f, 1, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 2:
                        // 2 이동/공격 Debug.Log("");
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(-6f, 1, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 3:
                        // 3 대화/대기 Debug.Log("");
                        JustChatManager.ConversationManager.SetChatName(Character.instance.MyName);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 4:
                        // 4 오브젝트 생성/대기 Debug.Log("");
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(4.5f, 0.5f, 0f), Quaternion.identity);
                        StartCoroutine(JustChatManager.Wait(7));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 5:
                        // 5 캐릭터 방향 변경 Debug.Log("");
                        Character.instance.MyPlayerController.SetPlayerPosition(0);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        // 5 대화/대기 Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-1");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 6:
                        // 6 이동/공격 Debug.Log("");
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(3f, 0, false));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 7:
                        // 7 대화/대기 Debug.Log("");
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        JustChatManager.ChangeNpcNumberChatType("0-2");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 8:
                        // 8 이동x/공격 Debug.Log("");
                        Character.instance.SetCharacterInput(false, false, false);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(Character.instance.transform.position.x, 0, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 9:
                        // 9 오브젝트 생성/대기 Debug.Log("");
                        JustChatManager.CreateNpc(new Vector3(5.5f, 0f, 0f), 0, 8, "펜던트소녀");
                        StartCoroutine(JustChatManager.Wait(7));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 10:
                        // 10 대화/대기 Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-3");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());


                        StartCoroutine(NextStepDetail());
                        break;
                    case 11:
                        // 11 대화/대기 Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-4");
                        JustChatManager.ConversationManager.SetChatName("???");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());


                        StartCoroutine(NextStepDetail());
                        break;
                    case 12:
                        // 12 대화/대기 Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-5");
                        //JustChatManager.ConversationManager.CurNpc = JustChatManager.JustChatNpc;
                        JustChatManager.ConversationManager.SetChatName("펜던트소녀");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 13:
                        // 13 선택 창 Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-6");
                        JustChatManager.ConversationManager.AddSelectEvent(SelectJob);
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 14:
                        // 14 대화/대기 Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-7");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 15:
                        // 15 콜라이더 설정/오브젝트 생성/끝 Debug.Log("");
                        JustChatManager.SetCollider();
                        JustChatManager.CreatePortal(new Vector3(6.5f, 0f, 0f), NextMapNumber);
                        Character.instance.SetCharacterInput(true, true, false);

                        _step = 1;
                        _stepDetail = 0;
                        break;
                }
                break;
            case 1:
                // 각 직업 별 게임 5번
                switch (StepDetail)
                {
                    case 0:
                        // 2 이동/공격 Debug.Log("");
                        //StartCoroutine(JustChatManager.MoveCharacterToInterActive(-5.5f, 1, true));

                        StartCoroutine(NextStepDetail());
                        break;
                }
                break;
        }
    }

    IEnumerator NextStepDetail()
    {
        while (JustChatManager.IsWorking)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StepDetail++;
    }

    private void SelectJob(int Job)
    {
        switch(Job)
        {
            case 0:
                Debug.Log("대장장이 선택");
                NextMapNumber = "0001";
                break;
            case 1:
                Debug.Log("상인 선택");
                NextMapNumber = "0201";
                break;
        }
    }
}
