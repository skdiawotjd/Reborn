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
    private Vector3 lookDirection;
    private Quaternion lookRotation;
    private Vector3 myPos;
    private Vector3 targetPos;
    private Vector3 quaternionToTarget;
    private Quaternion targetRotation;
    private int damage;
    private string idName;

    private float lerpTime = 0.5f;
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
        
        currentTime = 0;
        
        StartCoroutine("TargetChase", 0.001f);
    }
    private void SetProjectileRotation()
    {
        Debug.Log("투사체 위치 : " + this.transform.position.ToString());
        Debug.Log("캐릭터 위치 : " + endPosition.ToString());
        Vector2 direction2 = new Vector2(this.transform.position.x - endPosition.x, this.transform.position.y - endPosition.y);
        float angle = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
        Debug.Log("두 오브젝트 사이의 각도 : " + angle);
        Quaternion angleAxis = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(this.transform.rotation, angleAxis, 5000);
        Debug.Log("rotation 값 : " + angleAxis.ToString());
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
