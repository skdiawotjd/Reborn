using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class SceneLoadManager : MonoBehaviour
{
    private GameObject temMap;
    private GameObject temNPC;
    private GameObject temObject;
    private BattleManager temBattleManager;
    private ExploreManager temExploreManager;
    private AdventureGameManager temAdventureGameManager;
    private AdventurePortal temAdventurePortal;
    private JustChatManager temJustChatManager;

    AssetBundle bundle;
    AssetBundle bundleP;
    AssetBundle temBundle;

    List<Dictionary<string, object>> MapNumber;


    // 씬의 크기
    public RectTransform Background;

    public UnityEvent MapSettingEvent;
    public static SceneLoadManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    void Start()
    {
        GameManager.instance.AddSceneMoveEvent(MapSetting);
        MapNumber = CSVReader.Read("MapNumber");
    }
    void Update()
    {
        
    }
    public void MapSetting()
    {
        bundleP = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "public"));
        switch (Character.instance.MyMapNumber)
        {
            case "0000": // 평민 Home
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "homeofcommons"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0013");
                temNPC = Instantiate(bundleP.LoadAsset<GameObject>("ButlerNPC"), new Vector3(-6, -4, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<ButlerNPC>().SetNpcNumber(10);

                bundle.Unload(false);
                break;
            case "0100": // 준귀족 Home
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "homeofcommons"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0113");

                bundle.Unload(false);
                break;            
            case "0200": // 귀족 Home
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "homeofcommons"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0213");

                bundle.Unload(false);
                break;            
            case "0300": // 왕 Home
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "homeofcommons"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0313");

                bundle.Unload(false);
                break;
            case "0001": // 대장장이 아지트
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ForgeOfSmith"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(0, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0013");
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToManufacture";
                temObject.GetComponent<Portal>().ChangeSceneName("0004");
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToFoundry";
                temObject.GetComponent<Portal>().ChangeSceneName("0003");

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0013": // 평민의 Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName("0000");
                temObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 150);
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(0, 8, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName("0008");

                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(13, 5.5f, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToSmithAgit";
                temObject.GetComponent<Portal>().ChangeSceneName("0001");

                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-18f, 0f, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToPassageToCastle";
                temObject.GetComponent<Portal>().ChangeSceneName("0102");

                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(0f, -10f, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToAdventure";
                temObject.GetComponent<Portal>().ChangeSceneName("0009");

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("QuestNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<QuestGiveNpc>().SetNpcNumber(11);
                temNPC.transform.GetComponent<QuestGiveNpc>().SetOrderString("7010");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "퀘스트부여";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(13);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0005");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "퀴즈NPC";

                bundle.Unload(false);
                break;
            case "0002": // 튜토리얼
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "justchat"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("PassageToCastle"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundle.LoadAsset<GameObject>("JustChatManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "JustChatManager";
                temJustChatManager = temObject.GetComponent<JustChatManager>();
                //temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(7.5f, 0f, 0f), Quaternion.identity) as GameObject;
                temJustChatManager.SetJustChatPortal(bundleP.LoadAsset<GameObject>("Door").GetComponent<Portal>());
                //temNPC = Instantiate(bundle.LoadAsset<GameObject>("JustChatNPC"), new Vector3(5.5f, 0f, 0f), Quaternion.identity) as GameObject;
                //temNPC.name = "JustChatNPC";
                temJustChatManager.SetJustChatNPC(bundle.LoadAsset<GameObject>("JustChatNPC").GetComponent<BasicNpc>());

                bundle.Unload(false);
                break;
            case "0102": // 평민의 통로
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "justchat"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("PassageToCastle"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temNPC = Instantiate(bundle.LoadAsset<GameObject>("PassageNPC"), new Vector3(-8.5f, -1f, transform.position.z), Quaternion.identity) as GameObject;
                //temNPC.transform.GetComponent<QuestGiveNpc>().SetNpcNumber(11);
                //temNPC.transform.GetComponent<QuestGiveNpc>().SetOrderString("7010");
                //temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "경비병";
                bundle.Unload(false);
                break;
            case "0113": // 준귀족의 Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";

                bundle.Unload(false);
                break;
            case "0213": // 귀족의 Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";

                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(18f, 0f, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToCommonsTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0013");

                bundle.Unload(false);
                break;
            case "0004": // Minigame 대장장이 제작
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ForgeOfSmith"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGTimingManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGTimingManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToBase";
                temObject.GetComponent<Portal>().ChangeSceneName("0001");

                bundle.Unload(false);
                temBundle.Unload(false);
                break;                  
            case "0003": // Minigame 대장장이 주조
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ForgeOfSmith"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGDDRManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGDDRManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(7, -2, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToBase";
                temObject.GetComponent<Portal>().ChangeSceneName("0001");

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0105": // Minigame 상인 판매기술 배우기
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameedu"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("EduCenter"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGQuizManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGQuizManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0013");

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0005": // Minigame 대장장이 판매기술 배우기
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameedu"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("EduCenter"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGQuizManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGQuizManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName("0013");

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0008": // 대장장이의 탐험
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ExploreOfCave"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("BattleManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temBattleManager = temObject.GetComponent<BattleManager>();
                temBattleManager.name = "BattleManager";
                temObject.gameObject.SetActive(true);
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("ExploreManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temExploreManager = temObject.GetComponent<ExploreManager>();
                temExploreManager.name = "ExploreManager";

                temBattleManager.SetexploreManager(temExploreManager);
                temExploreManager.SetBattleManager(temBattleManager);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;            
            case "0108": // 대장장이의 탐험
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ExploreOfCave"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("BattleManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temBattleManager = temObject.GetComponent<BattleManager>();
                temBattleManager.name = "BattleManager";
                temObject.gameObject.SetActive(true);
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("ExploreManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temExploreManager = temObject.GetComponent<ExploreManager>();
                temExploreManager.name = "ExploreManager";

                temBattleManager.SetexploreManager(temExploreManager);
                temExploreManager.SetBattleManager(temBattleManager);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0009": // 노예 모험
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "adventure"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("AdventureOfGrass"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";

                temObject = Instantiate(temBundle.LoadAsset<GameObject>("BattleManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temBattleManager = temObject.GetComponent<BattleManager>();
                temBattleManager.name = "BattleManager";
                temObject = Instantiate(bundle.LoadAsset<GameObject>("AdventurePortal"), new Vector3(8.5f, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "AdventurePortal";
                temAdventurePortal = temObject.GetComponent<AdventurePortal>();
                temObject.gameObject.SetActive(false);
                temObject = Instantiate(bundle.LoadAsset<GameObject>("AdventureGameManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "AdventureGameManager";
                temAdventureGameManager = temObject.GetComponent<AdventureGameManager>();
                temAdventureGameManager.SetBattleManager(temBattleManager);
                temObject = Instantiate(bundle.LoadAsset<GameObject>("MGAdventureManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGAdventureManager";
                temObject.GetComponent<MGAdventureManager>().SetAdventurePortal(temAdventurePortal);
                temAdventureGameManager.SetMGAdventureManager(temObject.GetComponent<MGAdventureManager>());
                temObject = Instantiate(bundle.LoadAsset<GameObject>("Spawner"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "Spawner";
                temAdventureGameManager.SetSpawner(temObject.GetComponent<Spawner>());
                temObject = Instantiate(bundle.LoadAsset<GameObject>("PoolManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "PoolManager";
                temAdventureGameManager.SetPoolManager(temObject.GetComponent<PoolManager>());

                temBattleManager.AdventureStart();

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0109": // 대장장이 모험
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "adventure"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("AdventureOfGrass"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("BattleManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temBattleManager = temObject.GetComponent<BattleManager>();
                temBattleManager.name = "BattleManager";
                temObject.gameObject.SetActive(true);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0209": // 상인 모험
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "adventure"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("AdventureOfGrass"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("BattleManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temBattleManager = temObject.GetComponent<BattleManager>();
                temBattleManager.name = "BattleManager";
                temObject.gameObject.SetActive(true);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            default:
                break;
        }
        bundleP.Unload(false);
        Background = GameObject.Find("Background").GetComponent<RectTransform>();

        MapSettingEvent.Invoke();
    }


}
