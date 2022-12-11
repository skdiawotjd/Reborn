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
        // �Ϸ簡 ���۵��� �ʾҴµ� ��ê ���� ������
        if ((!GameManager.instance.IsDayStart && GameManager.instance.SceneName == "JustChat"))
        {
            UIManagerList[(int)UIPopUpOrder.MainUI].SetActivePanel(false);
        }
        else
        {
            UIManagerList[(int)UIPopUpOrder.MainUI].SetActivePanel(true);
        }
    }

    public void VisibleSpecificUI(UIPopUpOrder Type)
    {
        transform.GetChild((int)Type).gameObject.SetActive(true);

        UIManagerList[(int)Type].SetActivePanel();
    }

    private void ActiveUIManagerList()
    {
        for (int i = 1; i < UIManagerList.Count; i++)
        {
            UIManagerList[i].gameObject.SetActive(true);
        }
    }
}
