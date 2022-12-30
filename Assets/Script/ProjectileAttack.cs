using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerProjectile projectile;
    private PlayerProjectile temProjectile;
    [SerializeField]
    private Transform pos;
    private Vector3 generatePosition;
    private float curtime;
    private float cooltime;
    void Start()
    {
        generatePosition = new Vector3(0f, 0f, 0f);
        cooltime = 0.5f;
    }

    void Update()
    {
/*        if(curtime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                ProjectileFire();
            }
            curtime = cooltime;
        }
        curtime -= Time.deltaTime;*/
    }
    public void ProjectileFire()
    {
        temProjectile = Instantiate(projectile, pos.position, Quaternion.identity) as PlayerProjectile;
    }
/*    public void GenerateProjectile(UnityAction addEvent)
    {
        //temProjectile.AddListenerPlayerProjectileAttackEvent(addEvent);
    }*/

}
