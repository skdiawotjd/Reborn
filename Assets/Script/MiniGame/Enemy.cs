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
        if (!isLive)
            return;            
        dirVec = target.position - rigid.position;
        nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        
        rigid.velocity = Vector2.zero;
        if (isHit)
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

    private void OnEnable()
    {
        target = Character.instance.GetComponent<Rigidbody2D>();
        isLive = true;
        Spum._anim.SetBool("Run", true);
        Spum._anim.SetFloat("RunState", 0.5f);
    }
    public void Init(SpawnData data)
    {
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
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
