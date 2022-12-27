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

    AssetBundle bundle;
    AssetBundle bundleP;
    AssetBundle temBundle;

    List<Dictionary<string, object>> MapNumber;


    // ���� ũ��
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
            case "0000": // �뿹�� Home 
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "home"));
                
                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfSlayer"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(5);
                temObject.GetComponent<SpriteRenderer>().color = new Color(90, 0, 255, 150);
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(5, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToExplore";
                temObject.GetComponent<Portal>().ChangeSceneName(13);
                temNPC = Instantiate(bundle.LoadAsset<GameObject>("Noble"), new Vector3(1, -5, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(bundleP.LoadAsset<GameObject>("ButlerNPC"), new Vector3(-6, -4, transform.position.z), Quaternion.identity) as GameObject;

                bundle.Unload(false);
                break;
            case "0001": // �뿹�� Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";

                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName(4);
                temObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 150);

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(5);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0105");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "û������";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(-12, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(7);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0002");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "��������";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("QuestChatNPC"), new Vector3(13, 5.5f, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(9);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9999");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "������������";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(6);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0005");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "ä��ä��NPC";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("QuestChatNPC"), new Vector3(13, -9, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(9);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9797");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "��������";

                bundle.Unload(false);
                break;
            case "0002": // Minigame Slayer�� ����
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameslayer"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("LaundryOfSlayer"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGDDRManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGDDRManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-8, 0, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, 0, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(5);
                temObject.GetComponent<SpriteRenderer>().color = new Color(90, 0, 255, 150);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0005": // Minigame Slayer�� ä��
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameslayer"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("MineOfSlayer"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGObjectManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGObjectManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-8, -3, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(5);
                temObject.GetComponent<SpriteRenderer>().color = new Color(90, 0, 255, 150);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;            
            case "0105": // MinigameSlayer�� û��
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "home"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfSlayer"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(5);
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGObjectManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGObjectManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;


                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0100": // ��� �� �Ʒ��ܰ��� Home
            case "0200":
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "homeofcommons"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(8);
                temNPC = Instantiate(bundleP.LoadAsset<GameObject>("ButlerNPC"), new Vector3(-6, -4, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<ButlerNPC>().SetNpcNumber(10);

                bundle.Unload(false);
                break;
            case "0300": // ��� �� ���ܰ��� Home
            case "0400":
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "homeofcommons"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(12);

                bundle.Unload(false);
                break;
            case "0003": // Minigame �������� ����
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ForgeOfSmith"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGTimingManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGTimingManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(8);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0004": // Minigame ���� �Ǹű�� ����
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameedu"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("EduCenter"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGQuizManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGQuizManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(10);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0104": // Minigame �������� �Ǹű�� ����
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameedu"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("EduCenter"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGQuizManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGQuizManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(8);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0205": // Minigame ���� �Ǹ�
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofbania"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ShopOfBania"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGObjectManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGObjectManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(10);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0101": // ������ Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";

                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName(9);
                temObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 150);

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("QuestChatNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(11);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9999");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "�������޳뿹";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(13);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0004");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "����NPC";
                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(13, -9, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(14);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0205");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "��������";

                bundle.Unload(false);
                break;
            case "0201": // ���������� Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName(9);
                temObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 150);

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(13, 5.5f, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(12);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0003");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "������������";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("QuestChatNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(11);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9999");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "�������޳뿹";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(13);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0104");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "����NPC";

                bundle.Unload(false);
                break;
            case "0301": // ������� Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName(9);
                temObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 150);

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(13, -9, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(15);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0305");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "��������";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(13);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0004");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "����NPC";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(16);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0202");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "�������NPC";

                bundle.Unload(false);
                break;
            case "0401": // ������ Town
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "town"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("TownBackground"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToHome";
                temObject.GetComponent<Portal>().ChangeSceneName(9);
                temObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 150);

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(13, 5.5f, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(17);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0103");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "������������";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(13);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0104");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "����NPC";

                temNPC = Instantiate(bundle.LoadAsset<GameObject>("MiniGameTownNPC"), new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(18);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0102");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "�������NPC";

                bundle.Unload(false);
                break;
            case "0102": // ������ �������
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameedu"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("EduCenter"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGDDRManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGDDRManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(12);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0202": // ������� �������
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameedu"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("EduCenter"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGDDRManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGDDRManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -4, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(11);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0103": // ������ ����
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ForgeOfSmith"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGTimingManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGTimingManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(12);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "0305": // ������� �Ǹ�
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofbania"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ShopOfBania"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("MGObjectManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "MGObjectManager";
                temNPC = Instantiate(temBundle.LoadAsset<GameObject>("MiniGameNPC"), new Vector3(-7, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundleP.LoadAsset<GameObject>("Door"), new Vector3(8, -2, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "DoorToTown";
                temObject.GetComponent<Portal>().ChangeSceneName(11);

                bundle.Unload(false);
                temBundle.Unload(false);
                break;
            case "4444": // ���������� Ž��
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigameofsmith"));
                temBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "minigamep"));

                temMap = Instantiate(bundle.LoadAsset<GameObject>("ExploreOfCave"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(temBundle.LoadAsset<GameObject>("ExploreManager"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temObject.name = "ExploreManager";

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
