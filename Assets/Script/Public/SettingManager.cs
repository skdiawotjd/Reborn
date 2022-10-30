using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    [SerializeField]
    private Button SaveButton;
    [SerializeField]
    private Image LoadPanel;

    private int CurSaveDataCount;

    void Awake()
    {
        CurSaveDataCount = 0;
    }

    void Start()
    {
        SaveButton.onClick.AddListener(GameManager.instance.SaveData);
    }

    void Update()
    {
        
    }

    public void SetActiveLoadPanel()
    {
        CheckLoadButton();

        LoadPanel.gameObject.SetActive(!LoadPanel.gameObject.activeSelf);
    }

    private void CheckLoadButton()
    {
        GameManager.instance.SetSaveDataCount();
        Debug.Log(".json이 " + GameManager.instance.SaveDataCount + "개 있음");
        // 실제 저장 데이터 수와 현재 저장데이터 수가 달라
        if (CurSaveDataCount != GameManager.instance.SaveDataCount)
        {
            // 버튼을 새로 생성
            for ( ; CurSaveDataCount < GameManager.instance.SaveDataCount; CurSaveDataCount++)
            {
                GameObject NewLoadButton = Instantiate(Resources.Load("Public/LoadButton")) as GameObject;
                NewLoadButton.transform.SetParent(LoadPanel.transform, false);
                int SaveDataButtonCount = CurSaveDataCount;
                NewLoadButton.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.LoadData(SaveDataButtonCount); });
                NewLoadButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CurSaveDataCount.ToString();
                NewLoadButton.GetComponent<RectTransform>().anchoredPosition = new Vector2((NewLoadButton.GetComponent<RectTransform>().anchoredPosition.x + (200 * CurSaveDataCount)), NewLoadButton.GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
    }
}