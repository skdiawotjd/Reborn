using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject HomeOfSlayer;
    [SerializeField]
    private GameObject HomeOfCommons;
    [SerializeField]
    private GameObject LaundryOfSlayer;
    [SerializeField]
    private GameObject Noble;
    [SerializeField]
    private GameObject Butler;
    [SerializeField]
    private GameObject TownBackground;
    [SerializeField]
    private GameObject MiniGameSlayerDDR0NPC;
    [SerializeField]
    private GameObject MiniGameSlayerObject0NPC;
    [SerializeField]
    private GameObject MiniGameSlayerObject1NPC;
    [SerializeField]
    private GameObject QuestSlayerChat0NPC;
    [SerializeField]
    private GameObject Minigame1Background;
    [SerializeField]
    private GameObject MinigameMineBackground;
    [SerializeField]
    private GameObject MiniGameNPC;
    [SerializeField]
    private GameObject ForgeOfSmith;
    [SerializeField]
    private GameObject ShopOfBania;
    [SerializeField]
    private GameObject EduCenter;
    [SerializeField]
    private GameObject MGDDRManager;
    [SerializeField]
    private GameObject MGObjectManager;
    [SerializeField]
    private GameObject MGTimingManager;
    [SerializeField]
    private GameObject MGQuizManager;
    [SerializeField]
    private GameObject TownManager;
    private GameObject temMap;
    private GameObject temNPC;
    private GameObject temObject;
    private BattleManager temBattleManager;
    private ExploreManager temExploreManager;

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

    // Update is called once per frame
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
                temObject.GetComponent<Portal>().ChangeSceneName("0104");
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

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(13, 5.5f, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(12);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0004");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "수습대장장이";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("QuestChatNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(11);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9999");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "물건전달노예";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(13);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0005");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "퀴즈NPC";

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
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(7, -2, transform.position.z), Quaternion.identity) as GameObject;
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

            default:
                break;
                
        }
        bundleP.Unload(false);
        Background = GameObject.Find("Background").GetComponent<RectTransform>();

        MapSettingEvent.Invoke();
    }


}
