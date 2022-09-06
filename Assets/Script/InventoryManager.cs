using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    private GameObject InvenPanel;
    private GameObject DataGroup;
    private TextMeshProUGUI[] CharacterStatArray;

    // Start is called before the first frame update
    void Start()
    {
        InvenPanel = transform.GetChild(0).gameObject;
        DataGroup = InvenPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;

        CharacterStatArray = new TextMeshProUGUI[DataGroup.transform.childCount];
        for(int i = 0; i < CharacterStatArray.Length; i++)
        {
            CharacterStatArray[i] = DataGroup.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        for(int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateCharacterStat(i);
        }

        Character.instance.EventUIChange.AddListener(UpdateCharacterStat);
    }

    void Update()
    {

    }

    public void UpdateCharacterStat(int Type)
    {
        switch (Type)
        {
            // MyName
            case 0:
                CharacterStatArray[0].text = Character.instance.name;
                break;
            // MySocialClass
            case 1:
                CharacterStatArray[1].text = Character.instance.MySocialClass.ToString();
                break;
            // MyJob
            case 2:
                CharacterStatArray[2].text = Character.instance.MyJob.ToString();
                break;
            // MyAge
            case 3:
                CharacterStatArray[3].text = Character.instance.MyAge.ToString();
                break;
            // TodoProgress
            case 5:
                CharacterStatArray[4].text = Character.instance.TodoProgress.ToString();
                break;
        }
    }
}
