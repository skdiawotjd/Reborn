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
    private bool battleStart; // Ž�� ����
    private bool adventureStart; // ���� ����
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
        Character.instance.MyPlayerController.PlayAttackProcess();
    }
    public void Damaged(int damage)
    {
        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -damage);
        Debug.Log("�÷��̾� �ǰ�. ���� HP : " + Character.instance.ActivePoint);
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