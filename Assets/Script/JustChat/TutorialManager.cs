using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private int _step;
    [SerializeField]
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
    private GameObject _objjustChatManager;
    private JustChatManager _justChatManager;

    public GameObject ObjjustChatManager
    {
        set
        {
            _objjustChatManager = Instantiate(value, Vector3.zero, Quaternion.identity) as GameObject;
            _objjustChatManager.name = "JustChatManager";
            _justChatManager = _objjustChatManager.GetComponent<JustChatManager>();
        }
    }

    public JustChatManager JustChatManager
    {
        get { return _justChatManager; }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        _step = 3;
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
            //JustChatManager = GameObject.Find("JustChatManager").GetComponent<JustChatManager>();
            //_step = 0;
            //_stepDetail = 0;
            ///
            //GameManager.instance.RemoveSceneMoveEvent(SetTutorial);
            //GameManager.instance.AddSceneMoveEvent(CheckStep);
            CheckStep();
        }
        else
        {
            GameManager.instance.RemoveSceneMoveEvent(SetTutorial);
            Destroy(this.gameObject);
        }
    }

    private void CheckStep()
    {
        switch (Step)
        {
            case 0:
                // ��ê���� ���
                switch (StepDetail)
                {
                    case 0:
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        //_justChatManager = GameObject.Find("JustChatManager").GetComponent<JustChatManager>();
                        StepDetail++;
                        break;
                    case 1:
                        // 1 ������Ʈ ����/�̵�/���� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-4f, 0.5f, 0f), Quaternion.identity);
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(-7f, 0.5f, 0f), Quaternion.identity);
                        if(JustChatManager)
                        {
                            StartCoroutine(JustChatManager.MoveCharacterToInterActive(-3f, 1, true));

                            StartCoroutine(NextStepDetail());
                        }
                        else
                        {
                            StartCoroutine(WaitSceneMoveEvent());
                        }
                        break;
                    case 2:
                        // 2 �̵�/���� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(-6f, 1, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 3:
                        // 3 ��ȭ/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        JustChatManager.ChangeNpcNumberChatType("0-8");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 4:
                        // 4 ������Ʈ ����/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        Instantiate(Resources.Load("Prefabs/Trash"), new Vector3(4.5f, 0.5f, 0f), Quaternion.identity);
                        StartCoroutine(JustChatManager.Wait(7));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 5:
                        // 5 ĳ���� ���� ���� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        Character.instance.MyPlayerController.SetPlayerPosition(0);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        // 5 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("0-7");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 6:
                        // 6 �̵�/���� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(3f, 0, false));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 7:
                        // 7 ��ȭ/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        Character.instance.MyPlayerController.SetPlayerPosition(-1);
                        JustChatManager.ChangeNpcNumberChatType("0-6");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 8:
                        // 8 �̵�x/���� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        Character.instance.SetCharacterInput(false, false, false);
                        StartCoroutine(JustChatManager.MoveCharacterToInterActive(Character.instance.transform.position.x, 0, true));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 9:
                        // 9 ������Ʈ ����/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        JustChatManager.CreateNpc(new Vector3(5.5f, 0f, 0f), 0, 0, 2);
                        StartCoroutine(JustChatManager.Wait(7));

                        StartCoroutine(NextStepDetail());
                        break;
                    case 10:
                        // 10 ��ȭ/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        JustChatManager.ChangeNpcNumberChatType("0-5");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());


                        StartCoroutine(NextStepDetail());
                        break;
                    case 11:
                        // 11 ��ȭ/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        JustChatManager.ChangeNpcNumberChatType("0-4");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());


                        StartCoroutine(NextStepDetail());
                        break;
                    case 12:
                        // 12 ��ȭ/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        JustChatManager.ChangeNpcNumberChatType("0-3");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 13:
                        // 13 ���� â Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        JustChatManager.ChangeNpcNumberChatType("0-2");
                        JustChatManager.ConversationManager.AddSelectEvent(SelectJob);
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 14:
                        // 14 ��ȭ/��� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
                        JustChatManager.ChangeNpcNumberChatType("0-1");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 15:
                        // 15 �ݶ��̴� ����/������Ʈ ����/�� Debug.Log("");
                        //Debug.Log("CheckStep - " + Step + "/" + StepDetail);
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
                        // 0 ��ȭ/��� Debug.Log("");
                        Character.instance.SetCharacterInput(false, false, false);
                        JustChatManager.ChangeNpcNumberChatType("1-5");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 1:
                        // 1 ��ȭ/��� Debug.Log("");
                        JustChatManager.ChangeNpcNumberChatType("1-4");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 2:
                        // 2 �뿹 ��������Ʈ �ο� Debug.Log("");
                        /*Character.instance.SetCharacterStat(CharacterStatType.MyItem, "99990");
                        QuestManager.instance.AddQuest("9999");*/

                        NextMapNumber = "0008";
                        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, NextMapNumber);

                        _stepDetail++;
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        break;
                    case 3:
                        _stepDetail++;
                        break;
                    case 4:
                        // 4 ��������Ʈ�� �´� ������ �̵� Debug.Log("");
                        NextMapNumber = "0001";
                        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, NextMapNumber);

                        _stepDetail++;
                        SceneManager.LoadScene("MiniGame");
                        break;
                    case 5:
                         Debug.Log(" 5 ��������Ʈ �Ϸ�, ��ȭ/��� ");
                        //Character.instance.SetCharacterStat(CharacterStatType.MyItem, "99991");
                        Character.instance.SetCharacterStat(CharacterStatType.MyItem, "100010");

                        Character.instance.SetCharacterInput(false, false, false);
                        JustChatManager.ChangeNpcNumberChatType("1-3");
                        Character.instance.MyPlayerController.InvokeEventConversation();
                        StartCoroutine(JustChatManager.WaitChat());

                        StartCoroutine(NextStepDetail());
                        break;
                    case 6:
                          Debug.Log("6 �÷��̾ ���� ���� ������ �̵�");
                        Character.instance.SetCharacterInput(true, true, false);

                        _stepDetail++;
                        break;
                    case 7:
                          Debug.Log("7 ������ ������ �̵��ߴ��� Ȯ��");
                        NextMapNumber = "0003";

                        if (NextMapNumber == Character.instance.MyMapNumber)
                        {
                            _stepDetail++;
                            StepDetail++;
                        }
                        else
                        {
                            Character.instance.SetCharacterInput(false, false, false);
                            JustChatManager.ChangeNpcNumberChatType("1-2");
                            Character.instance.MyPlayerController.InvokeEventConversation();
                            StartCoroutine(JustChatManager.WaitChat());
                            StartCoroutine(NextStepDetail());
                        }
                        break;
                    case 8:
                          Debug.Log("8 �߸� �̵��� ��� �ٽ� ���� ������ �̵�");
                        NextMapNumber = "0001";
                        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, NextMapNumber);

                        _stepDetail = 6;
                        SceneManager.LoadScene("MiniGame");
                        break;
                    case 9:
                         Debug.Log("9 ������ �� �Ϸ�, ��ȭ/���");
                        if (Character.instance.MyItemManager.IsExistItem("1600"))
                        {
                            Character.instance.SetCharacterStat(CharacterStatType.MyItem, "16009");
                            Character.instance.SetCharacterStat(CharacterStatType.MyItem, "13004");
                            Character.instance.SetCharacterStat(CharacterStatType.MyItem, "30001");

                            Character.instance.SetCharacterInput(false, false, false);
                            JustChatManager.ChangeNpcNumberChatType("1-1");
                            Character.instance.MyPlayerController.InvokeEventConversation();
                            StartCoroutine(JustChatManager.WaitChat());
                            StartCoroutine(NextStepDetail());
                        }
                        break;
                    case 10:
                         Debug.Log("10 �÷��̾ ���� ���� ������ �̵�");
                        Character.instance.SetCharacterInput(true, true, false);

                        _stepDetail++;
                        break;
                    case 11:
                         Debug.Log("11 ������ ������ �̵��ߴ��� Ȯ��");
                        NextMapNumber = "0004";

                        if (NextMapNumber == Character.instance.MyMapNumber)
                        {
                            _stepDetail++;
                            StepDetail++;
                        }
                        else
                        {
                            Character.instance.SetCharacterInput(false, false, false);
                            JustChatManager.ChangeNpcNumberChatType("1-2");
                            Character.instance.MyPlayerController.InvokeEventConversation();
                            StartCoroutine(JustChatManager.WaitChat());
                            StartCoroutine(NextStepDetail());
                        }
                        break;
                    case 12:
                         Debug.Log("12 �߸� �̵��� ��� �ٽ� ���� ������ �̵�");
                        NextMapNumber = "0001";
                        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, NextMapNumber);

                        _stepDetail = 10;
                        SceneManager.LoadScene("MiniGame");
                        break;
                    case 13:
                         Debug.Log("13 Ư�� �������� �ִٸ� ���/���");
                        if (Character.instance.MyItemManager.IsExistItem("2000"))
                        {
                            Character.instance.SetCharacterInput(false, false, false);
                            JustChatManager.ChangeNpcNumberChatType("1-0");
                            Character.instance.MyPlayerController.InvokeEventConversation();
                            StartCoroutine(JustChatManager.WaitChat());
                            StartCoroutine(NextStepDetail());
                        }
                        break;
                    case 14:
                         Debug.Log("14 �Ϸ�");
                        Character.instance.SetCharacterStat(CharacterStatType.Reputation, 100);
                        Character.instance.SetCharacterStat(CharacterStatType.Smith, 50);
                        GameManager.instance.IsDayStart = true;
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -Character.instance.ActivePoint);
                        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -Character.instance.ActivePoint);
                        Debug.Log("asdasdasdsad " + Character.instance.ActivePoint);
                        _stepDetail++;
                        break;
                    case 15:
                        Destroy(gameObject);
                        break;
                }
                break;
        }
    }

    IEnumerator NextStepDetail()
    {
        //Debug.Log("NextStepDetail 1 - " + StepDetail);
        while (JustChatManager.IsWorking)
        {
            //Debug.Log("NextStepDetail 2 - " + StepDetail);
            yield return new WaitForSeconds(0.05f);
        }
        //Debug.Log("NextStepDetail 3 - " + (_stepDetail + 1));
        StepDetail++;
    }

    private void SelectJob(int Job)
    {
        switch(Job)
        {
            case 0:
                //Debug.Log("�������� ����");
                NextMapNumber = "0001";
                break;
            case 1:
                //Debug.Log("���� ����");
                NextMapNumber = "0201";
                break;
        }
    }

    IEnumerator WaitSceneMoveEvent()
    {
        //Debug.Log("WaitSceneMoveEvent 1");
        while (!JustChatManager)
        {
            //Debug.Log("WaitSceneMoveEvent 2");
            yield return new WaitForSeconds(0.016f);
        }
        //Debug.Log("WaitSceneMoveEvent 3");
        CheckStep();
    }
}
