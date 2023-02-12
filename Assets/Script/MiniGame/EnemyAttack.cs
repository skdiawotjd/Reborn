using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class EnemyAttack : MonoBehaviour, IPoolObject
{
    private Transform startPosition;
    private Vector3 endPosition;
    private Rigidbody2D t_Rigidbody;
    private Vector3 direction;
    private int damage;
    private string idName;

    private float lerpTime = 1f;
    private float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {

    }
    public void OnCreatedInPool()
    {
        t_Rigidbody = Character.instance.GetComponent<Rigidbody2D>();
        direction = Vector3.one;
        idName = "EnemyAttack";
    }
    public void OnGettingFromPool()
    {
        endPosition = Character.instance.transform.position;
        if (t_Rigidbody.position.x < transform.position.x)
        {
            direction.x = -1f;
            transform.localScale = direction;
        }
        else
        {
            direction.x = 1f;
            transform.localScale = direction;
        }
        StartCoroutine("TargetChase", 0.001f);

    }
    public void SetStartPosition(Transform start)
    {
        startPosition = start;
        transform.position = startPosition.position;
    }
    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
    private void ReturnObject()
    {
        AdventureGameManager.instance.pool.TakeToPool<EnemyAttack>(this.idName, this);
        //this.transform.SetParent(AdventureGameManager.instance.pool.transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.name)
        {
            case "HitArea":
                AdventureGameManager.instance.battleManager.Damaged(damage);
                ReturnObject();
                Debug.Log("플레이어 피격");
                break;
        }
    }
    IEnumerator TargetChase(float delayTime)
    {
        Debug.Log("타겟 추적중");
        while (currentTime < lerpTime)
        {
            yield return delayTime;
            this.transform.position = Vector3.Lerp(startPosition.position, endPosition, currentTime / lerpTime);
            currentTime += delayTime;
        }
        ReturnObject();
    }
}
