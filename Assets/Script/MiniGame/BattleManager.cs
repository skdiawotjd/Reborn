using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Redcode.Pools;

public class BattleManager : MonoBehaviour
{
    private PoolManager poolManager;
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
    private bool battleStart; // Ž�� ����
    private bool adventureStart; // ���� ����
    private string message;
    private BoxCollider2D hitArea;
    private Rigidbody2D characterRigid;
    private Boomerang boomerang;
    public bool isAttack;
    void Start()
    {
        gameObject.SetActive(true);
        playerAtk = 50;
        playerHp = Character.instance.ActivePoint;
        cooltime = 3.1f;
        hitArea = Character.instance.transform.GetChild(3).GetComponent<BoxCollider2D>();
        hitArea.enabled = true;
        characterRigid = Character.instance.GetComponent<Rigidbody2D>();
        battleStart = false;
        message = "���͸� �����ƴ�!";
        poolManager = Instantiate(SceneLoadManager.instance.PoolManager);
    }

    void Update()
    {
        if (battleStart)
        {
            if (!temMonster.battle)
            {

            } else
            {
                if (curtime <= 0)
                {
                    GenerateProjectile();
                    Debug.Log("�߻���");
                    curtime = cooltime;
                }
                curtime -= Time.deltaTime;
            }
            
            if (temMonster == null || playerHp <= 0)
            {
                EndBattle();
            }
        }
        if (adventureStart)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (!Character.instance.MyPlayerController.CanAttack() && !isAttack)
                {
                    isAttack = true;
                    Character.instance.MyPlayerController.PlayAttackProcess();
                    for (int i = 0; i < 4; i++)
                    {
                        boomerang = AdventureGameManager.instance.pool.GetFromPool<Boomerang>(1);
                        boomerang.transform.SetParent(Character.instance.transform);
                        boomerang.idName = "Boomerang";
                        boomerang.name = "boomerang";
                        boomerang.SetOrder(i);
                    }
                    
                    Invoke("DisableAttack", 3f);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if(adventureStart)
            characterRigid.velocity = Vector2.zero;
    }
    public void AdventureStart()
    {
        adventureStart = true;
        playerHp = Character.instance.ActivePoint;
    }
    public void AdventureEnd()
    {
        adventureStart = false;
        hitArea.enabled = false;
    }
    public void DisableAttack()
    {
        Debug.Log("isAttack : " + isAttack);
        isAttack = false;
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
        temProjectile = poolManager.GetFromPool<PlayerProjectile>(3);
        temProjectile.transform.SetParent(Character.instance.transform);
        temProjectile.SetDamage(playerAtk);
        temProjectile.playerDamage += MonsterDamaged;
        Character.instance.MyPlayerController.PlayAttackProcess();
    }
    public void Damaged(int damage)
    {
        if (Character.instance.ActivePoint <= damage)
        {
            PlayerDead();
        } else
        {
            Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -damage);
        }
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
    private void PlayerDead() // ���� �� �÷��̾� ���
    {
        Character.instance.MyPlayerController.PlayDieProcess(true);
        Character.instance.SetCharacterInput(false, false, false);
        Invoke("OnDead", 3f);
        
    }
    private void OnDead() // ��� �� �÷��� ���
    {
        playerHp -= (Character.instance.ActivePoint + 20);
        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, playerHp);
        Debug.Log("�÷��̾� ���. ���� ����");
        Character.instance.MyPlayerController.PlayDieProcess(false);
        Character.instance.SetCharacterInput(true, true, true);
        characterRigid.velocity = Vector2.zero;
        AdventureGameManager.instance.MGManager.GameEnd(false);
    }
}