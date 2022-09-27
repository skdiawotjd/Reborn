using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    enum Direction { Left = 1, Right = -1}

    private SPUM_Prefabs Spum;
    private GameObject RotationObject;
    private bool CharacterControllable;
    private bool UIControllable;
    public bool ConversationNext;
    private float moveSpeed;
    private float InputX;
    private float InputY;

    public UnityEvent EventConversation;
    public UnityEvent<int> EventUIInput;

    private Collider2D BodyCollider;
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
        Spum = gameObject.GetComponent<SPUM_Prefabs>();
        RotationObject = gameObject.transform.GetChild(0).gameObject;
        BodyCollider = gameObject.GetComponent<Collider2D>();
        InterActionCollider = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Collider2D>();

        moveSpeed = 4.0f;
        CharacterControllable = false;
        UIControllable = false;
        ConversationNext = false;

        Arrow = new Vector3(1.0f, 1.0f, 1.0f);
        Offset = new Vector2(0.02f, 0.34f);

        LimitPosition = new Vector2(8.9f, 5.4f);
        FinalPosition = new Vector3(0f, 0f, 0f);
    }
    void Start()
    {
        GameManager.instance.GameEnd.AddListener(EndPlayerController);
        GameManager.instance.SceneMove.AddListener(SetPlayerPositionRange);

        SetPlayerPositionRange();
    }

    void Update()
    {
        if (CharacterControllable)
        {
            // 움직임
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Arrow.x = -1.0f;
                Offset.x = 0.02f;
                if(RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;
                
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Arrow.x = 1.0f;
                Offset.x = -0.02f;
                if (RotationObject.transform.localScale.x != Arrow.x)
                {
                    RotationObject.transform.localScale = Arrow;
                }
                BodyCollider.offset = Offset;
            }

            // 공격
            if (Input.GetKey(KeyCode.X))
            {
                if (!Spum._anim.GetBool("IsAttack") && !ConversationNext)
                {
                    //BodyCollider.enabled = false;
                    InterActionCollider.enabled = true;
                    Invoke("DisableCollider", 0.1f);
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
                    Debug.Log("x키 클릭으로 대사 넘기기");
                    if (EventConversation != null)
                    {
                        ConversationNext = false;
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
                    EventUIInput.Invoke(2);
                }
            }
            // 미니맵
            else if (Input.GetKeyDown(KeyCode.M))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(3);
                }
            }
            // 설정
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EventUIInput != null)
                {
                    EventUIInput.Invoke(4);
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
        }
        
        
        FinalPosition.x = Mathf.Clamp(transform.position.x + InputX * moveSpeed * Time.deltaTime, -LimitPosition.x, LimitPosition.x);
        FinalPosition.y = Mathf.Clamp(transform.position.y + InputY * moveSpeed * Time.deltaTime, -LimitPosition.y, LimitPosition.y - 2.7f);

        transform.position = FinalPosition;
    }

    private void SetPlayerPositionRange()
    {
        LimitPosition.x = (GameManager.instance.Background.sizeDelta.x / 2f) - 0.7f;
        LimitPosition.y = (GameManager.instance.Background.sizeDelta.y / 2f);
    }

    private void AttackProcess()
    {
        if (!Spum._anim.GetBool("IsAttack") && Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("RunState"))
        {
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
            }

            Spum._anim.SetBool("IsAttack", true);
            Spum._anim.SetTrigger("Attack");
            AttackCoroutine = StartCoroutine(CoroutineAttack());
        }
    }

    IEnumerator CoroutineAttack()
    {
        while (Spum._anim.GetBool("IsAttack"))
        {
            if (Spum._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Spum._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                yield return 0.016f;
                Spum._anim.SetBool("IsAttack", false);
                CharacterControllable = true;
            }
            else
            {
                yield return 0.1f;
            }
        }
    }

    private void DisableCollider()
    {
        InterActionCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InterActionCollider.enabled = false;
        CancelInvoke("DisableCollider");

        switch (collision.gameObject.tag)
        {
            case "Attackable":
                Debug.Log("캐릭터가 공격가능 콜라이더랑 충돌");
                CharacterControllable = false;
                AttackProcess();
                break;
            case "Conversationable":
                Debug.Log("캐릭터가 대화가능 콜라이더랑 충돌");
                Debug.Log("대사 시작 1 - 콜리전 충돌(캐릭터)");
                CharacterControllable = false;
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
        CharacterControllable = CharacterInput;
        UIControllable = UIInput;
    }

    private void EndPlayerController()
    {
        Spum._anim.SetBool("Run", false);
        Spum._anim.SetFloat("RunState", 0.0f);
    }
}


/*
 * Animator의 Parameter IsAttack은 AttackState에 있는 상태에서는 Attack 트리거를 실행시키기 않기 위해서 사용
 */