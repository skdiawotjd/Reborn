using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGObjectManager : MiniGameManager
{
    [SerializeField]
    private GoldBox GoldBox;
    private GoldBox temBox;
    private int questCount;
    private int maxCount;
    private bool objectGameActive = false;
    private Sprite[] goods;
    [SerializeField]
    private Sprite Goods1;
    [SerializeField]
    private Sprite Goods2;
    [SerializeField]
    private Sprite Goods3;
    [SerializeField]
    private Sprite MineralOfMine;

    private void Awake()
    {
        goods = new Sprite[3];
        goods[0] = Goods1;
        goods[1] = Goods2;
        goods[2] = Goods3;
    }

    protected override void Start()
    {
        base.Start();
        QuestManager.instance.EventCountChange.AddListener(SetMainWork);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void GameStart()
    {
        if (!objectGameActive)
        {
            Generate();
        }
        Character.instance.SetCharacterInput(true, true, true);
        objectGameActive = true;
    }
    public override void GameEnd(bool clear)
    {
        objectGameActive = false;
        QuestManager.instance.MinigameClear(true);
    }
    public override void Generate() // GoldBoxGenerate()
    {
        maxCount = 5;
        for (int i = 0; i < maxCount; i++)
        {
            if (Character.instance.MyMapNumber == "0005")
            {
                temBox = Instantiate(GoldBox, new Vector3(Random.Range(-6f, 6f), Random.Range(-5f, -1f), transform.position.z), Quaternion.identity) as GoldBox;
                temBox.GetComponent<SpriteRenderer>().sprite = MineralOfMine;
            }
            else if (Character.instance.MyMapNumber == "0105")
            {
                temBox = Instantiate(GoldBox, new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, -5f), transform.position.z), Quaternion.identity) as GoldBox;
            }
            else if (Character.instance.MyMapNumber == "0205")
            {
                temBox = Instantiate(GoldBox, new Vector3(Random.Range(-6f, 6f), Random.Range(-5f, -1f), transform.position.z), Quaternion.identity) as GoldBox;
                temBox.transform.localScale = new Vector3(1f, 1f, 1f);
                temBox.GetComponent<SpriteRenderer>().sprite = goods[Random.Range(0, 3)];
            }
            else
            {
                temBox = Instantiate(GoldBox, new Vector3(Random.Range(-5f, 5f), Random.Range(-4f, 4f), transform.position.z), Quaternion.identity) as GoldBox;
            }
        }
    }
    public override void SetMainWork() // BoxCount()
    {
        questCount++;
        Debug.Log("전리품 1개 획득. 현재 개수 : " + questCount + " / " + maxCount);
        if (questCount == maxCount)
        {
            Debug.Log("전리품 획득 완료. 게임 종료");
            questCount = 0;
            GameEnd(true);
        }
    }
}
