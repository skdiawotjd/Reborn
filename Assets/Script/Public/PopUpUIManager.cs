using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUIManager : MonoBehaviour
{
    private GameObject MainUI;
    private GameObject InvenPanel;
    private GameObject MiniMapPanel;
    private GameObject SettingPanel;
    private GameObject QuestPanel;

    private void Awake()
    {
        MainUI = transform.GetChild(0).gameObject;
        InvenPanel = transform.GetChild(2).GetChild(0).gameObject;
        MiniMapPanel = transform.GetChild(3).GetChild(0).gameObject;
        SettingPanel = transform.GetChild(4).GetChild(0).gameObject;
        QuestPanel = transform.GetChild(5).GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SceneMove.AddListener(SceneMovePopUI);
        GameManager.instance.DayEnd.AddListener(DayEnd);
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

    private void DayEnd()
    {
        InvenPanel.SetActive(false);
        MiniMapPanel.SetActive(false);
        SettingPanel.SetActive(false);
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
