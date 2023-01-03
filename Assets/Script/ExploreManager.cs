using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    private string m_Message;
    private string m_Message2;
    private string m_Message3;
    private float m_Speed = 0.2f;
    private int exploreCount = 0;
    private int maxExplore = 60;

    private bool waitActive;
    public MessageManager messageManager;
    public FloatingOptionManager floatingOptionManager;
    private BattleManager battleManager;
    [SerializeField]
    private GameObject randomObject;
    private float randomObjectNumber;
    private float randomTemNumber;
    private int ObjectNumber;
    private int goodsNumber;
    List<Dictionary<string, object>> itemList;
    List<Dictionary<string, object>> boxGoodsList;
    List<Dictionary<string, object>> mineralGoodsList;
    private GameObject temObject;
    void Start()
    {
        messageManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(5).GetComponent<MessageManager>();
        floatingOptionManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(6).GetComponent<FloatingOptionManager>();
        itemList = CSVReader.Read("FoundThings");
        boxGoodsList = CSVReader.Read("TreasureBoxGoods");
        mineralGoodsList = CSVReader.Read("MineralGoods");
        messageManager.setMessagePanel(true);
        waitActive = true;
        m_Message = "뚜벅... 뚜벅...";
        m_Message2 = "무언갈 발견했다!";
        m_Message3 = "몬스터를 마주쳤다!";

        Character.instance.SetCharacterInput(false, false, false);
        Character.instance.MyPlayerController.SetRunState(true);
        Character.instance.MyPlayerController.PlayerRotation(Direction.Right);
        Character.instance.SetCharacterPosition();

        StartCoroutine("CountOneSecond", 0.1f);
        StartCoroutine("CountNNSecond", 3f);
    }


    void Update()
    {

    }

    public void ChangeMessage(string message) // 메세지창의 메세지를 바꿔준다
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
        randomObjectNumber = Random.Range(0.001f, 1.000f);
        if (WhatIsFoundThings(randomObjectNumber, 0) == "monster")
        {
            battleManager.GenerateMonster();
        } else
        {
            temObject = Instantiate(randomObject, new Vector3(5, -2, transform.position.z), Quaternion.identity) as GameObject;
            temObject.name = WhatIsFoundThings(randomObjectNumber, 0);
        }
    }

    private string WhatIsFoundThings(float randomNumber, int WhatIsThis)
    {
        switch(WhatIsThis)
        {
            case 0: // 발견한 무언가
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (randomObjectNumber <= float.Parse(itemList[i]["Rate"].ToString()))
                    {
                        ObjectNumber = i;
                        return itemList[i]["Name"].ToString();
                    }
                }
                return "";
            case 1: // 박스에서 나온 무언가
                if (randomNumber < 0.0003)
                {
                    randomTemNumber = 0.0001f;
                } else
                {
                    randomTemNumber = randomNumber / 0.3f;
                }
                for (int i = 0; i < boxGoodsList.Count; i++)
                {
                    if (randomObjectNumber <= float.Parse(boxGoodsList[i]["Rate"].ToString()))
                    {
                        goodsNumber = i;
                        return boxGoodsList[i]["ItemNumber"].ToString();
                    }
                }
                break;
            case 2: // 광물에서 나온 무언가
                if (randomNumber < 0.0003)
                {
                    randomTemNumber = 0.0001f;
                }
                else
                {
                    randomTemNumber = randomNumber / 0.3f;
                }
                for (int i = 0; i < mineralGoodsList.Count; i++)
                {
                    if (randomObjectNumber <= float.Parse(mineralGoodsList[i]["Rate"].ToString()))
                    {
                        goodsNumber = i;
                        return mineralGoodsList[i]["ItemNumber"].ToString();
                    }
                }
                break;
        }
        return null;
    }

    private void FloatingChoiceMessage() // 발견한 무언가에 대해 행동 선택지를 띄워준다.
    {
        switch (itemList[ObjectNumber]["Number"].ToString())
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "7":
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
                switch (itemList[ObjectNumber]["Number"].ToString())
                {
                    case "0":
                    case "1":
                    case "2":
                        Character.instance.SetCharacterStat(CharacterStatType.MyItem, WhatIsFoundThings(randomObjectNumber, 1) + ProbabilityOfHowMany());
                        ChangeMessage(boxGoodsList[goodsNumber]["Name"].ToString() + "를 획득했다!");
                        break;
                    case "3":
                    case "4":
                    case "5":
                        Character.instance.SetCharacterStat(CharacterStatType.MyItem, WhatIsFoundThings(randomObjectNumber, 2) + ProbabilityOfHowMany());
                        ChangeMessage(mineralGoodsList[goodsNumber]["Name"].ToString() + "를 획득했다!");
                        break;
                    case "7":
                        Character.instance.SetCharacterStat(CharacterStatType.Gold, 1000); // 재화 획득 임시
                        ChangeMessage("1000" + itemList[ObjectNumber]["Name"].ToString() + "를 획득했다!");
                        break;
                }
                
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
                    if(temObject != null)
                    {
                        Destroy(temObject);
                    } else if(battleManager.GetExistTemMonster())
                    {
                        battleManager.DestroyTemMonster();
                    }
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
            case 3: // 말 걸어본다 선택지를 골랐을 시 기능
                battleManager.StartBattle();
                StopCoroutine("Typing");
                ChangeMessage(m_Message3);
                // 전투
                break;
        }
        floatingOptionManager.optionPanel.gameObject.SetActive(false);
    }
    private string ProbabilityOfHowMany() // 나온 아이템이 몇 개일 확률
    {
        int percentageNumber;
        percentageNumber = Random.Range(1, 100);
        if(percentageNumber <= 60)
        {
            return "1";
        }else if(percentageNumber <= 90)
        {
            return "2";
        }else
        {
            return "3";
        }
    }
    public void GoToNext(float delayTime)
    {
        StartCoroutine(CountGoToNext(delayTime));
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

    public void SetBattleManager(BattleManager newBattleManager)
    {
        battleManager = newBattleManager;
    }

    /*    1초에 한 번, 뚜벅 뚜벅 걷고 있다는 메시지를 화면에 띄워준다.
     *    N초에 한 번, 멈춰서서 무언가를 발견한다
     *    발견하는 무언가는 - ex) 상자, 광물, 몬스터, 재화, 잡템, 문[보물방 or 함정]
     *    발견한 무언가에 대해 어떻게 행동할 지 선택지를 준다.
     *    선택지에 따라 행동한다.*/
}
