using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed;
    private float health;
    private float maxHealth;
    private int damage;
    public float playerDamage;
    private Rigidbody2D target;
    private SpriteRenderer spriter;
    [SerializeField]
    private SPUM_Prefab Spum;

    private bool isLive;
    private bool isHit = false;
    private bool isRun;
    private bool isAttack;
    private float distance;

    private Vector2 dirVec;
    private Vector2 nextVec;
    private Vector3 direction;

    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        direction = new Vector3(2f, 2f, 1f);

    }
    private void FixedUpdate()
    {
        if (!isLive) // 살아 있는 상태가 아니면 리턴
            return;
        if (Vector2.Distance(target.position, rigid.position) <= distance) // 플레이어와의 거리가 본인 사거리보다 짧거나 같다면
        {
            if(isRun) // 플레이어와의 거리가 본인 사거리보다 짧거나 같으면서 뛰고 있다면 멈춤
            {
                EnemyRunning(false);
                return;
            } else // 플레이어와의 거리가 본인 사거리보다 짧거나 같으면서 멈춘 상태라면
            {
                // 공격
                // EnemyAttackProcess();
                return;
            }
        } else if(isAttack) // 플레이어와의 거리가 본인 사거리보다 길지만, 공격중인 상태라면 리턴
        {
            return;
        }
        // 살아있으면서, 플레이어와의 거리가 본인 사거리보다 길고 공격중인 상태가 아니라면 플레이어를 향해 돌진
        if(!isRun)
        {
            EnemyRunning(true);
        }
        dirVec = target.position - rigid.position;
        nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.velocity = Vector2.zero;

        if (isHit) // 피격 당했다면
        {
            rigid.MovePosition(rigid.position - nextVec);
            return;
        }
        else
        {
            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;

        if(target.position.x < rigid.position.x)
        {
            direction.x = 2f;
            transform.localScale = direction;
        }
        else
        {
            direction.x = -2f;
            transform.localScale = direction;
        }

    }
    private void EnemyRunning(bool state)
    {
        if (state)
        {
            Spum._anim.SetBool("Run", state);
            Spum._anim.SetFloat("RunState", 0.5f);
            isRun = state;
        } else
        {
            Spum._anim.SetBool("Run", state);
            Spum._anim.SetFloat("RunState", 0f);
            isRun = state;
        }
        
        
    }
    private void OnEnable()
    {
        target = Character.instance.GetComponent<Rigidbody2D>();
        isLive = true;
        Spum._anim.SetBool("Run", true);
        Spum._anim.SetFloat("RunState", 0.5f);
        isRun = true;
    }
    public void Init(SpawnData data)
    {
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        distance = data.distance;
        if(data.damage == 0)
        {
            damage = 1;
        } else
        {
            damage = data.damage;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.name)
        {
            case "AttackArea":
                Debug.Log("몬스터 피격, 현재 체력 : " + health + "깎을 체력 : " + playerDamage);

                health -= playerDamage;
                Debug.Log("깎인 체력 : " + health);

                if (health > 0)
                {
                    // 아직 살아있는 상태, 맞는 애니메이션
                    // 피격 시 저지 0.2초
                    isHit = true;
                    Invoke("CanHit", 0.35f);
                }
                else
                {
                    // 사망
                    Dead();
                }
                break;
            case "HitArea":
                AdventureGameManager.instance.battleManager.Damaged(damage);
                Debug.Log("플레이어 피격");
                break;
        }
    }
    private void EnemyAttackProcess()
    {

    }
    private void CanHit()
    {
        isHit = false;
    }
    private void Dead()
    {
        AdventureGameManager.instance.pool.ReturnObject(this);
        AdventureGameManager.instance.MGManager.ChangeEnemyCount(-1);
    }
}
