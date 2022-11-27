using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUIManager : MonoBehaviour
{
    [SerializeField]
    private UIManager MainUI;
    [SerializeField]
    private UIManager InvenPanel;
    [SerializeField]
    private UIManager MiniMapPanel;
    [SerializeField]
    private UIManager SettingPanel;
    [SerializeField]
    private UIManager QuestPanel;

    [SerializeField]
    private List<UIManager> UIManagerList;

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
            /*switch (Type)
            {
                case UIPopUpOrder.MainUI:
                    UIManagerList[(int)Type].SetActivePanel();
                    //InvenPanel.SetActivePanel();
                    //InvenPanel.SetActive(!InvenPanel.activeSelf);
                    break;
                case UIPopUpOrder.InvenPanel:
                    UIManagerList[(int)Type].SetActivePanel();
                    //InvenPanel.SetActivePanel();
                    //InvenPanel.SetActive(!InvenPanel.activeSelf);
                    break;
                case UIPopUpOrder.MiniMapPanel:
                    UIManagerList[(int)Type].SetActivePanel();
                    //MiniMapPanel.SetActivePanel();
                    break;
                case UIPopUpOrder.SettingPanel:
                    UIManagerList[(int)Type].SetActivePanel();
                    //SettingPanel.SetActive(!SettingPanel.activeSelf);
                    break;
                case UIPopUpOrder.QuestPanel:
                    UIManagerList[(int)Type].SetActivePanel();
                    //QuestPanel.SetActive(!QuestPanel.activeSelf);
                    break;
            }*/
        }
    }

    private void AllClosePopUpUI()
    {
        /*foreach (var EachUIManager in UIManagerList)
        {
            EachUIManager.SetActivePanel(false);
        }*/
        for(int i = 1; i < UIManagerList.Count; i++)
        {
            UIManagerList[i].SetActivePanel(false);
        }
        
        //InvenPanel.SetActivePanel(false);
        //MiniMapPanel.SetActivePanel(false);
        //SettingPanel.SetActive(false);
        //QuestPanel.SetActive(false);
    }

    private void SceneMovePopUI()
    {
        // 미니게임 씬이거나 하루가 시작되지 않았는데 저챗 씬에 있으면
        if(GameManager.instance.SceneName == "MiniGame" || (!GameManager.instance.IsDayStart && GameManager.instance.SceneName == "JustChat"))
        {
            MainUI.SetActivePanel(false);
        }
        else
        {
            MainUI.SetActivePanel(true);
        }
    }
}
