using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager2 : MonoBehaviour
{
    [SerializeField]
    private GameObject poolingObjectPrefab; // Enemy ������
    private int playerDamage;
    private Queue<Enemy> poolingObjectQueue = new Queue<Enemy>(); // ť�� �ʱ�ȭ

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
            poolingObjectQueue.Enqueue(CreateNewObject()); // ���ο� ��ü�� ����� ť�� ����ֱ�
        }
    }

    private Enemy CreateNewObject() // ���ο� ��ü �����
    {
        var temObject = Instantiate(poolingObjectPrefab).GetComponent<Enemy>();
        temObject.gameObject.SetActive(false);
        temObject.transform.SetParent(transform);
        temObject.playerDamage = playerDamage;
        return temObject;
    }

    public Enemy GetObject()
    {
        if (poolingObjectQueue.Count > 0) // ť�� ������Ʈ�� ������ ������ �ش� ������Ʈ ����
        {
            var returnObject = poolingObjectQueue.Dequeue();
            returnObject.transform.SetParent(null);
            returnObject.gameObject.SetActive(true);
            return returnObject;
        }
        else // ť�� ���� ������Ʈ�� ���ٸ� ���ο� ��ü ����
        {
            var newObject = CreateNewObject();
            newObject.gameObject.SetActive(true);
            newObject.transform.SetParent(null);
            return newObject;
        }
    }

    public void ReturnObject(Enemy obj)
    {
        obj.gameObject.SetActive(false); // ��Ȱ��ȭ
        obj.transform.SetParent(transform); // �ٽ� Ǯ�� �ͼ�
        poolingObjectQueue.Enqueue(obj); // ť�� ����ֱ�
    }
}
