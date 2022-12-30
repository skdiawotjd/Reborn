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
        message = "���͸� �����ƴ�!";
    }

    void Update()
    {
        if (battleStart)
        {
            if (curtime <= 0)
            {
                GenerateProjectile();
                Debug.Log("�߻���");
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
        Debug.Log("���� ��");
        // ������ ��
        battleStart = false;
        exploreManager.ChangeMessage(message);
        exploreManager.GoToNext(5f);
    }
    public void GenerateMonster() // ���� ����
    {
        temMonster = Instantiate(monster, new Vector3(5, -2, 0f), Quaternion.identity) as Monster;
        temMonster.transform.GetChild(0).transform.localScale = new Vector3(-1f, 1f, 1f);
        temMonster.monsterDamage += Damaged;
        temMonster.AddListenerMonsterAttackToPlayerEvent(Damaged); // Damaged �Լ��� UnityAction ���·� monster�� MonsterAttackEvent�� �����ش�.
    }
    private void GenerateProjectile() // �÷��̾� ����ü ����
    {
        temProjectile = Instantiate(projectile, pos.position, Quaternion.identity) as PlayerProjectile;
        temProjectile.SetDamage(playerAtk);
        temProjectile.playerDamage += MonsterDamaged;
    }
    public void Damaged(int damage)
    {
        playerHp -= damage;
        Debug.Log("�÷��̾� �ǰ�. ���� HP : " + playerHp);
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