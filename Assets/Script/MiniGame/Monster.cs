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
    private int monsterHp;
    private int monsterAtk;
    public bool battle;
    private float curtime;
    private float cooltime;
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
        temProjectile.SetDamage(monsterAtk);
        Spum._anim.SetTrigger("Attack");
    }
    public void StartBattle()
    {
        battle = true;
    }
    public void MonsterDamaged(int damage)
    {
        monsterHp -= damage;
        Debug.Log("피격. 남은 HP : " + monsterHp);
    }
    private void DestroyMonster()
    {
        Destroy(gameObject);
    }
}

