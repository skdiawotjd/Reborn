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
                // ��ê���� ���
                switch (StepDetail)
                {
                    case 0:
                        JustChatManager = GameObject.Find("JustChatManager").GetComponent<JustChatManager>();
                        StepDetail++;
                        break;
                    case 1:
                        // 1 ������Ʈ ����/�̵�/���� Debug.Log("");
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-4f, 0.5f, 0f), Quaternion.identity);
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-7f, 0.5f, 0f), Quaternion.identity);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(-3f, 1, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 2:
                        // 2 �̵�/���� Debug.Log("");
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(-6f, 1, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 3:
                        // 3 ��ȭ/��� Debug.Log("");
                        JustChatManager.ConversationManager.SetChatName(Character.instance.MyName);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 4:
                        // 4 ������Ʈ ����/��� Debug.Log("");
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(4.5f, 0.5f, 0f), Quaternion.identity);
                        StartCoroutine(JustChatManager.Wait(7));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 5:
                        // 5 ĳ���� ���� ���� Debug.Log("");
                        Character.instance.MyPlayerController.SetPlayerPosition(0);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        // 5 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-1");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 6:
                        // 6 �̵�/���� Debug.Log("");
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(3f, 0, false));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 7:
                        // 7 ��ȭ/��� Debug.Log("");
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        JustChatManager.ChangeNpcNumberChatType("0-2");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 8:
                        // 8 �̵�x/���� Debug.Log("");
                        Character.instance.SetCharacterInput(false, false, false);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(Character.instance.transform.position.x, 0, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 9:
                        // 9 ������Ʈ ����/��� Debug.Log("");
                        JustChatManager.CreateNpc(new Vector3(5.5f, 0f, 0f), 0, 8, "���Ʈ�ҳ�");
                        StartCoroutine(JustChatManager.Wait(7));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 10:
                        // 10 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-3");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());


                        StartCoroutine(NextStepDetail());
                        break;
                    case 11:
                        // 11 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-4");
                        JustChatManager.ConversationManager.SetChatName("???");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());


                        StartCoroutine(NextStepDetail());
                        break;
                    case 12:
                        // 12 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-5");
                        //JustChatManager.ConversationManager.CurNpc = JustChatManager.JustChatNpc;
                        JustChatManager.ConversationManager.SetChatName("���Ʈ�ҳ�");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 13:
                        // 13 ���� â Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-6");
                        JustChatManager.ConversationManager.AddSelectEvent(SelectJob);
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 14:
                        // 14 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-7");
                        Character.instance.MyPlayerController.EventConversation.Invoke();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 15:
                        // 15 �ݶ��̴� ����/������Ʈ ����/�� Debug.Log("");
                        JustChatManager.SetCollider();
                        JustChatManager.CreatePortal(new Vector3(6.5f, 0f, 0f), NextMapNumber);
                        Character.instance.SetCharacterInput(true, true, false);

                        _step = 1;
                        _stepDetail = 0;
                        break;
                }
                break;
            case 1:
                // �� ���� �� ���� 5��
                switch (StepDetail)
                {
                    case 0:
                        // 2 �̵�/���� Debug.Log("");
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
                Debug.Log("�������� ����");
                NextMapNumber = "0001";
                break;
            case 1:
                Debug.Log("���� ����");
                NextMapNumber = "0201";
                break;
        }
    }
}
