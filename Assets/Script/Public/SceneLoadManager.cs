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
    private GameObject DoorToHome;
    [SerializeField]
    private GameObject DoorToTown;
    [SerializeField]
    private GameObject DoorToJustChat;
    [SerializeField]
    private GameObject DoorToTest;
    [SerializeField]
    private GameObject Butler;
    [SerializeField]
    private GameObject Tree0;
    [SerializeField]
    private GameObject Tree1;
    [SerializeField]
    private GameObject Tree2;
    [SerializeField]
    private GameObject Tree3;
    [SerializeField]
    private GameObject Tree4;
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
    private GameObject TownManager;
    private GameObject temMap;
    private GameObject temNPC;
    private GameObject temObject;

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
        GameManager.instance.SceneMove.AddListener(MapSetting);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapSetting()
    {
        switch (Character.instance.MyPosition)
        {
            case "0000": // 노예의 Home
                AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles", "home"));
                
                temMap = Instantiate(bundle.LoadAsset<GameObject>("HomeOfCommons"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(bundle.LoadAsset<GameObject>("DoorToTest"), new Vector3(5, -5, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(bundle.LoadAsset<GameObject>("DoorToTown"), new Vector3(6, -5, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(bundle.LoadAsset<GameObject>("Noble"), new Vector3(1, -5, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(bundle.LoadAsset<GameObject>("ButlerNPC"), new Vector3(-6, -4, transform.position.z), Quaternion.identity) as GameObject;

                bundle.Unload(false);
                break;
            case "0001": // 노예의 Town
                temMap = Instantiate(TownBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(DoorToHome, new Vector3(-13, 6, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameSlayerDDR0NPC, new Vector3(-15, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(5);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0105");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "청소장인";
                temNPC = Instantiate(MiniGameSlayerDDR0NPC, new Vector3(-12, -9, transform.position.z), Quaternion.Euler(0, 180.0f, 0)) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(5);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0002");
                temNPC.transform.GetChild(1).Rotate(0, 180.0f, 0);
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "빨래장인";
                temNPC = Instantiate(QuestSlayerChat0NPC, new Vector3(13, 5.5f, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(9);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9999");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "수습대장장이";
                temNPC = Instantiate(MiniGameSlayerObject1NPC, new Vector3(14, -2, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<TownNPC>().SetNpcNumber(6);
                temNPC.transform.GetComponent<TownNPC>().SetquestNumber("0005");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "채집채광NPC";
                temNPC = Instantiate(QuestSlayerChat0NPC, new Vector3(13, -9, transform.position.z), Quaternion.identity) as GameObject;
                temNPC.transform.GetComponent<DeliveryNPC>().SetNpcNumber(9);
                temNPC.transform.GetComponent<DeliveryNPC>().SetOrderString("9797");
                temNPC.transform.GetChild(1).GetComponent<TextMeshPro>().text = "수습상인";
                break;
            case "0002": // Minigame 빨래
                temMap = Instantiate(LaundryOfSlayer, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameNPC, new Vector3(-8, 0, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(DoorToTown, new Vector3(8, 0, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                break;            
            case "0005": // Minigame 채집
                temMap = Instantiate(MinigameMineBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameNPC, new Vector3(-8, -3, transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                temMap.name = "Background";
                break;            
            case "0105": // Minigame 청소
                temMap = Instantiate(HomeOfSlayer, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temNPC = Instantiate(MiniGameNPC, new Vector3(-7,-2,transform.position.z), Quaternion.identity) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                
                break;
            case "0100":
            case "0200":
                temMap = Instantiate(HomeOfCommons, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(DoorToJustChat) as GameObject;
                temObject = Instantiate(DoorToTest) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                break;
            default:
                //temMap = Instantiate(TownBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                //temMap.name = "Background";
                break;
                
        }

        Background = GameObject.Find("Background").GetComponent<RectTransform>();

        MapSettingEvent.Invoke();
    }


}
