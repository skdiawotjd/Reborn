using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
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

    void Awake()
    {
        /*SettingButton = transform.GetChild(0).GetComponent<Button>();
        SkipDayButton = transform.GetChild(1).GetComponent<Button>();
        DayImage = transform.GetChild(3).GetComponent<Image>();
        JobImage = transform.GetChild(4).GetComponent<Image>();
        TodoProgressImage = transform.GetChild(6).GetComponent<Image>();*/

        GameManager.instance.DayStart.AddListener(StartUI);
    }

    // Start is called before the first frame update
    void Start()
    {
        Character.instance.EventUIChange.AddListener(ChangeMainUI);
        GameManager.instance.DayEnd.AddListener(EndUI);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Timer()
    {
        while (GameManager.instance.IsDayStart)
        {
            DayImage.fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);


            yield return Time.deltaTime;
        }
    }

    private void ChangeMainUI(int Type)
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
            // MyPositon
            case 6:
                break;
            // ActivePoint
            case 7:
                ActivePointImage.fillAmount = (float)Character.instance.ActivePoint / 100f;
                break;
        }
    }

    public void SetZeroActivePoint()
    {
        Character.instance.SetCharacterStat(7, -Character.instance.ActivePoint);
    }

    private void EndUI()
    {
        SettingButton.interactable = false;
        SkipDayButton.interactable = false;

        //ChangeMainUI(4);

        StopCoroutine(Timer());

        DayImage.fillAmount = 1f;
    }

    private void StartUI()
    {
        SettingButton.interactable = true;
        SkipDayButton.interactable = true;

        ChangeMainUI(4);
        ChangeMainUI(7);

        StartCoroutine(Timer());
    }
}
