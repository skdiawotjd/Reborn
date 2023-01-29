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
                    MonsterProjectileAttackEvent.AddListener(NewAction); //BattleManager���� �޾ƿ� Damaged �Լ��� Add �س��� MonsterAttackEvent�� MonsterProjectileAttackEvent�� Add
                }
                else
                {
                    MonsterProjectileAttackEvent = new UnityEvent<int>();
                    MonsterProjectileAttackEvent.AddListener(NewAction); //BattleManager���� �޾ƿ� Damaged �Լ��� Add �س��� MonsterAttackEvent�� MonsterProjectileAttackEvent�� Add
                }*/
        MonsterProjectileAttackEvent = NewAction;


    }
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("������ ���ݿ� �浹�� ��ü" + collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Player":
                //MonsterProjectileAttackEvent.Invoke(damage); //BattleManager���� �޾ƿ� Damaged �Լ��� Add �س��� MonsterAttackEvent�� Add�� MonsterProjectileAttackEvent�� ����
                MonsterProjectileAttackEvent(damage);
                DestroyProjectile();
                break;
        }
    }
}