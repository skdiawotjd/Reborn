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
    public bool CharacterMovable
    {
        get
        {
            return _characterMovable;
        }
    }

    public UnityEvent EventConversation;
    public UnityEvent<UIPopUpOrder> EventUIInput;

    [SerializeField]
    private Collider2D BodyCollider;
    [SerializeField]
    private Collider2D InterActionCollider;

    private Coroutine AttackCoroutine;
    private Vector3 Arrow;
    private Vector2 Offset;
    // 제한 좌표
    private Vector2 LimitPosition;
    // 최종 좌표
    private Vector3 FinalPosition;

    private void Awake()
    {
        /*Spum = gameObject.GetComponent<SPUM_Prefabs>();
        RotationObject = gameObject.transform.GetChild(0).gameObject;
        BodyCollider = gameObject.GetComponent<Collider2D>();
        InterActionCollider = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Collider2D>();*/

        moveSpeed = 4.0f;
        _characterControllable = false;
        _characterMovable = false;
        UIControllable = false;
        ConversationNext = false;

        Arrow = new Vector3(1.0f, 1.0f, 1.0f);
        Offset = new Vector2(0.02f, 0.34f);

        LimitPosition = new Vector2(8.9f, 5.4f);
        FinalPosition = new Vector3(0f, 0f, 0f);
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
        if (CharacterControllable)
        {
            // 공격
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
            // 대사 넘기기
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (ConversationNext)
                {
                    //Debug.Log("x키 클릭으로 대사 넘기기");
                    if (EventConversation != null)
                    {
                        EventConversation.Invoke();
                    }
                }
            }
        }

        if (UIControllable)
        {
            // 인벤토리
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(EventUIInput != null)
                {
                    EventUIInput.Invoke(UIPopUpOrder.InvenPanel);
                }
            }
            // 설정
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(UIPopUpOrder.SettingPanel);
                }
            }
            // 퀘스트
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
                if (RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;
            }
            else if (InputX == -1)
            {
                PlayerRotation(Direction.Left);
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
    /// <summary>
    /// -1 - 정지, 0 - 좌, 1 - 우, 2 - 상, 3 - 하
    /// </summary>
    public void SetPlayerPosition(int k)
    {
        switch(k)
        {
            //정지
            case -1:
                InputX = 0f;
                InputY = 0f;
                break;
            //좌
            case 0:
                InputX = 1f;
                InputY = 0f;
                break;
            //우
            case 1:
                InputX = -1f;
                InputY = 0f;
                break;
            //상
            case 2:
                InputY = 1f;
                break;
            //하
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
            //Debug.Log("10. 애님 컨트롤러의 IsAttack = " + Spum._anim.GetBool("IsAttack"));
            AttackCoroutine = StartCoroutine(CoroutineAttack());
        }
    }

    IEnumerator CoroutineAttack()
    {
        //Debug.Log("11. CoroutineAttack 시작");
        while (Spum._anim.GetBool("IsAttack"))
        {
            //if (Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            if (Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                //Debug.Log("13. 12 조건이 달성되어서 1프레임정도 기다렸다가");
                yield return 0.016f;
                Spum._anim.SetBool("IsAttack", false);
                _characterControllable = true;
                //Debug.Log("14. Spum._anim.SetBool(IsAttack, false)하여 " + Spum._anim.GetBool("IsAttack").ToString() + " , CharacterControllable = " + CharacterControllable);
            }
            else
            {
                //Debug.Log("12. Spum._anim.GetCurrentAnimatorStateInfo(0).IsName(AttackState)이 " + Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") +
                //    "이고 Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime가 " + Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime + "이므로 0.05초 대기");
                yield return 0.05f;
            }
        }
        //Debug.Log("15. 코루틴 완료!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void DisableCollider()
    {
        InterActionCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InterActionCollider.enabled = false;
        CancelInvoke("DisableCollider");

        //Debug.Log("5. OnTriggerEnter2D 진입, InterActionCollider.enabled = " + InterActionCollider.enabled);

        switch (collision.gameObject.tag)
        {
            case "Attackable":
                //Debug.Log("캐릭터가 공격가능 콜라이더랑 충돌");
                //Debug.Log("6. 캐릭터와 충돌한 콜라이더의 태그가 Attackable인 경우");
                _characterControllable = false;
                //Debug.Log("7. CharacterControllable = " + CharacterControllable);
                AttackProcess();
                break;
            case "Conversationable":
                //Debug.Log("캐릭터가 대화가능 콜라이더랑 충돌 " + collision.gameObject.name);
                //Debug.Log("대사 시작 2 - 콜리전 충돌(캐릭터)");
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
        UIControllable = UIInput;

        //Debug.Log(_characterControllable + " " + UIControllable);
    }
    public void SetInput(bool CharacterInput,bool CharacterMove, bool UIInput)
    {
        _characterControllable = CharacterInput;
        _characterMovable = CharacterMove;
        UIControllable = UIInput;
    }

    private void EndPlayerController()
    {
        InterActionCollider.enabled = false;
        Spum._anim.SetBool("Run", false);
        Spum._anim.SetFloat("RunState", 0.0f);
        ConversationNext = false;
        _characterControllable = false;
        _characterMovable = false;
        UIControllable = false;
    }

    private void StartPlayerController()
    {
        _characterControllable = true;
        _characterMovable = true;
        UIControllable = true;;
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
}


/*
 * Animator의 Parameter IsAttack은 AttackState에 있는 상태에서는 Attack 트리거를 실행시키기 않기 위해서 사용
 */