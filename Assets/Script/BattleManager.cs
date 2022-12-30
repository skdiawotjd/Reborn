using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerProjectile projectile;
    private PlayerProjectile temProjectile;
    [SerializeField]
    private Monster monster;
    private Transform pos;
    private ExploreManager exploreManager;
    private Monster temMonster;
    private int playerHp;
    private int playerAtk;
    private float curtime;
    private float cooltime;
    private bool battleStart;
    private string message;
    void Start()
    {
        gameObject.SetActive(true);
        playerAtk = 50;
        playerHp = 100;
        cooltime = 3.1f;
        pos = Character.instance.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Transform>();
        battleStart = false;
        message = "몬스터를 물리쳤다!";
    }

    void Update()
    {
        if (battleStart)
        {
            if (curtime <= 0)
            {
                GenerateProjectile();
                Debug.Log("발사명령");
                curtime = cooltime;
            }
            curtime -= Time.deltaTime;
            if (temMonster == null || playerHp <= 0)
            {
                EndBattle();
            }
        }
    }
    public void StartBattle()
    {
        battleStart = true;
        temMonster.StartBattle();
    }
    private void EndBattle()
    {
        Debug.Log("게임 끝");
        // 게임이 끝
        battleStart = false;
        exploreManager.ChangeMessage(message);
        exploreManager.GoToNext(5f);
    }
    public void GenerateMonster() // 몬스터 생성
    {
        temMonster = Instantiate(monster, new Vector3(5, -2, 0f), Quaternion.identity) as Monster;
        temMonster.transform.GetChild(0).transform.localScale = new Vector3(-1f, 1f, 1f);
        temMonster.monsterDamage += Damaged;
        temMonster.AddListenerMonsterAttackToPlayerEvent(Damaged); // Damaged 함수를 UnityAction 형태로 monster의 MonsterAttackEvent에 보내준다.
    }
    private void GenerateProjectile() // 플레이어 투사체 생성
    {
        temProjectile = Instantiate(projectile, pos.position, Quaternion.identity) as PlayerProjectile;
        temProjectile.SetDamage(playerAtk);
        temProjectile.playerDamage += MonsterDamaged;
    }
    public void Damaged(int damage)
    {
        playerHp -= damage;
        Debug.Log("플레이어 피격. 남은 HP : " + playerHp);
    }
    public void MonsterDamaged(int damage)
    {
        temMonster.MonsterDamaged(damage);
    }

    public void SetexploreManager(ExploreManager NewexploreManager)
    {
        exploreManager = NewexploreManager;
    }
    public bool GetExistTemMonster()
    {
        if (temMonster != null)
        {
            return true;
        } else
        {
            return false;
        }
    }
    public void DestroyTemMonster()
    {
        Destroy(temMonster.gameObject);
    }
}