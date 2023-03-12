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
    private Vector3 rotatePosition;
    private float rotateSpeed;

    void Start()
    {
        rotatePosition = new Vector3(0f, 0f, 0f);
        rotateSpeed = 16f;
        speed = 5f;
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
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * 8f), transform.position.y, transform.position.z);
            rotatePosition.z += Time.deltaTime * rotateSpeed;
            transform.Rotate(rotatePosition);
        } else
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
        
    }
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                AdventureGameManager.instance.battleManager.MonsterDamaged(damage);
                DestroyProjectile();
                break;
        }
    }
}
