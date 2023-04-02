using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Enemy : MonoBehaviour, IPoolObject
{
    private float speed;
    private float health;
    private float maxHealth;
    private int damage;
    public float playerDamage;
    private Rigidbody2D target;
    [SerializeField]
    private SPUM_Prefab Spum;
    public string idName;
    private EnemyAttack projectile;

    private bool isLive;
    private bool isHit = false;
    private bool isRun;
    private bool isAttack;
    private float distance;
    private float attackTimeCount;

    private Vector2 dirVec;
    private Vector2 nextVec;
    private Vector3 direction;

    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        direction = new Vector3(2f, 2f, 1f);

    }
    private void FixedUpdate()
    {
        if (!isLive) // ��� �ִ� ���°� �ƴϸ� ����
            return;
        if (Vector2.Distance(target.position, rigid.position) <= distance) // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ª�ų� ���ٸ�
        {
            if(isRun) // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ª�ų� �����鼭 �ٰ� �ִٸ� ����
            {
                EnemyRunning(false);
                return;
            } else if(!isAttack) // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ª�ų� �����鼭 ���� �ְ� �������� �ƴ� ���¶��
            {
                // ����
                EnemyAttackProcess();
                isAttack = true;
                StartCoroutine("AttackTimeCoroutine", 3f);
                return;
            } else // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ª�ų� �����鼭 �����ְ� �������̶��
            {
                rigid.velocity = Vector2.zero;
                return;
            }
        } else if(isAttack) // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ������, �������� ���¶�� ����
        {
            rigid.velocity = Vector2.zero;
            return;
        }
        // ��������鼭, �÷��̾���� �Ÿ��� ���� ��Ÿ����� ��� �������� ���°� �ƴ϶�� �÷��̾ ���� ����
        if(!isRun)
        {
            EnemyRunning(true);
        }
        dirVec = target.position - rigid.position;
        nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.velocity = Vector2.zero;

        if (isHit) // �ǰ� ���ߴٸ�
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "HitArea":
                if(attackTimeCount > 1f)
                {
                    attackTimeCount = 0;
                    AdventureGameManager.instance.battleManager.Damaged(damage);
                    Debug.Log("�÷��̾� �ǰ�");
                } else
                {
                    attackTimeCount += Time.deltaTime * 2;
                    
                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.name)
        {
            case "AttackArea":
                Debug.Log("���� �ǰ�, ���� ü�� : " + health + "���� ü�� : " + playerDamage);

                health -= playerDamage;
                Debug.Log("���� ü�� : " + health);

                if (health > 0)
                {
                    // ���� ����ִ� ����, �´� �ִϸ��̼�
                    // �ǰ� �� ���� 0.2��
                    isHit = true;
                    Invoke("CanHit", 0.35f);
                }
                else
                {
                    // ���
                    Dead();
                }
                break;
            case "boomerang":
                health -= playerDamage;
                if (health > 0)
                {
                    // ���� ����ִ� ����, �´� �ִϸ��̼�
                    // �ǰ� �� ���� 0.2��
                    isHit = true;
                    Invoke("CanHit", 0.35f);
                }
                else if(isLive)
                {
                    // ���
                    isLive = false;
                    Dead();
                }
                break;
            case "HitArea":
                AdventureGameManager.instance.battleManager.Damaged(damage);
                Debug.Log("�÷��̾� �ǰ�");
                break;
        }
    }
    private void EnemyAttackProcess()
    {
        //projectile = AdventureGameManager.instance.pool.GetFromPool<EnemyAttack>(2);
        //projectile.SetDamage(damage);
        //projectile.SetStartPosition(rigid.transform);W
        AdventureGameManager.instance.MGManager.GenerateEnemyAttack(damage, rigid.transform);
    }
    private void CanHit()
    {
        isHit = false;
    }
    private void Dead()
    {
        Debug.Log("���� ���");
        AdventureGameManager.instance.pool.TakeToPool<Enemy>(this.idName, this);
        transform.SetParent(AdventureGameManager.instance.pool.transform);
        AdventureGameManager.instance.MGManager.ChangeEnemyCount(-1);
    }
    public void GameStop()
    {
        isLive = false;
        Spum._anim.SetBool("Run", false);
        Spum._anim.SetFloat("RunState", 0f);
    }
    public void OnCreatedInPool()
    {
        playerDamage = Character.instance.Reputation;
        if (playerDamage < 10)
            playerDamage = 10;
    }

    public void OnGettingFromPool()
    {
        target = Character.instance.GetComponent<Rigidbody2D>();
        isLive = true;
        Spum._anim.SetBool("Run", true);
        Spum._anim.SetFloat("RunState", 0.5f);
        isRun = true;
        isAttack = false;
    }
    IEnumerator AttackTimeCoroutine(float delayTime)
    {
        yield return YieldCache.WaitForSeconds(delayTime);
        isAttack = false;
    }
}
