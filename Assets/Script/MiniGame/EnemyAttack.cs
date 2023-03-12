using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class EnemyAttack : MonoBehaviour, IPoolObject
{
    private Transform startPosition;
    private Vector3 endPosition;
    private int damage;
    private string idName;
    private Vector2 direction;
    private Quaternion angleAxis;
    private Quaternion rotation;

    private float lerpTime = 0.5f;
    private float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnCreatedInPool()
    {
        idName = "EnemyAttack";
    }
    public void OnGettingFromPool()
    {
        endPosition = Character.instance.transform.position;
        
        currentTime = 0;
        
        StartCoroutine("TargetChase", 0.001f);
    }
    private void SetProjectileRotation()
    {
        direction.x = this.transform.position.x - endPosition.x;
        direction.y = this.transform.position.y - endPosition.y;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angleAxis = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        rotation = Quaternion.Slerp(this.transform.rotation, angleAxis, 5000);
        this.transform.rotation = rotation;
    }
    public void SetStartPosition(Transform start)
    {
        startPosition = start;
        transform.position = startPosition.position;
        SetProjectileRotation();
    }
    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
    private void ReturnObject()
    {
        StopAllCoroutines();
        this.transform.SetParent(AdventureGameManager.instance.pool.transform);
        AdventureGameManager.instance.pool.TakeToPool<EnemyAttack>(this.idName, this);
        
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
