using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProjectile : MonoBehaviour
{
    private UnityEvent PlayerProjectileAttackEvent;
    private int damage;
    private float speed;
    private bool direction;
    public delegate void Damage(int target);
    public Damage playerDamage;

    void Start()
    {
        PlayerProjectileAttackEvent = new UnityEvent();
        speed = 3f;
        if (Character.instance.MyPlayerController.GetPlayerDirection() == 1)
        {
            direction = false;
        } else
        {
            direction = true;
        }
        Invoke("DestroyProjectile", 3);
    }

    void Update()
    {
        if(direction)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        } else
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
        
    }
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    public void AddListenerPlayerProjectileAttackEvent(UnityAction NewAction)
    {
        Debug.Log(NewAction.GetInvocationList());
        if (PlayerProjectileAttackEvent != null)
        {
            PlayerProjectileAttackEvent.AddListener(NewAction); //BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 MonsterProjectileAttackEvent에 Add
        }
        else
        {
            PlayerProjectileAttackEvent = new UnityEvent();
            PlayerProjectileAttackEvent.AddListener(NewAction); //BattleManager에서 받아온 Damaged 함수를 Add 해놓은 MonsterAttackEvent를 MonsterProjectileAttackEvent에 Add
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("플레이어의 공격에 충돌한 개체" + collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                if(PlayerProjectileAttackEvent != null)
                {
                    //PlayerProjectileAttackEvent.Invoke();
                    playerDamage(damage);
                    DestroyProjectile();
                } else
                {

                }
                break;
        }
    }
}
