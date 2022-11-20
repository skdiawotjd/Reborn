using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    enum Direction { Left = 1, Right = -1}

    [SerializeField]
    private SPUM_Prefabs Spum;
    [SerializeField]
    private GameObject RotationObject;
    private bool _characterControllable;
    private bool UIControllable;
    public bool ConversationNext;
    private float moveSpeed;
    private float InputX;
    private float InputY;

    public bool CharacterControllable
    {
        get
        {
            return _characterControllable;
        }
    }

    public UnityEvent EventConversation;
    public UnityEvent<int> EventUIInput;

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

    private void Awake()
    {
        /*Spum = gameObject.GetComponent<SPUM_Prefabs>();
        RotationObject = gameObject.transform.GetChild(0).gameObject;
        BodyCollider = gameObject.GetComponent<Collider2D>();
        InterActionCollider = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Collider2D>();*/

        moveSpeed = 4.0f;
        _characterControllable = false;
        UIControllable = false;
        ConversationNext = false;

        Arrow = new Vector3(1.0f, 1.0f, 1.0f);
        Offset = new Vector2(0.02f, 0.34f);

        LimitPosition = new Vector2(8.9f, 5.4f);
        FinalPosition = new Vector3(0f, 0f, 0f);
    }
    void Start()
    {
        GameManager.instance.DayStart.AddListener(StartPlayerController);
        GameManager.instance.DayEnd.AddListener(EndPlayerController);
        SceneLoadManager.instance.MapSettingEvent.AddListener(SetPlayerPositionRange);
    }

    void Update()
    {
        //Debug.Log("1. CharacterControllable = " + CharacterControllable);
        if (CharacterControllable)
        {
            // ������
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                /*Arrow.x = -1.0f;
                Offset.x = 0.02f;
                if(RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;*/
                
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                /*Arrow.x = 1.0f;
                Offset.x = -0.02f;
                if (RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;*/
            }

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
                    EventUIInput.Invoke(2);
                }
            }
            // �̴ϸ�
            else if (Input.GetKeyDown(KeyCode.M))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(3);
                }
            }
            // ����
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(4);
                }
            }
            // ����Ʈ
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(5);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (CharacterControllable)
        {
            Move();
        }
    }

    private void Move()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");

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
                Arrow.x = -1.0f;
                Offset.x = 0.02f;
                if (RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;
            }
            else if (InputX == -1)
            {
                Arrow.x = 1.0f;
                Offset.x = -0.02f;
                if (RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;
            }
        }
        
        
        FinalPosition.x = Mathf.Clamp(transform.position.x + InputX * moveSpeed * Time.deltaTime, -LimitPosition.x, LimitPosition.x);
        FinalPosition.y = Mathf.Clamp(transform.position.y + InputY * moveSpeed * Time.deltaTime, -LimitPosition.y, LimitPosition.y - 2.7f);

        transform.position = FinalPosition;
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
        UIControllable = UIInput;
    }

    private void EndPlayerController()
    {
        InterActionCollider.enabled = false;
        Spum._anim.SetBool("Run", false);
        Spum._anim.SetFloat("RunState", 0.0f);
        ConversationNext = false;
        _characterControllable = false;
        UIControllable = false;
    }

    private void StartPlayerController()
    {
        _characterControllable = true;
        UIControllable = true;;
    }    
}


/*
 * Animator�� Parameter IsAttack�� AttackState�� �ִ� ���¿����� Attack Ʈ���Ÿ� �����Ű�� �ʱ� ���ؼ� ���
 */