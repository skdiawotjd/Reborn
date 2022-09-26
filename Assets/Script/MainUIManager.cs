using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    private Button SettingButton;
    private Button SkipDayButton;
    private Image DayImage;
    private Image JobImage;
    private Image TodoProgressImage;

    void Awake()
    {
        SettingButton = transform.GetChild(0).GetComponent<Button>();
        SkipDayButton = transform.GetChild(1).GetComponent<Button>();
        DayImage = transform.GetChild(3).GetComponent<Image>();
        JobImage = transform.GetChild(4).GetComponent<Image>();
        TodoProgressImage = transform.GetChild(6).GetComponent<Image>();

        GameManager.instance.GameStart.AddListener(StartUI);
    }

    // Start is called before the first frame update
    void Start()
    {
        Character.instance.EventUIChange.AddListener(CheckChangeMainUI);
        GameManager.instance.GameEnd.AddListener(EndUI);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Timer()
    {
        while (GameManager.instance.DayStart)
        {
            DayImage.fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);


            yield return Time.deltaTime;
        }
    }

    private void CheckChangeMainUI(int Type)
    {
        switch (Type)
        {
            // MySocialClass
            case 1:
                break;
            // MyJob
            case 2:
                break;
            // MyAge
            case 3:
                break;
            // TodoProgress
            case 4:
                TodoProgressImage.fillAmount = (float)Character.instance.TodoProgress / 100f;
                break;
            // MyRound
            case 5:
                break;
            // MyStackByJob
            case 6:
                break;
        }
    }

    public void SetZeroActivePoint()
    {
        Character.instance.SetCharacterStat(7, 0);
    }

    private void EndUI()
    {
        SettingButton.interactable = false;
        SkipDayButton.interactable = false;

        CheckChangeMainUI(4);

        StopCoroutine(Timer());

        DayImage.fillAmount = 1f;
    }

    private void StartUI()
    {
        SettingButton.interactable = true;
        SkipDayButton.interactable = true;

        CheckChangeMainUI(4);

        StartCoroutine(Timer());
    }
}
