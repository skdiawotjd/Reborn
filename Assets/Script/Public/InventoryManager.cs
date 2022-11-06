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
        GameManager.instance.DayStart.AddListener(InitializeInventory);
        //Character.instance.EventUIChange.AddListener(UpdateInventoryStat);
        Character.instance.UIChangeAddListener(UpdateInventoryStat);
        GameManager.instance.LoadEvent.AddListener(LoadInventory);

        CharacterStatArray = new TextMeshProUGUI[InfoDataGroup.transform.childCount];
        for(int i = 0; i < CharacterStatArray.Length; i++)
        {
            CharacterStatArray[i] = InfoDataGroup.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void UpdateInventoryStat(int Type)
    {
        switch (Type)
        {
            // MyName
            case 0:
                CharacterStatArray[0].text = Character.instance.MyName;
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
                // �κ��丮�� �������� ���� �����ۺ��� ���� ��
                if (Character.instance.MyItem.Count > ItemContent.transform.childCount)
                {

                    /*
                    int InsertOrder = 0;

                    if(ItemContent.transform.childCount == 0)
                    {
                        for(; InsertOrder < Character.instance.MyItem.Count; InsertOrder++)
                        {
                            AddItemButton(Character.instance.MyItem[InsertOrder], InsertOrder);
                        }
                    }
                    else
                    {
                        for (; InsertOrder < ItemContent.transform.childCount; InsertOrder++)
                        {
                            if (!(ItemContent.transform.GetChild(InsertOrder).name == Character.instance.MyItem[InsertOrder].ToString()))
                            {
                                break;
                            }
                        }
                        AddItemButton(Character.instance.MyItem[InsertOrder], InsertOrder);
                    }*/




                    /*for(; InsertOrder < (Character.instance.MyItem.Count - ItemContent.transform.childCount); InsertOrder++)
                    {
                        if(ItemContent.transform.childCount == 0)
                        {
                            AddItemButton(Character.instance.MyItem[InsertOrder], InsertOrder);
                        }
                        else
                        {
                            if(Character.instance.MyItem[InsertOrder].ToString() != ItemContent.transform.GetChild(ProOrder).name)
                            {
                                AddItemButton(Character.instance.MyItem[InsertOrder], InsertOrder);
                            }
                            else
                            {
                                ProOrder++;
                            }
                        }
                    }*/


/*                    int PreOrder = 0;
                    int ProOrder = 0;

                    for (; PreOrder < (Character.instance.MyItem.Count - ItemContent.transform.childCount); )
                    {
                        if (ItemContent.transform.childCount == 0)
                        {
                            AddItemButton(Character.instance.MyItem[PreOrder], PreOrder);
                            PreOrder++;

                            continue;
                        }
                        else
                        {
                            for(; ProOrder < ItemContent.transform.childCount; )
                            { 
                                if(ItemContent.transform.GetChild(ProOrder).name != Character.instance.MyItem[PreOrder].ToString())
                                {
                                    AddItemButton(Character.instance.MyItem[PreOrder], PreOrder);
                                }
                                else
                                {
                                    ProOrder++;
                                }
                                PreOrder++;
                            }
                        }
                        AddItemButton(Character.instance.MyItem[PreOrder], PreOrder);
                        PreOrder++;
                    }*/
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
                        //for (; ProOrder < ItemContent.transform.childCount;)
                        while (ProOrder < ItemContent.transform.childCount)
                        {
                            //Debug.Log("@@ ProOrder : " + ProOrder + " < ItemContent.transform.childCount : " + ItemContent.transform.childCount + "�� ��");
                            //Debug.Log("ItemContent.transform.GetChild(ProOrder " + ProOrder + ").name : " + ItemContent.transform.GetChild(ProOrder).name
                                //+ " Character.instance.MyItem[PreOrder " + PreOrder + "].ToString()" + Character.instance.MyItem[PreOrder].ToString() + "???");
                            if (ItemContent.transform.GetChild(ProOrder).name != Character.instance.MyItem[PreOrder].ToString())
                            {
                                //Debug.Log("�ٸ��� PreOrder�� " + PreOrder + "�� ������ ����");
                                AddItemButton(Character.instance.MyItem[PreOrder], PreOrder);
                                ProOrder++;
                                //Debug.Log("ProOrder : " + (ProOrder - 1) + "���� " + ProOrder + "�� ����");
                            }
                            else
                            {
                                //Debug.Log("������");
                                //Debug.Log("Character.instance.MyItem.Count : " + Character.instance.MyItem.Count + " �� ItemContent.transform.childCount " + ItemContent.transform.childCount + "��");
                                if ((ProOrder + 1) < ItemContent.transform.childCount || Character.instance.MyItem.Count == ItemContent.transform.childCount)
                                {
                                    //Debug.Log("�ٸ���");
                                    ProOrder++;
                                    //Debug.Log("ProOrder : " + (ProOrder - 1) + "���� " + ProOrder + "�� ����");
                                }
                            }
                            if ((PreOrder + 1) < Character.instance.MyItem.Count)
                            {
                                PreOrder++;
                            }
                            //Debug.Log("PreOrder : " + (PreOrder - 1) + "���� " + PreOrder + "�� ����");
                        }
                    }


                }
                // �κ��丮�� �������� ���� �����۰� ���� ��
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
                // �κ��丮�� �������� ���� �����ۺ��� ���� ��
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
                }
                break;
        }
    }

    private void AddItemButton(string ItemType, int InsertOrder)
    {
        // ���� �������� �߰�
        GameObject NewItemButton = Instantiate(Resources.Load("Public/ItemButton")) as GameObject;
        // �������� ��ġ ����
        NewItemButton.transform.SetParent(ItemContent.transform);
        // ��ư�̸� ����
        NewItemButton.name = ItemType;
        // ��ư ���� ����
        NewItemButton.transform.SetSiblingIndex(InsertOrder);
        // ��ư ������ text ����
        NewItemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemType + " " + Character.instance.MyItemCount[Character.instance.MyItem.IndexOf(ItemType)].ToString();
        //NewItemButton.GetComponent<Button>().onClick.AddListener();
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateInventoryStat(i);
        }

        UpdateInventoryStat(8);
    }

    private void LoadInventory()
    {
        for (int i = 0; i < ItemContent.transform.childCount; i++)
        {
            Destroy(ItemContent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < CharacterStatArray.Length; i++)
        {
            UpdateInventoryStat(i);
        }

        StartCoroutine(CoroutineLoadInventory());
    }

    IEnumerator CoroutineLoadInventory()
    {
        yield return new WaitForEndOfFrame();

        UpdateInventoryStat(8);
    }
}
