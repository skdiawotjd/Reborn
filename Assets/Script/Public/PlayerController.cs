using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private SPUM_Prefab Spum;
    [SerializeField]
    private GameObject RotationObject;
    [SerializeField]
    private bool _characterControllable;
    [SerializeField]
    private bool _characterMovable;
    [SerializeField]
    private bool _uIControllable;
    public bool _conversationNext;
    private float moveSpeed;
    private float InputX;
    private float InputY;

    public bool CharacterControllable
    {
        get { return _characterControllable; }
    }
    public bool CharacterMovable
    {
        get { return _characterMovable; }
    }
    public bool UIControllable
    {
        get { return _uIControllable; }
    }
    public bool ConversationNext
    {
        set { _conversationNext = value; }
        get { return _conversationNext; }
    }


    private UnityEvent EventConversation;
    private UnityEvent<KeyDirection> EventSelect;
    private UnityEvent<UIPopUpOrder> EventUIInput;

    [SerializeField]
    private Collider2D BodyCollider;
    [SerializeField]
    private Collider2D InterActionCollider;

    private Coroutine AttackCoroutine;
    private Vector3 Arrow;
    private Vector2 Offset;
    // ���� ��ǥ
    private Vector2 LimitPosition;
    // ���� ��ǥ
    private Vector3 FinalPosition;

    public void AddEventConversation(UnityAction AddEvent)
    {
        EventConversation.AddListener(AddEvent);
    }
    public void InvokeEventConversation()
    {
        EventConversation.Invoke();
    }
    public void AddEventSelect(UnityAction<KeyDirection> AddEvent)
    {
        EventSelect.AddListener(AddEvent);
    }
    public void DeleteEventSelect(UnityAction<KeyDirection> AddEvent)
    {
        EventSelect.RemoveListener(AddEvent);
    }
    public void AddEventUIInput(UnityAction<UIPopUpOrder> AddEvent)
    {
        EventUIInput.AddListener(AddEvent);
    }

    private void Awake()
    {
        moveSpeed = 4.0f;
        _characterControllable = false;
        _characterMovable = false;
        _uIControllable = false;
        ConversationNext = false;

        Arrow = new Vector3(1.0f, 1.0f, 1.0f);
        Offset = new Vector2(0.02f, 0.34f);

        LimitPosition = new Vector2(8.9f, 5.4f);
        FinalPosition = new Vector3(0f, 0f, 0f);

        EventConversation = new UnityEvent();
        EventSelect = new UnityEvent<KeyDirection>();
        EventUIInput = new UnityEvent<UIPopUpOrder>();
    }
    void Start()
    {
        GameManager.instance.AddDayStart(StartPlayerController);
        GameManager.instance.AddDayEnd(EndPlayerController);
        SceneLoadManager.instance.MapSettingEvent.AddListener(SetPlayerPositionRange);
    }

    void Update()
    {
        //Debug.Log("1. CharacterControllable = " + CharacterControllable);
        if (_characterControllable)
        {
            // ����
            //Debug.Log("2. Input.GetKey(KeyCode.X) = " + Input.GetKey(KeyCode.X));
            if (Input.GetKey(KeyCode.X))
            {
                //Debug.Log("3. !Spum._anim.GetBool(IsAttack) && !ConversationNext " + (!Spum._anim.GetBool("IsAttack") && !ConversationNext));
                if (!Spum._anim.GetBool("IsAttack") && !ConversationNext)
                {
                    //Debug.Log("4. InterActionCollider.enabled == true");
                    InterActionCollider.enabled = true;
                    Invoke("DisableCollider", 0.05f);
                }
            }
        }
        else
        {
            if(ConversationNext)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    EventSelect.Invoke(KeyDirection.Up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    EventSelect.Invoke(KeyDirection.Down);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    EventSelect.Invoke(KeyDirection.Left);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    EventSelect.Invoke(KeyDirection.Right);
                }
            }
            // ��� �ѱ��
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (ConversationNext)
                {
                    //Debug.Log("xŰ Ŭ������ ��� �ѱ��");
                    if (EventConversation != null)
                    {
                        EventConversation.Invoke();
                    }
                }
            }
        }

        if (UIControllable)
        {
            // �κ��丮
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(EventUIInput != null)
                {
                    EventUIInput.Invoke(UIPopUpOrder.InvenPanel);
                }
            }
            // ����
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(UIPopUpOrder.SettingPanel);
                }
            }
            // ����Ʈ
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(UIPopUpOrder.QuestPanel);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_characterMovable)
        {
            InputX = Input.GetAxisRaw("Horizontal");
            InputY = Input.GetAxisRaw("Vertical");

            Move();
        }  
    }

    private void Move()
    {
        //Debug.Log("Move");
        if (InputX == 0 && InputY == 0)
        {
            Spum._anim.SetBool("Run", false);
            Spum._anim.SetFloat("RunState", 0.0f);
        }
        else
        {
            Spum._anim.SetBool("Run", true);
            Spum._anim.SetFloat("RunState", 0.5f);

            if (InputX == 1)
            {
                PlayerRotation(Direction.Right);
                Offset.x = 0.02f;
            }
            else if (InputX == -1)
            {
                PlayerRotation(Direction.Left);
                Offset.x = -0.02f;
            }

            if (RotationObject.transform.localScale.x != Arrow.x)
            {
                RotationObject.transform.localScale = Arrow;
            }
            BodyCollider.offset = Offset;
        }
        
        
        FinalPosition.x = Mathf.Clamp(transform.position.x + InputX * moveSpeed * Time.deltaTime, -LimitPosition.x, LimitPosition.x);
        FinalPosition.y = Mathf.Clamp(transform.position.y + InputY * moveSpeed * Time.deltaTime, -LimitPosition.y, LimitPosition.y - 2.7f);

        transform.position = FinalPosition;
    }
    /// <summary>
    /// -1 - ����, 0 - ��, 1 - ��, 2 - ��, 3 - ��
    /// </summary>
    public void SetPlayerPosition(int k)
    {
        switch(k)
        {
            //����
            case -1:
                InputX = 0f;
                InputY = 0f;
                break;
            //��
            case 0:
                InputX = 1f;
                InputY = 0f;
                break;
            //��
            case 1:
                InputX = -1f;
                InputY = 0f;
                break;
            //��
            case 2:
                InputY = 1f;
                break;
            //��
            case 3:
                InputY = -1f;
                break;

        }
        Move();
    }

    public void SetInputX()
    {
        InterActionCollider.enabled = true;
        Invoke("DisableCollider", 0.05f);
    }

    public void PlayerRotation(Direction NewDirection)
    {
        Arrow.x = (float)NewDirection;

        RotationObject.transform.localScale = Arrow;
    }

    private void SetPlayerPositionRange()
    {
        LimitPosition.x = (SceneLoadManager.instance.Background.sizeDelta.x / 2f) - 0.7f;
        LimitPosition.y = (SceneLoadManager.instance.Background.sizeDelta.y / 2f);
    }

    private void AttackProcess()
    {
        //Debug.Log("8. !Spum._anim.GetBool(IsAttack) && Spum._anim.GetCurrentAnimatorStateInfo(0).IsName(RunState) " + !Spum._anim.GetBool("IsAttack") + " " + Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("RunState"));
        if (!Spum._anim.GetBool("IsAttack") && Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("RunState"))
        {
            //Debug.Log("9. AttackCoroutine != null " + (AttackCoroutine != null));
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
            }

            Spum._anim.SetBool("Run", false);
            Spum._anim.SetFloat("RunState", 0f);

            Spum._anim.SetBool("IsAttack", true);
            Spum._anim.SetTrigger("Attack");
            //Debug.Log("10. �ִ� ��Ʈ�ѷ��� IsAttack = " + Spum._anim.GetBool("IsAttack"));
            AttackCoroutine = StartCoroutine(CoroutineAttack());
        }
    }

    IEnumerator CoroutineAttack()
    {
        //Debug.Log("11. CoroutineAttack ����");
        while (Spum._anim.GetBool("IsAttack"))
        {
            //if (Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            if (Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                //Debug.Log("13. 12 ������ �޼��Ǿ 1���������� ��ٷȴٰ�");
                yield return 0.016f;
                Spum._anim.SetBool("IsAttack", false);
                _characterControllable = true;
                //Debug.Log("14. Spum._anim.SetBool(IsAttack, false)�Ͽ� " + Spum._anim.GetBool("IsAttack").ToString() + " , CharacterControllable = " + CharacterControllable);
            }
            else
            {
                //Debug.Log("12. Spum._anim.GetCurrentAnimatorStateInfo(0).IsName(AttackState)�� " + Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") +
                //    "�̰� Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime�� " + Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime + "�̹Ƿ� 0.05�� ���");
                yield return 0.05f;
            }
        }
        //Debug.Log("15. �ڷ�ƾ �Ϸ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void DisableCollider()
    {
        InterActionCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InterActionCollider.enabled = false;
        CancelInvoke("DisableCollider");

        //Debug.Log("5. OnTriggerEnter2D ����, InterActionCollider.enabled = " + InterActionCollider.enabled);

        switch (collision.gameObject.tag)
        {
            case "Attackable":
                //Debug.Log("ĳ���Ͱ� ���ݰ��� �ݶ��̴��� �浹");
                //Debug.Log("6. ĳ���Ϳ� �浹�� �ݶ��̴��� �±װ� Attackable�� ���");
                _characterControllable = false;
                //Debug.Log("7. CharacterControllable = " + CharacterControllable);
                AttackProcess();
                break;
            case "Conversationable":
                //Debug.Log("ĳ���Ͱ� ��ȭ���� �ݶ��̴��� �浹 " + collision.gameObject.name);
                //Debug.Log("��� ���� 2 - �ݸ��� �浹(ĳ����)");
                _characterControllable = false;
                _characterMovable = false;
                if (EventConversation != null)
                {
                    Spum._anim.SetBool("Run", false);
                    Spum._anim.SetFloat("RunState", 0.0f);
                    EventConversation.Invoke();
                }
                break;
        }
    }

    public void SetInput(bool CharacterInput, bool UIInput)
    {
        _characterControllable = CharacterInput;
        _uIControllable = UIInput;
    }
    public void SetInput(bool CharacterInput,bool CharacterMove, bool UIInput)
    {
        _characterControllable = CharacterInput;
        _characterMovable = CharacterMove;
        _uIControllable = UIInput;
    }

    private void EndPlayerController()
    {
        InterActionCollider.enabled = false;
        Spum._anim.SetBool("Run", false);
        Spum._anim.SetFloat("RunState", 0.0f);
        ConversationNext = false;
        _characterControllable = false;
        _characterMovable = false;
        _uIControllable = false;
    }
    private void StartPlayerController()
    {
        _characterControllable = true;
        _characterMovable = true;
        _uIControllable = true;;
    }    
    public void SetRunState(bool state)
    {
        if (state)
        {
            Spum._anim.SetBool("Run", state);
            Spum._anim.SetFloat("RunState", 0.5f);
        }
        else
        {
            Spum._anim.SetBool("Run", state);
            Spum._anim.SetFloat("RunState", 0.0f);
        }
    }
    public void PlayAttackProcess()
    {
        Spum._anim.SetTrigger("Attack");
    }
    public float GetPlayerDirection()
    {
        return RotationObject.transform.localScale.x;
    }
    public bool CanAttack()
    {
        return Spum._anim.GetBool("IsAttack");
    }

    public void StartDie()
    {
        Spum._anim.SetTrigger("Die");
    }
    IEnumerator EndDieCourtine()
    {
        Spum._anim.SetBool("EditChk", true);

        while (!Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("RunState"))
        {
            yield return new WaitForEndOfFrame();
        }

        Spum._anim.SetBool("EditChk", false);
    }
    public void EndDie()
    {
        StartCoroutine(EndDieCourtine());
    }
    public void PlayDieProcess(bool state)
    {
        if (state)
        {
            Debug.Log("������");
            StartDie();
        }
        else
        {
            Debug.Log("�Ͼ�� ���");
            EndDie();
        }

    }
}


/*
 * Animator�� Parameter IsAttack�� AttackState�� �ִ� ���¿����� Attack Ʈ���Ÿ� �����Ű�� �ʱ� ���ؼ� ���
 */