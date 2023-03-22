using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MGAdventureManager : MiniGameManager
{
    private int adventureLevel;
    private int adventureRound;
    private List<int> adventureEnemyLevel;
    private List<int> adventureEnemyCount;
    private int enemyLevel;
    private int maxRound;
    private int temRound;
    private int currentRound;
    private Canvas adventureCanvas;
    private TextMeshProUGUI enemyCountText;
    private TextMeshProUGUI gameRoundText;
    private Image endGamePanel;
    private AdventurePortal portal;
    private int enemyNumber;
    private float spriteNumber;
    private string enemyNumberString;
    private float randomObjectNumber;
    private int goodsNumber;
    private bool isClear;

    public int currentEnemyCount = 0;

    private List<Dictionary<string, object>> AdventureEnemyList;
    private List<Dictionary<string, object>> AdventureEnemyLevelList;
    private List<Dictionary<string, object>> AdventureReward;

    protected override void Start()
    {
        base.Start();
        adventureCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //portal = GameObject.Find("AdventurePortal").GetComponent<AdventurePortal>();
        enemyCountText = adventureCanvas.transform.GetChild(9).GetComponent<TextMeshProUGUI>();
        gameRoundText = adventureCanvas.transform.GetChild(10).GetComponent<TextMeshProUGUI>();
        endGamePanel = adventureCanvas.transform.GetChild(11).GetComponent<Image>();
        // ������ ���̵��� �� ������ ���� ����, ���� ��ü �� ���� ���� CSVRead
        AdventureEnemyList = CSVReader.Read("AdventureEnemyList");
        AdventureEnemyLevelList = CSVReader.Read("AdventureEnemyLevelList");
        AdventureReward = CSVReader.Read("AdventureReward");
        adventureEnemyLevel = new List<int>();
        adventureEnemyCount = new List<int>();
        maxRound = 5;
        adventureLevel = QuestManager.instance.GetAdventureLevel();

        InitSpawnObject();

        GameStart();
    }
    public void SetAdventurePortal(AdventurePortal adventurePortal)
    {
        portal = adventurePortal;
    }
    private void InitSpawnObject()
    {
        for(int i = 0; i < maxRound; i++)
        {
            temRound = i;
            adventureEnemyLevel.Add(int.Parse(AdventureEnemyList[adventureLevel]["EnemyLevel" + i].ToString()));
            adventureEnemyCount.Add(int.Parse(AdventureEnemyList[adventureLevel]["EnemyCount" + i].ToString()));
        }
    }
    public void ChangeEnemyCount(int count)
    {
        if(count > 0)
        {
            currentEnemyCount++;
        }
        else
        {
            currentEnemyCount--;
        }
        ChangeUIEnemyCount();
        if(currentEnemyCount == 0)
        {
            if (currentRound == maxRound - 1) // ���� ���尡 ������ �����̸�
                GameEnd(true);
            else
            {
                // ���� ����� �����ϴ� ��Ż Ȱ��ȭ
                portal.gameObject.SetActive(true);
            }
        }
    }
    private void ChangeUIEnemyCount()
    {
        // EnemyCount �ؽ�Ʈ ����
        enemyCountText.text = currentEnemyCount.ToString();
    }
    public override void GameStart()
    {
        Debug.Log("���� ����");
        // GameStart �ؽ�Ʈ Ȱ��ȭ
        gameRoundText.gameObject.SetActive(true);
        Invoke("SetGame", 3f); // 3�� �ڿ� SetGame
    }
    public override void GameEnd(bool clear)
    {
        isClear = clear;
        Debug.Log("GameEnd");
        enemyCountText.gameObject.SetActive(false);
        gameRoundText.gameObject.SetActive(false);
        endGamePanel.gameObject.SetActive(true);
        AdventureGameManager.instance.battleManager.AdventureEnd();
        Character.instance.MyPlayerController.DisableCollider();
        // ���� ���� ����
        Character.instance.SetCharacterStat(CharacterStatType.MyItem, "00003000");
        Character.instance.SetCharacterStat(CharacterStatType.MyItem, RandomReward() + "3");

        Invoke("EndAndClear", 5f);
    }
    private void EndAndClear()
    {
        QuestManager.instance.MinigameClear(isClear);
    }
    private string RandomReward()
    {
        randomObjectNumber = Random.Range(0.001f, 1.000f);
        for (int i = 0; i < AdventureReward.Count; i++)
        {
            if (randomObjectNumber <= float.Parse(AdventureReward[i]["Rate"].ToString()))
            {
                goodsNumber = i;
                return AdventureReward[goodsNumber]["ItemNumber"].ToString();
            }
        }
        return null;
    }
    public override void SetRound(int round)
    {
        portal.gameObject.SetActive(false);
        // ���� N �ؽ�Ʈ Ȱ��ȭ
        gameRoundText.gameObject.SetActive(true);
        // roundText.text = "Round " + round;
        gameRoundText.text = "Round " + round.ToString();
        currentRound = round;
        SetEnemyLevel();
        Invoke("Generate", 1f);
    }
    public void NextRound()
    {
        currentRound++;
        SetRound(currentRound);
    }
    private void SetEnemyLevel()
    {
        for (int i = 0; i < AdventureEnemyLevelList.Count; i++)
        {
            if (int.Parse(AdventureEnemyLevelList[i]["EnemyLevel"].ToString()) == adventureEnemyLevel[currentRound])
            {
                enemyLevel = i;
            }
        }
    }
    private int GetEnemyNumber()
    {
        if (AdventureEnemyLevelList[enemyLevel].Count - 1 == 1)
        {
            return int.Parse(AdventureEnemyLevelList[enemyLevel]["Sprite0"].ToString());
        }
        else
        {
            spriteNumber = Random.Range(0, AdventureEnemyLevelList[enemyLevel].Count - 1);
            enemyNumberString = AdventureEnemyLevelList[enemyLevel]["Sprite" + spriteNumber].ToString();
            enemyNumber = int.Parse(enemyNumberString);
            return enemyNumber;
        }
    }
    public override void SetGame()
    {
        // ���� ���� �� ǥ���ϴ� �ؽ�Ʈ SetActive
        enemyCountText.gameObject.SetActive(true);
        SetRound(0); // 0���� ����
    }
    public override void SetMainWork()
    {

    }
    public override void Generate()
    {
        for (int i = 0; i < adventureEnemyCount[currentRound]; i++) // ���� ������ EnemyCount ��ŭ �ݺ�
        {
            AdventureGameManager.instance.spawner.Spawn(GetEnemyNumber());
        }
    }
}
