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

    public Collider2D InterActiveCollider;
    private bool IsInterActing;

    Coroutine AttackCorutine;
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

        moveSpeed = 5.0f;

        CharacterControllable = true;
        UIControllable = true;
        IsInterActing = false;
        Arrow = new Vector3(1.0f, 1.0f, 1.0f);
        Offset = new Vector2(0.08f, 0.34f);
    }

    void Update()
    {
        if (CharacterControllable)
        {
            // ������
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Arrow.x = -1.0f;
                Offset.x = 0.08f;
                RotationObject.transform.localScale = Arrow;
                InterActiveCollider.offset = Offset;
                
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Arrow.x = 1.0f;
                Offset.x = -0.08f;
                RotationObject.transform.localScale = Arrow;
                InterActiveCollider.offset = Offset;
            }
            Move();

            // ����
            if (Input.GetKey(KeyCode.X))
            {
                if (IsInterActing == false)
                {
                    IsInterActing = true;
                    InterActiveCollider.enabled = true;
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
            if (AttackCorutine != null)
            {
                StopCoroutine(AttackCorutine);
            }

            Self._anim.SetBool("IsAttack", true);
            Self._anim.SetTrigger("Attack");
            AttackCorutine = StartCoroutine(CoroutineAttack());
        }
    }

    IEnumerator CoroutineAttack()
    {
        while (Self._anim.GetBool("IsAttack"))
        {
            if (Self._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState") && Self._anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                Self._anim.SetBool("IsAttack", false);
                IsInterActing = false;
            }
            else
            {
                yield return 0.1f;
            }
        }
    }

    private void DisableCollider()
    {
        InterActiveCollider.enabled = false;
        IsInterActing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CancelInvoke("DisableCollider");
        IsInterActing = true;
        InterActiveCollider.enabled = false;


        switch (collision.gameObject.tag)
        {
            case "Attackable":
                Debug.Log("ĳ���Ͱ� ���ݰ��� �ݶ��̴��� �浹");
                AttackProcess();
                break;
            case "Conversationable":
                Debug.Log("ĳ���Ͱ� ��ȭ���� �ݶ��̴��� �浹");
                break;
        }
    }
}


/*
 * Animator�� Parameter IsAttack�� AttackState�� �ִ� ���¿����� Attack Ʈ���Ÿ� �����Ű�� �ʱ� ���ؼ� ���
 */