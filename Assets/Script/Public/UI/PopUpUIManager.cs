using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUIManager : MonoBehaviour
{
    [SerializeField]
    private List<UIManager> UIManagerList;

    void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    void Start()
    {
        GameManager.instance.SceneMove.AddListener(SceneMovePopUI);
        GameManager.instance.DayEnd.AddListener(AllClosePopUpUI);
        GameManager.instance.DayStart.AddListener(AllClosePopUpUI);
        GameManager.instance.LoadEvent.AddListener(AllClosePopUpUI);
        Character.instance.MyPlayerController.EventUIInput.AddListener(VisibleUI);
    }

    public void VisibleUI(UIPopUpOrder Type)
    {
        if (GameManager.instance.IsDayStart)
        {
            UIManagerList[(int) Type].SetActivePanel();
        }
    }

    private void AllClosePopUpUI()
    {
        for(int i = 1; i < UIManagerList.Count; i++)
        {
            UIManagerList[i].SetActivePanel(false);
        }
    }

    private void SceneMovePopUI()
    {
        // 미니게임 씬이거나 하루가 시작되지 않았는데 저챗 씬에 있으면
        if(GameManager.instance.SceneName == "MiniGame" || (!GameManager.instance.IsDayStart && GameManager.instance.SceneName == "JustChat"))
        {
            UIManagerList[(int)UIPopUpOrder.MainUI].SetActivePanel(false);
        }
        else
        {
            UIManagerList[(int)UIPopUpOrder.MainUI].SetActivePanel(true);
        }
    }

    public void SettingUIGame()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        GameManager.instance.SceneMove.AddListener(SceneMovePopUI);
        GameManager.instance.DayEnd.AddListener(AllClosePopUpUI);
        GameManager.instance.DayStart.AddListener(AllClosePopUpUI);
        GameManager.instance.LoadEvent.AddListener(AllClosePopUpUI);
        Character.instance.MyPlayerController.EventUIInput.AddListener(VisibleUI);
    }
}
