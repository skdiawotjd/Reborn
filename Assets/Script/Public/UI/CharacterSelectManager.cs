using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CharacterSelectPanel;
    [SerializeField]
    private TextMeshProUGUI SelectText;
    [SerializeField]
    private GameObject ItemContent;

    private bool CanVisible;

    void Awake()
    {
        CanVisible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveCharacterSelectPanel()
    {
        if (CanVisible)
        {
            CharacterSelectPanel.SetActive(!CharacterSelectPanel.activeSelf);
        }
        if(!CharacterSelectPanel.activeSelf)
        {
            CanVisible = false;
        }
    }

    public void SetCharacterSelectButton(Job StartJob)
    {
        CanVisible = true;
        int ButtonCount = 0;

        switch ((int)StartJob)
        {
            case 1:
            case 2:
                ButtonCount = 3;
                break;
            case 3:
            case 4:
                ButtonCount = 5;
                break;
            case 5:
            case 6:
                ButtonCount = 7;
                break;
            case 7:
            case 8:
                ButtonCount = 9;
                break;
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
                ButtonCount = (int)StartJob;
                break;
        }

        if (ItemContent.transform.childCount != ButtonCount)
        {
            //Debug.Log((Job)ButtonCount + "까지 생성");
            for (int i = 0; i < ButtonCount; i++)
            {
                GameObject NewSelectButton = Instantiate(Resources.Load("UI/SelectButton")) as GameObject;
                NewSelectButton.transform.SetParent(ItemContent.transform, false);
                //NewSelectButton.transform.GetChild(0).GetComponent<Image>()
                NewSelectButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((Job)i).ToString();
                int CurButtonCount = i;
                NewSelectButton.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.GameStart((Job)CurButtonCount); });
                NewSelectButton.GetComponent<Button>().onClick.AddListener(() => { CharacterSelectPanel.SetActive(!CharacterSelectPanel.activeSelf); });
            }
        }
    }
}
