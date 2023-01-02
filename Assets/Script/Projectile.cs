using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public int damage;
    private float speed;
    private bool direction;
    public UnityAction<int> MonsterProjectileAttackEvent;
    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        Invoke("DestroyProjectile", 3);
        direction = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
    }

    public void AddListenerMonsterProjectileAttackEvent(UnityAction<int> NewAction)
    {
        /*        if (MonsterProjectileAttackEvent != null)
                {
                    MonsterProjectileAttackEvent.AddListener(NewAction); //BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 MonsterProjectileAttackEvent에 Add
                }
                else
                {
                    MonsterProjectileAttackEvent = new UnityEvent<int>();
                    MonsterProjectileAttackEvent.AddListener(NewAction); //BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 MonsterProjectileAttackEvent에 Add
                }*/
        MonsterProjectileAttackEvent = NewAction;


    }
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("몬스터의 공격에 충돌한 개체" + collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Player":
                //MonsterProjectileAttackEvent.Invoke(damage); //BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 Add한 MonsterProjectileAttackEvent를 실행
                MonsterProjectileAttackEvent(damage);
                DestroyProjectile();
                break;
        }
    }
}