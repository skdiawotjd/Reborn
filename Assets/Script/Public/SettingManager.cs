using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    private Button SaveButton;
    [SerializeField]
    private Image LoadPanel;
    private bool IsCreate;

    void Awake()
    {
        IsCreate = false;
    }

    void Start()
    {
        SaveButton.onClick.AddListener(GameManager.instance.LoadGame);
    }

    void Update()
    {
        
    }

    public void SaveData()
    {
        /*string playerToJson = JsonUtility.ToJson(GameObject.Find("PlayerCharacter").gameObject);
        Debug.Log(playerToJson);*/
        
        string Json = "{\"PlayerName\":\"Player1\"}";
        string fileName = "SaveData";
        string path = Application.dataPath + "/Resources/Public/" + fileName + ".Json";
        Debug.Log(JsonConvert.SerializeObject(GameObject.Find("PlayerCharacter").GetComponent<PlayerController>()));
        File.WriteAllText(path, Json);
    }

    public void SetActiveLoadPanel()
    {
        if (!IsCreate)
        {
            CreateLoadButton();
        }

        LoadPanel.gameObject.SetActive(!LoadPanel.gameObject.activeSelf);
    }

    private void CreateLoadButton()
    {
        IsCreate = true;
        for (int i = 0; i < 2; i++)
        {
            GameObject NewLoadButton = Instantiate(Resources.Load("Public/LoadButton")) as GameObject;
            NewLoadButton.transform.SetParent(LoadPanel.transform, false);
            NewLoadButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            NewLoadButton.GetComponent<RectTransform>().anchoredPosition = new Vector2((NewLoadButton.GetComponent<RectTransform>().anchoredPosition.x + (200 * i)), NewLoadButton.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
}
