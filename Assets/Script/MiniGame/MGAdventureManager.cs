using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private AdventurePortal portal;
    private int enemyNumber;
    private float spriteNumber;
    private string enemyNumberString;

    public int currentEnemyCount = 0;

    private List<Dictionary<string, object>> AdventureEnemyList;
    private List<Dictionary<string, object>> AdventureEnemyLevelList;

    protected override void Start()
    {
        base.Start();
        adventureCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //portal = GameObject.Find("AdventurePortal").GetComponent<AdventurePortal>();
        enemyCountText = adventureCanvas.transform.GetChild(9).GetComponent<TextMeshProUGUI>();
        gameRoundText = adventureCanvas.transform.GetChild(10).GetComponent<TextMeshProUGUI>();
        // 모험의 난이도별 각 라운드의 몬스터 레벨, 몬스터 개체 수 등을 가진 CSVRead
        AdventureEnemyList = CSVReader.Read("AdventureEnemyList");
        AdventureEnemyLevelList = CSVReader.Read("AdventureEnemyLevelList");
        adventureEnemyLevel = new List<int>();
        adventureEnemyCount = new List<int>();
        maxRound = 5;
        adventureLevel = QuestManager.instance.GetAdventureLevel();

        InitSpawnObject();

        GameStart();
    }
/*    private void InitObjectList()
    {
        for(int i = 0; i < maxRound; i++)
        {
            temRound = i;
            adventureEnemyLevel[temRound] = new List<int>();
            adventureEnemyCount[temRound] = new List<int>();
        }
    }*/
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
    private void Update()
    {
        
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
            if (currentRound == maxRound - 1) // 현재 라운드가 마지막 라운드이면
                GameEnd(true);
            else
            {
                // 다음 라운드로 진행하는 포탈 활성화
                portal.gameObject.SetActive(true);
            }
        }
    }
    private void ChangeUIEnemyCount()
    {
        // EnemyCount 텍스트 변경
        enemyCountText.text = currentEnemyCount.ToString();
    }
    public override void GameStart()
    {
        Debug.Log("게임 시작");
        // GameStart 텍스트 활성화
        gameRoundText.gameObject.SetActive(true);
        Invoke("SetGame", 3f); // 3초 뒤에 SetGame
    }
    public override void GameEnd(bool clear)
    {
        Debug.Log("GameEnd");
        enemyCountText.gameObject.SetActive(false);
        gameRoundText.gameObject.SetActive(false);
        AdventureGameManager.instance.battleManager.AdventureEnd();
        Character.instance.MyPlayerController.DisableCollider();
        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0013");
        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -10);
        // 모험 보상 수령
        Character.instance.SetCharacterStat(CharacterStatType.MyItem, "00003000");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
    }

    public override void SetRound(int round)
    {
        portal.gameObject.SetActive(false);
        // 라운드 N 텍스트 활성화
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
        // 남은 몬스터 수 표시하는 텍스트 SetActive
        enemyCountText.gameObject.SetActive(true);
        SetRound(0); // 0라운드 시작
    }
    public override void SetMainWork()
    {

    }
    public override void Generate()
    {
        for (int i = 0; i < adventureEnemyCount[currentRound]; i++) // 현재 라운드의 EnemyCount 만큼 반복
        {
            AdventureGameManager.instance.spawner.Spawn(GetEnemyNumber());
        }
    }
}
