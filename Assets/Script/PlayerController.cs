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

    private bool TemBool = true;

    void Start()
    {
        Self = gameObject.GetComponent<SPUM_Prefabs>();
        RotationObject = gameObject.transform.GetChild(0).gameObject;

        moveSpeed = 5.0f;

        CharacterControllable = true;
        UIControllable = true;


        //StartCoroutine("Tem");
    }

    void Update()
    {
        if (CharacterControllable)
        {
            // 움직임
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotationObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotationObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            Move();

            // 공격
            if (Input.GetKey(KeyCode.X))
            {
                //Self._anim.Play("2_Attack_Normal_0", 0);
                if (Self._anim.GetCurrentAnimatorStateInfo(0).IsName("RunState"))
                {
                    if (!Self._anim.GetBool("IsAttack"))
                    {
                        Self._anim.SetBool("IsAttack", true);
                        Self._anim.SetTrigger("Attack");
                        StartCoroutine(CoroutineAttack());
                    }
                }
                //Self._anim.SetBool("AttackEnd", true);
                //TemBool = !TemBool;
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

    IEnumerator CoroutineAttack()
    {
        while (Self._anim.GetBool("IsAttack"))
        {
            if (Self._anim.GetCurrentAnimatorStateInfo(0).IsName("AttackState"))
            {
                yield return Self._anim.GetCurrentAnimatorStateInfo(0).length;

                Self._anim.SetBool("IsAttack", false);
            }
            else
            {
                yield return 0.01f;
            }
        }
    }
    
/*    private void SetAnimation(int Type)
    {
        switch(Type)
        {
            case 0:
                //Self.PlayAnimation("0_idle");
                
                break;
            case 1:
                //Self.PlayAnimation("1_Run");
                break;
            case 2:
                //Self._anim.Play("2_Attack_Normal_0", 0);
                //Self.PlayAnimation("2_Attack_Normal_0");
                
                break;
        }
    }

    IEnumerator Tem()
    {
        while(TemBool)
        {
            yield return new WaitForSeconds(2.0f);
            SetAnimation(2);
            Self._anim.SetBool("AttackEnd", true);
        }
        
    }*/
}


/*
 * Animator의 Parameter IsAttack은 AttackState에 있는 상태에서는 Attack 트리거를 실행시키기 않기 위해서 사용
 */