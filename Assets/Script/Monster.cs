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
                // 죽는 애니메이션, 1초 대기 코루틴 추가할 것
                Invoke("DestroyMonster", 3f);
                //DestroyMonster();
            }
        }

    }
    public void ProjectileFire()
    {
        temProjectile = Instantiate(projectile, transform.position, transform.rotation) as Projectile;
        temProjectile.damage = monsterAtk;
        
        temProjectile.AddListenerMonsterProjectileAttackEvent(MonsterProjectileAttackEvent2); // BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 실행하는 함수를 MonsterProjectile의 AddListener 함수에 보내준다.
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
                    MonsterAttackEvent.AddListener(NewAction); // UnityAction 형태로 넘어온 BattleManager의 Damaged(int a) 함수를 int monsterAtk 인자를 넣어서 MonsterAttackEvent에 Add
                }
                else
                {
                    MonsterAttackEvent = new UnityEvent<int>();
                    MonsterAttackEvent.AddListener(NewAction); // UnityAction 형태로 넘어온 BattleManager의 Damaged(int a) 함수를 int monsterAtk 인자를 넣어서 MonsterAttackEvent에 Add
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
                    Debug.Log("몬스터 피격");
                    break;
            }
        }*/
    public void MonsterDamaged(int damage)
    {
        monsterHp -= damage;
        Debug.Log("피격. 남은 HP : " + monsterHp);
    }
    private void DestroyMonster()
    {
        Destroy(gameObject);
    }
/*    private void MonsterProjectileAttackEvent(int damage)
    {
        MonsterAttackEvent.Invoke(damage); // BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 실행
    }*/
}

