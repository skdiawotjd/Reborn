using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager2 : MonoBehaviour
{
    [SerializeField]
    private GameObject poolingObjectPrefab; // Enemy 프리팹
    private int playerDamage;
    private Queue<Enemy> poolingObjectQueue = new Queue<Enemy>(); // 큐의 초기화

    private void Awake()
    {
        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        playerDamage = Character.instance.Reputation;
        if(playerDamage < 10)
            playerDamage = 10;
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); // 새로운 개체를 만들어 큐에 집어넣기
        }
    }

    private Enemy CreateNewObject() // 새로운 개체 만들기
    {
        var temObject = Instantiate(poolingObjectPrefab).GetComponent<Enemy>();
        temObject.gameObject.SetActive(false);
        temObject.transform.SetParent(transform);
        temObject.playerDamage = playerDamage;
        return temObject;
    }

    public Enemy GetObject()
    {
        if (poolingObjectQueue.Count > 0) // 큐에 오브젝트가 있으면 빼내어 해당 오브젝트 리턴
        {
            var returnObject = poolingObjectQueue.Dequeue();
            returnObject.transform.SetParent(null);
            returnObject.gameObject.SetActive(true);
            return returnObject;
        }
        else // 큐에 남은 오브젝트가 없다면 새로운 개체 생성
        {
            var newObject = CreateNewObject();
            newObject.gameObject.SetActive(true);
            newObject.transform.SetParent(null);
            return newObject;
        }
    }

    public void ReturnObject(Enemy obj)
    {
        obj.gameObject.SetActive(false); // 비활성화
        obj.transform.SetParent(transform); // 다시 풀로 귀속
        poolingObjectQueue.Enqueue(obj); // 큐로 집어넣기
    }
}
