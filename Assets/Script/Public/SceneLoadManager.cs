using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Noble;
    [SerializeField]
    private GameObject DoorToTown;
    [SerializeField]
    private GameObject DoorToJustChat;
    [SerializeField]
    private GameObject DoorToTest;
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
    private GameObject Minigame2Background;
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
                temMap = Instantiate(TownBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                temObject = Instantiate(Tree0) as GameObject;
                temObject = Instantiate(Tree1) as GameObject;
                temObject = Instantiate(Tree2) as GameObject;
                temObject = Instantiate(Tree3) as GameObject;
                temObject = Instantiate(Tree4) as GameObject;
                temObject = Instantiate(DoorToJustChat) as GameObject;
                temObject = Instantiate(DoorToTest) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                temNPC = Instantiate(Noble) as GameObject;
                break;
            case "0001": // 노예의 Town
                temMap = Instantiate(TownBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameSlayerDDR0NPC) as GameObject;
                temNPC = Instantiate(MiniGameSlayerObject0NPC) as GameObject;
                temNPC = Instantiate(MiniGameSlayerObject1NPC) as GameObject;
                temNPC = Instantiate(QuestSlayerChat0NPC) as GameObject;
                temMap.name = "Background";
                break;
            case "0002": // Minigame 빨래
                temMap = Instantiate(Minigame1Background, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameNPC) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                temMap.name = "Background";
                break;            
            case "0005": // Minigame 채집
                temMap = Instantiate(Minigame2Background, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameNPC) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                temMap.name = "Background";
                break;            
            case "0105": // Minigame 청소
                temMap = Instantiate(Minigame1Background, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameNPC) as GameObject;
                temObject = Instantiate(DoorToTown) as GameObject;
                temMap.name = "Background";
                break;
            default:
                temMap = Instantiate(TownBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                break;
                
        }

        Background = GameObject.Find("Background").GetComponent<RectTransform>();

        MapSettingEvent.Invoke();
    }


}
