using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUIManager : MonoBehaviour
{
    /*private enum UIOrder { MainUI, InvenPanel, MiniMapPanel, SettingPanel, QuestPanel }
    private List<GameObject> UIList;*/

    [SerializeField]
    private GameObject MainUI;
    [SerializeField]
    private GameObject InvenPanel;
    [SerializeField]
    private GameObject MiniMapPanel;
    [SerializeField]
    private GameObject SettingPanel;
    [SerializeField]
    private GameObject QuestPanel;

    private void Awake()
    {
        /*MainUI = transform.GetChild(0).gameObject;
        InvenPanel = transform.GetChild(2).GetChild(0).gameObject;
        MiniMapPanel = transform.GetChild(3).GetChild(0).gameObject;
        SettingPanel = transform.GetChild(4).GetChild(0).gameObject;
        QuestPanel = transform.GetChild(5).GetChild(0).gameObject;

        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            UIList.Add(gameObject.transform.GetChild(i).gameObject);
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SceneMove.AddListener(SceneMovePopUI);
        GameManager.instance.DayEnd.AddListener(AllClosePopUpUI);
        GameManager.instance.LoadEvent.AddListener(AllClosePopUpUI);
        Character.instance.MyPlayerController.EventUIInput.AddListener(VisibleUI);
    }

    public void VisibleUI(int Type)
    {
        switch(Type)
        {
            case 2:
                InvenPanel.SetActive(!InvenPanel.activeSelf);
                break;
            case 3:
                MiniMapPanel.SetActive(!MiniMapPanel.activeSelf);
                break;
            case 4:
                SettingPanel.SetActive(!SettingPanel.activeSelf);
                break;
            case 5:
                QuestPanel.SetActive(!QuestPanel.activeSelf);
                break;
        }
    }

    private void AllClosePopUpUI()
    {
        InvenPanel.SetActive(false);
        MiniMapPanel.SetActive(false);
        SettingPanel.SetActive(false);
        QuestPanel.SetActive(false);
    }

    private void SceneMovePopUI()
    {
        if(GameManager.instance.SceneName == "MiniGame")
        {
            MainUI.SetActive(true);
        }
        else
        {
            MainUI.SetActive(true);
        }
    }
}
