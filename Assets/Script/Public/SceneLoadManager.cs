using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneLoadManager : MonoBehaviour
{
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
    private GameObject TownManager;
    private GameObject temMap;
    private GameObject temNPC;

    // ¾ÀÀÇ Å©±â
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
            case "0001": // ³ë¿¹ÀÇ Town
                temMap = Instantiate(TownBackground, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temNPC = Instantiate(MiniGameSlayerDDR0NPC) as GameObject;
                temNPC = Instantiate(MiniGameSlayerObject0NPC) as GameObject;
                temNPC = Instantiate(MiniGameSlayerObject1NPC) as GameObject;
                temNPC = Instantiate(QuestSlayerChat0NPC) as GameObject;
                temMap.name = "Background";
                break;
            case "0002": // Minigame »¡·¡
                temMap = Instantiate(Minigame1Background, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                break;            
            case "0005": // Minigame Ã¤Áý
                temMap = Instantiate(Minigame2Background, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                temMap.name = "Background";
                break;            
            case "0105": // Minigame »¡·¡
                temMap = Instantiate(Minigame1Background, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
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
