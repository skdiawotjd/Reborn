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
    private bool battleStart; // 탐험 시작
    private bool adventureStart; // 모험 시작
    private string message;
    private CircleCollider2D attackArea;
    private BoxCollider2D hitArea;
    private Rigidbody2D characterRigid;
    public GameObject attackParticle;
    private GameObject temParticle;
    void Start()
    {
        gameObject.SetActive(true);
        playerAtk = 50;
        playerHp = 100;
        cooltime = 3.1f;
        attackArea = Character.instance.transform.GetChild(2).GetComponent<CircleCollider2D>();
        hitArea = Character.instance.transform.GetChild(3).GetComponent<BoxCollider2D>();
        hitArea.enabled = true;
        characterRigid = Character.instance.GetComponent<Rigidbody2D>();
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
        if (adventureStart)
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (!Character.instance.MyPlayerController.CanAttack() && !attackArea.enabled)
                {
                    attackArea.enabled = true;
                    Character.instance.MyPlayerController.PlayAttackProcess();
                    temParticle = Instantiate(attackParticle);
                    temParticle.transform.position = new Vector3(Character.instance.transform.position.x, Character.instance.transform.position.y + 0.5f, Character.instance.transform.position.z) ;
                    Invoke("DisableCollider", 0.5f);
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
    }
    public void AdventureEnd()
    {
        adventureStart = false;
        hitArea.enabled = false;
    }
    public void DisableCollider()
    {
        attackArea.enabled = false;
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
        Character.instance.MyPlayerController.PlayAttackProcess();
    }
    public void Damaged(int damage)
    {
        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -damage);
        Debug.Log("플레이어 피격. 남은 HP : " + Character.instance.ActivePoint);
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