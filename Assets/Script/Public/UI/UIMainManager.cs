using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainManager : UIManager
{
    /*[SerializeField]
    private Button SettingButton;
    [SerializeField]
    private Button SkipDayButton;
    [SerializeField]
    private Image ActivePointImage;
    [SerializeField]
    private Image DayImage;
    [SerializeField]
    private Image JobImage;
    [SerializeField]
    private Image TodoProgressImage;*/
    [SerializeField]
    private List<Button> MainButton;
    [SerializeField]
    private List<Image> MainImage;

    protected override void Start()
    {
        base.Start();
        MainButton[(int)UIMainButtonOrder.Setting].onClick.AddListener(() => { transform.GetComponentInParent<PopUpUIManager>().VisibleUI(UIPopUpOrder.SettingPanel); });
        MainButton[(int)UIMainButtonOrder.SkipDay].onClick.AddListener(SkipDay);

        GameManager.instance.AddLoadEvent(LoadUI);
        Character.instance.UIChangeAddListener(ChangeMainUIType);
    }

    protected override void StartUI()
    {
        if (!Panel.activeSelf)
        {
            SetActivePanel(true);
        }

        foreach (var Button in MainButton)
        {
            Button.interactable = true;
        }

        InitializeMainUI();

        StartCoroutine(Timer());
    }

    protected override void EndUI()
    {
        foreach (var Button in MainButton)
        {
            Button.interactable = false;
        }

        StopCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (GameManager.instance.IsDayStart)
        {
            //DayImage.fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);
            MainImage[(int)UIMainImageOrders.Day].fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);

            yield return Time.deltaTime;
        }
    }

    public void ChangeMainUIType(CharacterStatType Type)
    {
        if(Enum.TryParse(Type.ToString(), out UIMainImageOrders TemEnum))
        {
            ChangeMainUI(TemEnum);
        }
    }

    private void ChangeMainUI(UIMainImageOrders Type)
    {
        switch (Type)
        {
            // Qurter
            case UIMainImageOrders.Qurter:
                //ActivePointImage.fillAmount = (float)Character.instance.ActivePoint / 100f;
                MainImage[(int)UIMainImageOrders.Qurter].fillAmount = 1f - (float)(GameManager.instance.Days - 1) / 9f;
                break;
            // ActivePoint
            case UIMainImageOrders.ActivePoint:
                //ActivePointImage.fillAmount = (float)Character.instance.ActivePoint / 100f;
                MainImage[(int)UIMainImageOrders.ActivePoint].fillAmount = (float)Character.instance.ActivePoint / 100f;
                break;
            case UIMainImageOrders.Job:
                //MainImage[(int)UIMainImageOrders.Job].sprite = "현 직업에 맞는 이미지 삽입";
                break;
            // TodoProgress
            case UIMainImageOrders.TodoProgress:
                //TodoProgressImage.fillAmount = (float)Character.instance.TodoProgress / 100f;
                MainImage[(int)UIMainImageOrders.TodoProgress].fillAmount = (float)Character.instance.Reputation / 100f;
                break;    
        }
    }
    private void SkipDay()
    {
        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -Character.instance.ActivePoint);
    }

    private void LoadUI()
    {
        Debug.Log("LoadEvent - UIMainManager");
        InitializeMainUI();
    }

    private void InitializeMainUI()
    {
        foreach (int Order in Enum.GetValues(typeof(UIMainImageOrders)))
        {
            ChangeMainUI((UIMainImageOrders)Order);
        }

        //DayImage.fillAmount = 1f;
        MainImage[(int)UIMainImageOrders.Day].fillAmount = 1f;
    }
}
