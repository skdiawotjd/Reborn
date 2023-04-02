using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector2 m_Position;
    private SpawnData[] spawnDatas;
    //private int level;
    private int maxLevel;
    private List<Enemy> enemyList;
    private Enemy temEnemy;

    private List<Dictionary<string, object>> AdventureEnemyDataList;

    void Start()
    {
        enemyList = new List<Enemy>();
        m_Position = new Vector2(0f, 0f);
        // 몬스터 넘버별 데이터를 CSVRead
        // MaxLevel = 몬스터 넘버별 데이터의 개수
        // MaxLevel만큼 스폰데이터를 생성
        maxLevel = 5;
        AdventureEnemyDataList = CSVReader.Read("AdventureEnemyDataList");
        maxLevel = AdventureEnemyDataList.Count;
        spawnDatas = new SpawnData[maxLevel];
        InitSpawnDatas();

    }

    private void InitSpawnDatas()
    {
        // 몬스터 넘버별 데이터를 CSVRead
        // 데이터 입력
        for(int i = 0; i < spawnDatas.Length; i++)
        {
            spawnDatas[i] = new SpawnData();
            spawnDatas[i].health = int.Parse(AdventureEnemyDataList[i]["Health"].ToString());
            spawnDatas[i].speed = float.Parse(AdventureEnemyDataList[i]["Speed"].ToString());
            spawnDatas[i].damage = int.Parse(AdventureEnemyDataList[i]["EnemyNumber"].ToString());
            spawnDatas[i].distance = int.Parse(AdventureEnemyDataList[i]["Distance"].ToString());
        }

    }
    public void Spawn(int number)
    {
        temEnemy = AdventureGameManager.instance.pool.GetFromPool<Enemy>(0);
        m_Position.x = Random.Range(-4f, 8.5f);
        m_Position.y = Random.Range(-5f, 2.7f);
        temEnemy.transform.position = m_Position;
        temEnemy.Init(spawnDatas[number]);
        AdventureGameManager.instance.MGManager.ChangeEnemyCount(1);
        enemyList.Add(temEnemy);
    }
    public void EnemyStop()
    {
        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GameStop();
        }
    }
}
public class SpawnData
{
    public int health;
    public int damage;
    public float speed;
    public float distance;
}
