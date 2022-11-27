using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainManager : UIManager
{
    [SerializeField]
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
    private Image TodoProgressImage;
    [SerializeField]
    private List<Button> MainButton;
    [SerializeField]
    private List<Image> MainImage;

    protected override void Start()
    {
        base.Start();
        MainButton[(int)UIMainButtonOrder.Setting].onClick.AddListener(() => { transform.GetComponentInParent<PopUpUIManager>().VisibleUI(UIPopUpOrder.SettingPanel); });
        MainButton[(int)UIMainButtonOrder.SkipDay].onClick.AddListener(SkipDay);

        GameManager.instance.LoadEvent.AddListener(LoadUI);
        Character.instance.UIChangeAddListener(ChangeMainUI);
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
            DayImage.fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);


            yield return Time.deltaTime;
        }
    }

    private void ChangeMainUI(CharacterStatType Type)
    {
        switch (Type)
        {
            // TodoProgress
            case CharacterStatType.TodoProgress:
                TodoProgressImage.fillAmount = (float)Character.instance.TodoProgress / 100f;
                break;
            // ActivePoint
            case CharacterStatType.ActivePoint:
                ActivePointImage.fillAmount = (float)Character.instance.ActivePoint / 100f;
                break;
        }
    }
    private void SkipDay()
    {
        Character.instance.SetCharacterStat(CharacterStatType.ActivePoint, -Character.instance.ActivePoint);
    }

    private void LoadUI()
    {
        InitializeMainUI();
    }

    private void InitializeMainUI()
    {
        foreach (int Order in Enum.GetValues(typeof(UIMainImageOrder)))
        {
            ChangeMainUI((CharacterStatType)Order);
        }

        DayImage.fillAmount = 1f;
    }
}
