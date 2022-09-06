using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    private Image SettingImage;
    private Image DayImage;
    private Image JobImage;
    private Image TodoProgressImage;

    private void Awake()
    {
        SettingImage = transform.GetChild(0).GetComponent<Image>();
        DayImage = transform.GetChild(2).GetComponent<Image>();
        JobImage = transform.GetChild(3).GetComponent<Image>();
        TodoProgressImage = transform.GetChild(5).GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Character.instance.EventUIChange.AddListener(CheckChangeMainUI);

        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Timer()
    {
        while (GameManager.instance.GameStart)
        {
            DayImage.fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);


            yield return Time.deltaTime;
        }

    }

    private void CheckChangeMainUI(int Type)
    {
        switch(Type)
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
            // MyRound
            case 4:
                break;
            // TodoProgress
            case 5:
                TodoProgressImage.fillAmount = Character.instance.TodoProgress / 100f;
                break;
            // MyStackByJob
            case 6:
                break;
        }
    }
}
