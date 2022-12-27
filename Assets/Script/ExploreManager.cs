using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    private string m_Message;
    private string m_Message2;
    private float m_Speed = 0.2f;
    private int exploreCount = 0;
    private int maxExplore = 5;

    private bool waitActive;
    public MessageManager messageManager;
    public FloatingOptionManager floatingOptionManager;
    [SerializeField]
    private GameObject randomObject;
    private float randomObjectNumber;
    private int ObjectNumber;
    
    List<Dictionary<string, object>> itemList;
    private GameObject temObject;
    void Start()
    {
        messageManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(5).GetComponent<MessageManager>();
        floatingOptionManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(6).GetComponent<FloatingOptionManager>();
        itemList = CSVReader.Read("FoundThings");
        messageManager.setMessagePanel(true);
        waitActive = true;
        m_Message = "�ѹ�... �ѹ�...";
        m_Message2 = "���� �߰��ߴ�!";
        Character.instance.SetCharacterInput(false, false);
        Character.instance.MyPlayerController.SetRunState(true);
        Character.instance.SetCharacterPosition();
        StartCoroutine("CountOneSecond", 0.1f);
        StartCoroutine("CountNNSecond", 10f);
    }


    void Update()
    {

    }

    private void ChangeMessage(string message) // �޼���â�� �޼����� �ٲ��ش�
    {
        StartCoroutine(Typing(messageManager.ContentsText, message, m_Speed));
    }
    IEnumerator CountOneSecond(float delayTime) // 1�ʿ� �ѹ� ChangeMessage()�� ��������ش�
    {
        yield return new WaitForSeconds(delayTime);
        if (waitActive)
        {
            ChangeMessage(m_Message);
            StartCoroutine("CountOneSecond", 4f);
        }

    }

    private void GenerateObject() // �߰ߵ� ���𰡸� �������ش�.
    {
        randomObjectNumber = Random.Range(0.001f, 1f);
        
        temObject = Instantiate(randomObject, new Vector3(5, -2, transform.position.z), Quaternion.identity) as GameObject;
        temObject.name = WhatIsFoundThings(randomObjectNumber);
    }

    private string WhatIsFoundThings(float randomNumber)
    {
        for(int i=0; i < itemList.Count; i++)
        {
            if (randomObjectNumber <= float.Parse(itemList[i]["Rate"].ToString()))
            {
                ObjectNumber = i;
                return itemList[i]["Name"].ToString();
            }
        }
        return "";
    }

    private void FloatingChoiceMessage() // �߰��� ���𰡿� ���� �ൿ �������� ����ش�.
    {
        switch (itemList[ObjectNumber]["Number"].ToString())
        {
            case "0":
            case "1":
            case "2":
            case "8":
                floatingOptionManager.GenerateOptionButton(true, ExploreAction);
                break;
            case "6":
                floatingOptionManager.GenerateOptionButton(false, ExploreAction);
                break;

        }
        floatingOptionManager.optionPanel.gameObject.SetActive(true);

    }
    public void ExploreAction(int actionNumber)
    {
        switch(actionNumber)
        {
            case 0:
                Debug.Log("���¹ڽ�");
                ChangeMessage(itemList[ObjectNumber]["Name"].ToString() + "�� ȹ���ߴ�!");

                // ȹ���� ��ȭ ǰ�� �ִ� ���
                exploreCount++;
                if(exploreCount != maxExplore)
                {
                    StartCoroutine(CountGoToNext(5f));
                } else
                {
                    ExploreAction(2);
                }
                // ���ڸ� ����� �������� ����� �� ���
                break;
            case 1:
                Debug.Log("����");
                exploreCount++;
                if (exploreCount != maxExplore)
                {
                    Destroy(temObject);
                    Character.instance.MyPlayerController.SetRunState(true);
                    QuestManager.instance.ChangeMoveBG(true);
                    StopCoroutine("Typing");
                    StartCoroutine("CountOneSecond", 0.1f);
                    StartCoroutine("CountNNSecond", 10f);
                } else
                {
                    ExploreAction(2);
                }
                break;
            // �����ϰ� �������� �������� ����� �� ���
            case 2:
                Debug.Log("����");
                messageManager.setMessagePanel(false);
                Character.instance.SetCharacterInput(true, true);
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0201"); // ���������� Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                // �������� �������� ����� �� ���
                break;
        }
        floatingOptionManager.optionPanel.gameObject.SetActive(false);
    }

    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }
    IEnumerator CountNNSecond(float delayTime) // N�ʿ� �� ��, ���缭�� ���𰡸� �߰��Ѵ�.
    {
        yield return new WaitForSeconds(delayTime);
        QuestManager.instance.ChangeMoveBG(false);
        // ����� �������� �����
        StopCoroutine("CountOneSecond");
        // 1�ʿ� �ѹ� ����Ǵ� �ڵ带 ��ž
        ChangeMessage(m_Message2);
        // ���� �߰��ߴٴ� �޼��� ����
        Character.instance.MyPlayerController.SetRunState(false);
        //StartCoroutine("CountOneSecond", 10f);
        GenerateObject();
        FloatingChoiceMessage();;
    }
    IEnumerator CountGoToNext(float delayTime) // N�� �ڿ� �������� �����Ѵ�.
    {
        yield return new WaitForSeconds(delayTime);
        ExploreAction(1);
    }


    /*    1�ʿ� �� ��, �ѹ� �ѹ� �Ȱ� �ִٴ� �޽����� ȭ�鿡 ����ش�.
     *    N�ʿ� �� ��, ���缭�� ���𰡸� �߰��Ѵ�
     *    �߰��ϴ� ���𰡴� - ex) ����, ����, ����, ��ȭ, ����, ��[������ or ����]
     *    �߰��� ���𰡿� ���� ��� �ൿ�� �� �������� �ش�.
     *    �������� ���� �ൿ�Ѵ�.*/



}
