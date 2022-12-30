using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private SPUM_Prefab Spum;
    public Projectile projectile;
    private Projectile temProjectile;
    public delegate void MonsterDamage(int target);
    public MonsterDamage monsterDamage;
    private int monsterHp;
    private int monsterAtk;
    private bool battle;
    private float curtime;
    private float cooltime;
    public UnityAction<int> MonsterProjectileAttackEvent2;
    // Start is called before the first frame update
    void Start()
    {
        battle = false;

        monsterAtk = 10;
        monsterHp = 100;
        cooltime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (battle)
        {
            if (curtime <= 0)
            {
                ProjectileFire();
                curtime = cooltime;
            }
            curtime -= Time.deltaTime;
            if (monsterHp <= 0)
            {
                battle = false;
                Spum._anim.SetTrigger("Die");
                // �״� �ִϸ��̼�, 1�� ��� �ڷ�ƾ �߰��� ��
                Invoke("DestroyMonster", 3f);
                //DestroyMonster();
            }
        }

    }
    public void ProjectileFire()
    {
        temProjectile = Instantiate(projectile, transform.position, transform.rotation) as Projectile;
        temProjectile.damage = monsterAtk;
        
        temProjectile.AddListenerMonsterProjectileAttackEvent(MonsterProjectileAttackEvent2); // BattleManager���� �޾ƿ� Damaged �Լ��� Add �س��� MonsterAttackEvent�� �����ϴ� �Լ��� MonsterProjectile�� AddListener �Լ��� �����ش�.
    }
/*    public void AddDamageAction(UnityAction<int> NewAction)
    {
        temProjectile.monsterDamage += () => { NewAction; };
    }*/
    public void AddListenerMonsterAttackToPlayerEvent(UnityAction<int> NewAction) 
    {
        Debug.Log(NewAction);
        /*        if (MonsterAttackEvent != null)
                {
                    MonsterAttackEvent.AddListener(NewAction); // UnityAction ���·� �Ѿ�� BattleManager�� Damaged(int a) �Լ��� int monsterAtk ���ڸ� �־ MonsterAttackEvent�� Add
                }
                else
                {
                    MonsterAttackEvent = new UnityEvent<int>();
                    MonsterAttackEvent.AddListener(NewAction); // UnityAction ���·� �Ѿ�� BattleManager�� Damaged(int a) �Լ��� int monsterAtk ���ڸ� �־ MonsterAttackEvent�� Add
                }*/
        MonsterProjectileAttackEvent2 = NewAction;


    }
    public void StartBattle()
    {
        battle = true;
    }
    /*    private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "PlayerAttack":
                    Debug.Log("���� �ǰ�");
                    break;
            }
        }*/
    public void MonsterDamaged(int damage)
    {
        monsterHp -= damage;
        Debug.Log("�ǰ�. ���� HP : " + monsterHp);
    }
    private void DestroyMonster()
    {
        Destroy(gameObject);
    }
/*    private void MonsterProjectileAttackEvent(int damage)
    {
        MonsterAttackEvent.Invoke(damage); // BattleManager���� �޾ƿ� Damaged �Լ��� Add �س��� MonsterAttackEvent�� ����
    }*/
}

