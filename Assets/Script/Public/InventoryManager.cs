using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InfoDataGroup;
    private TextMeshProUGUI[] CharacterStatArray;

    [SerializeField]
    private GameObject InfoStackGroup;
    [SerializeField]
    private GameObject ItemContent;


    void Start()
    {
        GameManager.instance.DayStart.AddListener(StartInventoryStat);
        Character.instance.EventUIChange.AddListener(UpdateInventoryStat);

        CharacterStatArray = new TextMeshProUGUI[InfoDataGroup.transform.childCount];
        for(int i = 0; i < CharacterStatArray.Length; i++)
        {
            CharacterStatArray[i] = InfoDataGroup.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        StartInventoryStat();
    }

    public void UpdateInventoryStat(int Type)
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
            case 4:
                CharacterStatArray[4].text = Character.instance.TodoProgress.ToString();
                break;
            case 8:
                // 인벤토리에 아이템이 실제 아이템보다 적을 때
                if(Character.instance.MyItem.Count > ItemContent.transform.childCount)
                {
                    int InsertOrder = 0;

                    for ( ; InsertOrder < ItemContent.transform.childCount; InsertOrder++)
                    {
                        if (!(ItemContent.transform.GetChild(InsertOrder).name == Character.instance.MyItem[InsertOrder].ToString()))
                        {
                            break;
                        }
                    }
                    AddItemButton(Character.instance.MyItem[InsertOrder], InsertOrder);
                }
                // 인벤토리에 아이템이 실제 아이템과 같을 때
                else if (Character.instance.MyItem.Count == ItemContent.transform.childCount)
                {
                    for (int i = 0; i < ItemContent.transform.childCount; i++)
                    {
                        if (ItemContent.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text != (Character.instance.MyItem[i] + " " + Character.instance.MyItemCount[i].ToString()))
                        {
                            ItemContent.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = Character.instance.MyItem[i] + " " + Character.instance.MyItemCount[i].ToString();
                        }
                    }
                }
                // 인벤토리에 아이템이 실제 아이템보다 많을 때
                else if (Character.instance.MyItem.Count < ItemContent.transform.childCount)
                {
                    int DeleteCount = 0;
                    for(; DeleteCount < Character.instance.MyItem.Count; DeleteCount++)
                    {
                        if (Character.instance.MyItem[DeleteCount] != ItemContent.transform.GetChild(DeleteCount).name)
                        {
                            Destroy(ItemContent.transform.GetChild(DeleteCount).gameObject);
                            return;
                        }
                    }
                    Destroy(ItemContent.transform.GetChild(DeleteCount).gameObject);
                }
                break;
        }
    }

    private void StartInventoryStat()
    {
        for (int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateInventoryStat(i);
        }
    }


    private void AddItemButton(string ItemType, int InsertOrder)
    {
        // 실제 아이템을 추가
        GameObject NewItemButton = Instantiate(Resources.Load("Public/ItemButton")) as GameObject;
        // 계층구조 위치 변경
        NewItemButton.transform.SetParent(ItemContent.transform);
        // 버튼이름 수정
        NewItemButton.name = ItemType;
        // 버튼 순서 변경
        NewItemButton.transform.SetSiblingIndex(InsertOrder);
        // 버튼 하위의 text 변경
        NewItemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemType + " " + Character.instance.MyItemCount[Character.instance.MyItem.IndexOf(ItemType)].ToString();
        //NewItemButton.GetComponent<Button>().onClick.AddListener();
    }
}
