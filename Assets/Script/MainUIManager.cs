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
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Timer()
    {
        while(GameManager.instance.GameStart)
        {
            DayImage.fillAmount = 1 - (GameManager.instance.PlayTime / GameManager.instance.TotalPlayTime);


            yield return Time.deltaTime;
        }
        
    }

    public void ChangeTodoProgressImage(int TodoProgress)
    {
        TodoProgressImage.fillAmount = (float)TodoProgress;
    }
}
