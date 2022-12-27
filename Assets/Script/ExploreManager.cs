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
        m_Message = "뚜벅... 뚜벅...";
        m_Message2 = "무언갈 발견했다!";
        Character.instance.SetCharacterInput(false, false);
        Character.instance.MyPlayerController.SetRunState(true);
        Character.instance.SetCharacterPosition();
        StartCoroutine("CountOneSecond", 0.1f);
        StartCoroutine("CountNNSecond", 10f);
    }


    void Update()
    {

    }

    private void ChangeMessage(string message) // 메세지창의 메세지를 바꿔준다
    {
        StartCoroutine(Typing(messageManager.ContentsText, message, m_Speed));
    }
    IEnumerator CountOneSecond(float delayTime) // 1초에 한번 ChangeMessage()를 실행시켜준다
    {
        yield return new WaitForSeconds(delayTime);
        if (waitActive)
        {
            ChangeMessage(m_Message);
            StartCoroutine("CountOneSecond", 4f);
        }

    }

    private void GenerateObject() // 발견된 무언가를 생성해준다.
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

    private void FloatingChoiceMessage() // 발견한 무언가에 대해 행동 선택지를 띄워준다.
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
                Debug.Log("오픈박스");
                ChangeMessage(itemList[ObjectNumber]["Name"].ToString() + "를 획득했다!");

                // 획득한 재화 품에 넣는 기능
                exploreCount++;
                if(exploreCount != maxExplore)
                {
                    StartCoroutine(CountGoToNext(5f));
                } else
                {
                    ExploreAction(2);
                }
                // 상자를 열어본다 선택지를 골랐을 시 기능
                break;
            case 1:
                Debug.Log("무시");
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
            // 무시하고 지나간다 선택지를 골랐을 시 기능
            case 2:
                Debug.Log("도망");
                messageManager.setMessagePanel(false);
                Character.instance.SetCharacterInput(true, true);
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0201"); // 대장장이의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                // 도망간다 선택지를 골랐을 시 기능
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
    IEnumerator CountNNSecond(float delayTime) // N초에 한 번, 멈춰서서 무언가를 발견한다.
    {
        yield return new WaitForSeconds(delayTime);
        QuestManager.instance.ChangeMoveBG(false);
        // 배경의 움직임을 멈춘다
        StopCoroutine("CountOneSecond");
        // 1초에 한번 실행되는 코드를 스탑
        ChangeMessage(m_Message2);
        // 무언갈 발견했다는 메세지 송출
        Character.instance.MyPlayerController.SetRunState(false);
        //StartCoroutine("CountOneSecond", 10f);
        GenerateObject();
        FloatingChoiceMessage();;
    }
    IEnumerator CountGoToNext(float delayTime) // N초 뒤에 다음으로 진행한다.
    {
        yield return new WaitForSeconds(delayTime);
        ExploreAction(1);
    }


    /*    1초에 한 번, 뚜벅 뚜벅 걷고 있다는 메시지를 화면에 띄워준다.
     *    N초에 한 번, 멈춰서서 무언가를 발견한다
     *    발견하는 무언가는 - ex) 상자, 광물, 몬스터, 재화, 잡템, 문[보물방 or 함정]
     *    발견한 무언가에 대해 어떻게 행동할 지 선택지를 준다.
     *    선택지에 따라 행동한다.*/



}
