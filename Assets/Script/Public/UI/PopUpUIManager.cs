using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUIManager : MonoBehaviour
{
    [SerializeField]
    private List<UIManager> _uIManagerList;
    public List<UIManager> UIManagerList
    {
        get
        {
            return _uIManagerList;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    void Start()
    {
        /*GameManager.instance.SceneMove.AddListener(SceneMovePopUI);
        GameManager.instance.DayEnd.AddListener(AllClosePopUpUI);
        GameManager.instance.DayStart.AddListener(AllClosePopUpUI);
        GameManager.instance.LoadEvent.AddListener(AllClosePopUpUI);
        Character.instance.MyPlayerController.EventUIInput.AddListener(VisibleUI);*/

        GameManager.instance.AddGenerateGameEvent(SettingUIGame);
    }

    public void VisibleUI(UIPopUpOrder Type)
    {
        Debug.Log("LoadEvent - PopUpUIManager");
        if (GameManager.instance.IsDayStart)
        {
            UIManagerList[(int)Type].SetActivePanel();
        }
    }

    private void AllClosePopUpUI()
    {
        for (int i = 1; i < UIManagerList.Count; i++)
        {
            UIManagerList[i].SetActivePanel(false);
        }
    }

    public void SettingUIGame()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        GameManager.instance.AddSceneMoveEvent(SceneMovePopUI);
        GameManager.instance.AddDayEnd(AllClosePopUpUI);
        GameManager.instance.AddDayStart(AllClosePopUpUI);
        GameManager.instance.AddLoadEvent(ActiveUIManagerList);
        Character.instance.MyPlayerController.EventUIInput.AddListener(VisibleUI);
    }

    private void SceneMovePopUI()
    {
        // 하루가 시작되지 않았는데 저챗 씬에 있으면
        if ((!GameManager.instance.IsDayStart && GameManager.instance.SceneName == "JustChat"))
        {
            UIManagerList[(int)UIPopUpOrder.MainUI].SetActivePanel(false);
        }
        else
        {
            UIManagerList[(int)UIPopUpOrder.MainUI].SetActivePanel(true);
        }
    }

    public void SetActiveLoad()
    {
        transform.GetChild((int)UIPopUpOrder.SettingPanel).gameObject.SetActive(!transform.GetChild((int)UIPopUpOrder.SettingPanel).gameObject.activeSelf);
    }    

    public void SetActiveLoadPanel()
    {
        SetActiveLoad();
        UIManagerList[(int)UIPopUpOrder.SettingPanel].SetActivePanel();
        UIManagerList[(int)UIPopUpOrder.SettingPanel].GetComponent<UISettingManager>().CheckLoadButton();
        UIManagerList[(int)UIPopUpOrder.SettingPanel].GetComponent<UISettingManager>().SetActivePanel(UISettingPanelOrder.Load);
        
    }

    private void ActiveUIManagerList()
    {
        for (int i = 1; i < UIManagerList.Count; i++)
        {
            UIManagerList[i].gameObject.SetActive(true);
        }
    }
}
