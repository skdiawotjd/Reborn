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
        GameManager.instance.AddDayStart(InitializeInventory);
        //Character.instance.EventUIChange.AddListener(UpdateInventoryStat);
        Character.instance.UIChangeAddListener(UpdateInventoryStat);
        //GameManager.instance.LoadEvent.AddListener(LoadInventory);

        CharacterStatArray = new TextMeshProUGUI[InfoDataGroup.transform.childCount];
        for(int i = 0; i < CharacterStatArray.Length; i++)
        {
            CharacterStatArray[i] = InfoDataGroup.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void UpdateInventoryStat(CharacterStatType Type)
    {
        switch (Type)
        {
            // MyName
            case 0:
                CharacterStatArray[0].text = Character.instance.MyName;
                break;
            // MySocialClass
            case CharacterStatType.MySocialClass:
                CharacterStatArray[1].text = Character.instance.MySocialClass.ToString();
                break;
            // MyJob
            case CharacterStatType.MyJob:
                CharacterStatArray[2].text = Character.instance.MyJob.ToString();
                break;
            // MyAge
            case CharacterStatType.MyAge:
                CharacterStatArray[3].text = Character.instance.MyAge.ToString();
                break;
            // TodoProgress
            case CharacterStatType.TodoProgress:
                CharacterStatArray[4].text = Character.instance.TodoProgress.ToString();
                break;
            case CharacterStatType.MyItem:
                // 인벤토리에 아이템이 실제 아이템보다 적을 때
                if (Character.instance.MyItem.Count > ItemContent.transform.childCount)
                {
                    int PreOrder = 0;
                    int ProOrder = 0;

                    if (ItemContent.transform.childCount == 0)
                    {
                        for(; PreOrder < Character.instance.MyItem.Count; PreOrder++)
                        {
                            AddItemButton(Character.instance.MyItem[PreOrder], PreOrder);
                        }
                    }
                    else
                    {
                        while (ProOrder < ItemContent.transform.childCount)
                        {
                            //Debug.Log("@@ ProOrder : " + ProOrder + " < ItemContent.transform.childCount : " + ItemContent.transform.childCount + "일 때");
                            //Debug.Log("ItemContent.transform.GetChild(ProOrder " + ProOrder + ").name : " + ItemContent.transform.GetChild(ProOrder).name
                                //+ " Character.instance.MyItem[PreOrder " + PreOrder + "].ToString()" + Character.instance.MyItem[PreOrder].ToString() + "???");
                            if (ItemContent.transform.GetChild(ProOrder).name != Character.instance.MyItem[PreOrder].ToString())
                            {
                                //Debug.Log("다르면 PreOrder가 " + PreOrder + "인 아이템 생성");
                                AddItemButton(Character.instance.MyItem[PreOrder], PreOrder);
                                ProOrder++;
                                //Debug.Log("ProOrder : " + (ProOrder - 1) + "에서 " + ProOrder + "로 증가");
                            }
                            else
                            {
                                //Debug.Log("같으면");
                                //Debug.Log("Character.instance.MyItem.Count : " + Character.instance.MyItem.Count + " 와 ItemContent.transform.childCount " + ItemContent.transform.childCount + "가");
                                if ((ProOrder + 1) < ItemContent.transform.childCount || Character.instance.MyItem.Count == ItemContent.transform.childCount)
                                {
                                    //Debug.Log("다르면");
                                    ProOrder++;
                                    //Debug.Log("ProOrder : " + (ProOrder - 1) + "에서 " + ProOrder + "로 증가");
                                }
                            }
                            if ((PreOrder + 1) < Character.instance.MyItem.Count)
                            {
                                PreOrder++;
                            }
                            //Debug.Log("PreOrder : " + (PreOrder - 1) + "에서 " + PreOrder + "로 증가");
                        }
                    }
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
                    for (; DeleteCount < Character.instance.MyItem.Count; DeleteCount++)
                    {
                        if (Character.instance.MyItem[DeleteCount] != ItemContent.transform.GetChild(DeleteCount).name)
                        {
                            Destroy(ItemContent.transform.GetChild(DeleteCount).gameObject);
                            return;
                        }
                    }
                    Destroy(ItemContent.transform.GetChild(DeleteCount).gameObject);


                    int PreOrder = 0;
                    int ProOrder = 0;

                    if (Character.instance.MyItem.Count == 0)
                    {
                        for (; ProOrder < ItemContent.transform.childCount; ProOrder++)
                        {
                            Destroy(ItemContent.transform.GetChild(ProOrder).gameObject);
                        }
                    }
                    else
                    {
                        while (PreOrder < Character.instance.MyItem.Count)
                        {
                            //Debug.Log("@@ ProOrder : " + ProOrder + " < ItemContent.transform.childCount : " + ItemContent.transform.childCount + "일 때");
                            //Debug.Log("ItemContent.transform.GetChild(ProOrder " + ProOrder + ").name : " + ItemContent.transform.GetChild(ProOrder).name
                            //+ " Character.instance.MyItem[PreOrder " + PreOrder + "].ToString()" + Character.instance.MyItem[PreOrder].ToString() + "???");
                            if (Character.instance.MyItem[PreOrder].ToString() != ItemContent.transform.GetChild(ProOrder).name)
                            {
                                //Debug.Log("다르면 PreOrder가 " + PreOrder + "인 아이템 생성");
                                Destroy(ItemContent.transform.GetChild(ProOrder).gameObject);
                                PreOrder++;
                                //Debug.Log("ProOrder : " + (ProOrder - 1) + "에서 " + ProOrder + "로 증가");
                            }
                            else
                            {
                                //Debug.Log("같으면");
                                //Debug.Log("Character.instance.MyItem.Count : " + Character.instance.MyItem.Count + " 와 ItemContent.transform.childCount " + ItemContent.transform.childCount + "가");
                                //if ((ProOrder + 1) < ItemContent.transform.childCount || Character.instance.MyItem.Count == ItemContent.transform.childCount)
                                if ((PreOrder + 1) < Character.instance.MyItem.Count || Character.instance.MyItem.Count == ItemContent.transform.childCount)
                                {
                                    //Debug.Log("다르면");
                                    PreOrder++;
                                    //Debug.Log("ProOrder : " + (ProOrder - 1) + "에서 " + ProOrder + "로 증가");
                                }
                            }
                            if ((ProOrder + 1) < ItemContent.transform.childCount)
                            {
                                ProOrder++;
                            }
                            //Debug.Log("PreOrder : " + (PreOrder - 1) + "에서 " + PreOrder + "로 증가");
                        }
                    }
                }
                break;
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

    private void InitializeInventory()
    {
        /*for (int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateInventoryStat((CharacterStatType)i);
        }

        UpdateInventoryStat(CharacterStatType.MyItem);*/

        for (int i = 0; i < ItemContent.transform.childCount; i++)
        {
            Destroy(ItemContent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateInventoryStat((CharacterStatType)i);
        }

        StartCoroutine(CoroutineLoadInventory());
    }

    private void LoadInventory()
    {
        for (int i = 0; i < ItemContent.transform.childCount; i++)
        {
            Destroy(ItemContent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateInventoryStat((CharacterStatType)i);
        }

        StartCoroutine(CoroutineLoadInventory());
    }

    IEnumerator CoroutineLoadInventory()
    {
        yield return new WaitForEndOfFrame();

        UpdateInventoryStat((CharacterStatType)8);
    }
}
