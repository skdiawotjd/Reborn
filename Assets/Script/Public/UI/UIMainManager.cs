using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private TextMeshProUGUI[] MainName;
    [SerializeField]
    private TextMeshProUGUI[] MainData;

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
            MainImage[(int)UIMainImageOrder.Time].fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);

            yield return Time.deltaTime;
        }
    }

    public void ChangeMainUIType(CharacterStatType Type)
    {
        if(Enum.TryParse(Type.ToString(), out UIMainImageOrder TemEnum))
        {
            ChangeMainUI(TemEnum);
        }
    }

    //private void ChangeMainUI(UIMainImageOrder Type)
    private void ChangeMainUI<T>(T Type)
    {
        switch (Type)
        {
            // ActivePoint
            case UIMainImageOrder.ActivePoint:
                MainImage[(int)UIMainImageOrder.ActivePoint].fillAmount = (float)Character.instance.ActivePoint / 100f;
                break;
            /*// Qurter
            case UIMainImageOrders.Qurter:
                MainImage[(int)UIMainImageOrders.Qurter].fillAmount = 1f - (float)(GameManager.instance.Days - 1) / 9f;
                break;*/
            // Days
            case UIMainTextOrder.Days:
                MainData[(int)UIMainTextOrder.Days].text = GameManager.instance.Days.ToString();
                break;
            // Job
            case UIMainImageOrder.Job:
                //MainImage[(int)UIMainImageOrders.Job].sprite = "현 직업에 맞는 이미지 삽입";
                break;
            // Proficiency
            case UIMainImageOrder.Proficiency:
                MainImage[(int)UIMainImageOrder.Proficiency].fillAmount = (float)Character.instance.Proficiency / 100f;
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
        foreach (int Order in Enum.GetValues(typeof(UIMainImageOrder)))
        {
            ChangeMainUI((UIMainImageOrder)Order);
        }
        foreach (int Order in Enum.GetValues(typeof(UIMainTextOrder)))
        {
            ChangeMainUI((UIMainTextOrder)Order);
        }

        MainImage[(int)UIMainImageOrder.Time].fillAmount = 1f;
    }
}
