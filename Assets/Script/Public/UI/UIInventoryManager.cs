using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class UIInventoryManager : UIManager
{
    [SerializeField]
    private TextMeshProUGUI[] InfoName;
    [SerializeField]
    private TextMeshProUGUI[] InfoData;
    [SerializeField]
    private TextMeshProUGUI[] StackName;
    [SerializeField]
    private TextMeshProUGUI[] StackData;
    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private GameObject ItemContent;
    [SerializeField]
    private GameObject ItemPrefab;

    private int[] StackOrder;

    void Awake()
    {
        StackOrder = new int[2];
    }

    protected override void Start()
    {
        base.Start();
        Character.instance.UIChangeAddListener(UpdateInventoryStat);
        GameManager.instance.AddLoadEvent(LoadInventory);
        GameManager.instance.AddGameStartEvent(SetStack);
    }

    protected override void StartUI()
    {
        // UI ������ �ʱ�ȭ
        foreach (UIInventoryOrder Order in Enum.GetValues(typeof(UIInventoryOrder)))
        {
            try
            {
                /*foreach (int Order in Enum.GetValues(typeof(UIInventoryOrder)))
                {
                    UpdateInventoryStat((CharacterStatType)Order);
                }*/
                UpdateInventoryStat((CharacterStatType)Enum.Parse(typeof(CharacterStatType), Order.ToString()));
                //Debug.Log((CharacterStatType)Enum.Parse(typeof(CharacterStatType), Order.ToString()));
            }
            catch
            {
                UpdateInventoryStat((CharacterStatType)Order);
                //Debug.Log((CharacterStatType)Order);
            }
        }

    }

    protected override void EndUI()
    {
        //Debug.Log("UIInventory ���� �� �ؾ��� �� ����");
    }

    private void SetStack()
    {
        switch (Character.instance.MySocialClass)
        {
            case SocialClass.Helot:
                StackOrder[0] = 0;
                StackOrder[1] = 1;
                StackName[0].text = "Smith";
                StackName[1].text = "Bania";
                break;
            case SocialClass.Commons:
                StackOrder[0] = 1;
                StackOrder[1] = 2;
                StackName[0].text = "Knight";
                StackName[1].text = "Alchemist";
                break;
            case SocialClass.SemiNoble:
                StackOrder[0] = 4;
                StackOrder[1] = 2;
                StackName[0].text = "Commons";
                StackName[1].text = "Noble";
                break;
            case SocialClass.Noble:
                StackOrder[0] = 4;
                StackOrder[1] = 2;
                StackName[0].text = "People";
                StackName[1].text = "King";
                break;
            case SocialClass.King:
                StackOrder[0] = 3;
                StackOrder[1] = 4;
                StackName[0].text = "People";
                StackName[1].text = "Noble";
                break;
        }
    }

    public void UpdateInventoryStat(CharacterStatType Type)
    {
        //Debug.Log(Type);
        switch (Type)
        {
            // MyName
            case 0:
                InfoData[0].text = Character.instance.MyName;
                break;
            // MySocialClass
            case CharacterStatType.MySocialClass:
                InfoData[1].text = Character.instance.MySocialClass.ToString();
                break;
            // MyJob
            case CharacterStatType.MyJob:
                InfoData[2].text = Character.instance.MyJob.ToString();
                break;
            /*// MyAge
            case CharacterStatType.MyAge:
                InfoData[3].text = Character.instance.MyAge.ToString();
                break;*/
            // MyAge
            case CharacterStatType.ActivePoint:
                InfoData[3].text = Character.instance.ActivePoint.ToString();
                break;
            // Reputation
            case CharacterStatType.Reputation:
                InfoData[4].text = Character.instance.Reputation.ToString();
                break;
            // Proficiency
            case CharacterStatType.Proficiency:
                InfoData[5].text = Character.instance.Proficiency.ToString();
                break;
            case CharacterStatType.MyItem:
                // �κ��丮�� �������� ���� �����ۺ��� ���� ��
                //if (Character.instance.MyItem.Count > ItemContent.transform.childCount + (Character.instance.MyItem.Count - Character.instance.QuestItemOrder))
                if (Character.instance.QuestItemOrder > ItemContent.transform.childCount)
                {
                    int PreOrder = 0;
                    int ProOrder = 0;

                    if (ItemContent.transform.childCount == 0)
                    {
                        AddItem(Character.instance.MyItem[PreOrder], PreOrder);
                        /*for (; PreOrder < Character.instance.MyItem.Count; PreOrder++)
                        {
                            AddItem(Character.instance.MyItem[PreOrder], PreOrder);
                        }*/
                    }
                    else
                    {
                        //while (ProOrder < ItemContent.transform.childCount)
                        while (ItemContent.transform.childCount < Character.instance.QuestItemOrder)
                        {
                            //Debug.Log("@@ ProOrder : " + ProOrder + " < ItemContent.transform.childCount : " + ItemContent.transform.childCount + "�� ��");
                            //Debug.Log("ItemContent.transform.GetChild(ProOrder " + ProOrder + ").name : " + ItemContent.transform.GetChild(ProOrder).name
                            //+ " Character.instance.MyItem[PreOrder " + PreOrder + "].ToString()" + Character.instance.MyItem[PreOrder].ToString() + "???");
                            if (ItemContent.transform.GetChild(ProOrder).name != Character.instance.MyItem[PreOrder].ToString())
                            {
                                //Debug.Log("�ٸ��� PreOrder�� " + PreOrder + "�� ������ ����");
                                AddItem(Character.instance.MyItem[PreOrder], PreOrder);
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
                //else if (Character.instance.MyItem.Count == ItemContent.transform.childCount)
                else if (ItemContent.transform.childCount == Character.instance.QuestItemOrder)
                {
                    Item TemItem;
                    for (int i = 0; i < ItemContent.transform.childCount; i++)
                    {
                        TemItem = ItemContent.transform.GetChild(i).GetComponent<Item>();
                        if (TemItem.ItemCount != Character.instance.MyItemCount[i])
                        {
                            TemItem.SetCount(Character.instance.MyItemCount[i]);
                        }
                        /*if (ItemContent.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text != (Character.instance.MyItem[i] + " " + Character.instance.MyItemCount[i].ToString()))
                        {
                            ItemContent.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = Character.instance.MyItem[i] + " " + Character.instance.MyItemCount[i].ToString();
                        }*/
                    }
                }
                // �κ��丮�� �������� ���� �����ۺ��� ���� ��
                else if (Character.instance.QuestItemOrder < ItemContent.transform.childCount)
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
                            //Debug.Log("@@ ProOrder : " + ProOrder + " < ItemContent.transform.childCount : " + ItemContent.transform.childCount + "�� ��");
                            //Debug.Log("ItemContent.transform.GetChild(ProOrder " + ProOrder + ").name : " + ItemContent.transform.GetChild(ProOrder).name
                            //+ " Character.instance.MyItem[PreOrder " + PreOrder + "].ToString()" + Character.instance.MyItem[PreOrder].ToString() + "???");
                            if (Character.instance.MyItem[PreOrder].ToString() != ItemContent.transform.GetChild(ProOrder).name)
                            {
                                //Debug.Log("�ٸ��� PreOrder�� " + PreOrder + "�� ������ ����");
                                Destroy(ItemContent.transform.GetChild(ProOrder).gameObject);
                                PreOrder++;
                                //Debug.Log("ProOrder : " + (ProOrder - 1) + "���� " + ProOrder + "�� ����");
                            }
                            else
                            {
                                //Debug.Log("������");
                                //Debug.Log("Character.instance.MyItem.Count : " + Character.instance.MyItem.Count + " �� ItemContent.transform.childCount " + ItemContent.transform.childCount + "��");
                                //if ((ProOrder + 1) < ItemContent.transform.childCount || Character.instance.MyItem.Count == ItemContent.transform.childCount)
                                if ((PreOrder + 1) < Character.instance.MyItem.Count || Character.instance.MyItem.Count == ItemContent.transform.childCount)
                                {
                                    //Debug.Log("�ٸ���");
                                    PreOrder++;
                                    //Debug.Log("ProOrder : " + (ProOrder - 1) + "���� " + ProOrder + "�� ����");
                                }
                            }
                            if ((ProOrder + 1) < ItemContent.transform.childCount)
                            {
                                ProOrder++;
                            }
                            //Debug.Log("PreOrder : " + (PreOrder - 1) + "���� " + PreOrder + "�� ����");
                        }
                    }
                }
                break;
            case CharacterStatType InputType when (InputType >= CharacterStatType.Smith):
                if(Character.instance.StackOrder - (int)Character.instance.MySocialClass == StackOrder[0])
                {
                    StackData[0].text = Character.instance.MyStack[Character.instance.StackOrder].ToString();
                }
                else if(Character.instance.StackOrder - (int)Character.instance.MySocialClass == StackOrder[1])
                {
                    StackData[0].text = Character.instance.MyStack[Character.instance.StackOrder].ToString();
                }
                else
                {
                    Debug.Log("���� ������ ���� �ʴ� ������ ����");
                }
                break;

        }
    }

    private void AddItem(string ItemType, int InsertOrder)
    {
        // ���� �������� �߰�
        GameObject NewItem = Instantiate(ItemPrefab);
        // �������� ��ġ ����
        NewItem.transform.SetParent(ItemContent.transform, false);
        // ��ư�̸� ����
        NewItem.name = ItemType;
        // ��ư ���� ����
        NewItem.transform.SetSiblingIndex(InsertOrder);
        // ��ư ������ text ����
        NewItem.GetComponent<Item>().SetCount(Character.instance.MyItemCount[InsertOrder]);
        //NewItemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemType + " " + Character.instance.MyItemCount[Character.instance.MyItem.IndexOf(ItemType)].ToString();
        //NewItemButton.GetComponent<Button>().onClick.AddListener();
    }

    private void LoadInventory()
    {
        Debug.Log("LoadEvent - UIInventoryManager");
        for (int i = 0; i < ItemContent.transform.childCount; i++)
        {
            Destroy(ItemContent.transform.GetChild(i).gameObject);
        }

        StartUI();

        //StartCoroutine(CoroutineLoadInventory());
    }
}
