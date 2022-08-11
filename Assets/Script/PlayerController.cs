using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private SPUM_Prefabs Self;
    private GameObject RotationObject;
    private bool CharacterControllable;
    private bool UIControllable;
    private float moveSpeed;
    private float InputX;
    private float InputY;

    public UnityEvent EventInven;

    private Collider2D BodyCollider;
    private Collider2D InterActionCollider;

    private Coroutine AttackCoroutine;
    private Vector3 Arrow;
    private Vector2 Offset;

    /*private bool _MoveAni;

    public bool MoveAni
    {
        get 
        { 
            return _MoveAni;
        }
        set 
        {
            _MoveAni = value;
            if (_MoveAni)
            {
                SetAnimation(1);
            }
            else
            {
                SetAnimation(0);
            }
        }
    }*/

    void Start()
    {
        Self = gameObject.GetComponent<SPUM_Prefabs>();
        RotationObject = gameObject.transform.GetChild(0).gameObject;
        BodyCollider = gameObject.GetComponent<Collider2D>();
        InterActionCollider = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Collider2D>();

        moveSpeed = 5.0f;

        CharacterControllable = true;
        UIControllable = true;
        Arrow = new Vector3(1.0f, 1.0f, 1.0f);
        Offset = new Vector2(0.02f, 0.34f);

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
                RotationObject.transform.localScale = Arrow;
                BodyCollider.offset = Offset;
                
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Arrow.x = 1.0f;
                Offset.x = -0.02f;
                RotationObject.transform.localScale = Arrow;
                BodyCollider.offset = Offset;
            }

            // 공격
            if (Input.GetKey(KeyCode.X))
            {
                if (!Self._anim.GetBool("IsAttack"))
                {
                    //BodyCollider.enabled = false;
                    InterActionCollider.enabled = true;
                    Invoke("DisableCollider", 0.1f);
                }
            }
        }

        if (UIControllable)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(EventInven != null)
                {
                    EventInven.Invoke();
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
            Self._anim.SetBool("Run", false);
            Self._anim.SetFloat("RunState", 0.0f);
        }
        else
        {
            Self._anim.SetBool("Run", true);
            Self._anim.SetFloat("RunState", 0.5f);
        }
        Vector3 moveVelocity = new Vector3(InputX, InputY, 0) * moveSpeed * Time.deltaTime;
        transform.position += moveVelocity;
    }

    private void AttackProcess()
    {
        if (!Self._anim.GetBool("IsAttack") && Self._anim.GetCurrentAnimatorStateInfo(0).IsName("RunState"))
        {
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
            }

            Self._anim.SetBool("IsAttack", true);
            Self._anim.SetTrigger("Attack");
            AttackCoroutine = StartCoroutine(CoroutineAttack());
        }
    }

    IEnumerator CoroutineAttack()
    {
        while (Self._anim.GetBool("IsAttack"))
        {
            if (Self._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Self._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                Self._anim.SetBool("IsAttack", false);
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
        //BodyCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CancelInvoke("DisableCollider");
        InterActionCollider.enabled = false;
        //BodyCollider.enabled = true;

        switch (collision.gameObject.tag)
        {
            case "Attackable":
                Debug.Log("캐릭터가 공격가능 콜라이더랑 충돌");
                AttackProcess();
                break;
            case "Conversationable":
                Debug.Log("캐릭터가 대화가능 콜라이더랑 충돌");
                break;
        }
    }

    public void SetInput(bool CharacterInput, bool UIInput)
    {
        CharacterControllable = CharacterInput;
        UIControllable = UIInput;
}
}


/*
 * Animator의 Parameter IsAttack은 AttackState에 있는 상태에서는 Attack 트리거를 실행시키기 않기 위해서 사용
 */