using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    private string m_Message;
    private string m_Message2;
    private string m_Message3;
    private float m_Speed = 0.05f;
    private int exploreCount = 0;
    private int maxExplore = 300;

    private ConversationManager conversationManager;
    private bool waitActive;
    public MessageManager messageManager;
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
    List<Dictionary<string, object>> exploreSelectFloatingMessage;
    private GameObject temObject;

    /*[SerializeField]
    private int ExploreConversationCount;*/

    void Start()
    {
        messageManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(5).GetComponent<MessageManager>();
        itemList = CSVReader.Read("FoundThings");
        boxGoodsList = CSVReader.Read("TreasureBoxGoods");
        mineralGoodsList = CSVReader.Read("MineralGoods");
        exploreSelectFloatingMessage = CSVReader.Read("ExploreSelectFloatingMessage");
        messageManager.setMessagePanel(true);
        waitActive = true;
        m_Message = "�ѹ�... �ѹ�...";
        m_Message2 = "���� �߰��ߴ�!";
        m_Message3 = "���͸� �����ƴ�!";
        //ExploreConversationCount = 1;

        conversationManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(4).GetComponent<ConversationManager>();
        conversationManager.AddSelectEvent(ExploreAction);
        //conversationManager.SetSelect(ref ExploreConversationCount, ref exploreSelectFloatingMessage);
        conversationManager.SetSelect(ref exploreSelectFloatingMessage);

        Character.instance.SetCharacterInput(false, false, false);
        Character.instance.MyPlayerController.SetRunState(true);
        Character.instance.MyPlayerController.PlayerRotation(Direction.Right);
        Character.instance.SetCharacterPosition();

        Character.instance.MyPlayerController.InvokeEventConversation();
        StartCoroutine("CountOneSecond", 0.1f);
        StartCoroutine("CountNNSecond", 3f);
    }

    public void ChangeMessage(string message) // �޼���â�� �޼����� �ٲ��ش�
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
        randomObjectNumber = Random.Range(0.001f, 1.000f);
        if (WhatIsFoundThings(randomObjectNumber, 0) == "monster")
        {
            //conversationManager.SetConversationCount(0, 2);
            //Character.instance.MyPlayerController.InvokeEventConversation();

            //ExploreConversationCount = 2;
            conversationManager.SetConversationCount(2);
            battleManager.GenerateMonster();
        } else
        {
            //conversationManager.SetConversationCount(0, 1);
            //Character.instance.MyPlayerController.InvokeEventConversation();

            //ExploreConversationCount = 1;
            conversationManager.SetConversationCount(1);
            temObject = Instantiate(randomObject, new Vector3(5, -2, transform.position.z), Quaternion.identity) as GameObject;
            temObject.name = WhatIsFoundThings(randomObjectNumber, 0);
        }
    }

    private string WhatIsFoundThings(float randomNumber, int WhatIsThis)
    {
        switch(WhatIsThis)
        {
            case 0: // �߰��� ����
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (randomObjectNumber <= float.Parse(itemList[i]["Rate"].ToString()))
                    {
                        ObjectNumber = i;
                        return itemList[i]["Name"].ToString();
                    }
                }
                return "";
            case 1: // �ڽ����� ���� ����
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
            case 2: // �������� ���� ����
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

    private void FloatingChoiceMessage() // �߰��� ���𰡿� ���� �ൿ �������� ����ش�.
    {
        Character.instance.MyPlayerController.InvokeEventConversation();
    }
    public void ExploreAction(int actionNumber)
    {
        switch (actionNumber)
        {
            case 0:
                switch (itemList[ObjectNumber]["Number"].ToString())
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "7":
                        switch (itemList[ObjectNumber]["Number"].ToString())
                        {
                            case "0":
                            case "1":
                            case "2":
                                Character.instance.SetCharacterStat(CharacterStatType.MyItem, WhatIsFoundThings(randomObjectNumber, 1) + ProbabilityOfHowMany());
                                ChangeMessage(boxGoodsList[goodsNumber]["Name"].ToString() + "�� ȹ���ߴ�!");
                                break;
                            case "3":
                            case "4":
                            case "5":
                                Character.instance.SetCharacterStat(CharacterStatType.MyItem, WhatIsFoundThings(randomObjectNumber, 2) + ProbabilityOfHowMany());
                                ChangeMessage(mineralGoodsList[goodsNumber]["Name"].ToString() + "�� ȹ���ߴ�!");
                                break;
                            case "7":
                                Character.instance.SetCharacterStat(CharacterStatType.MyItem, "00001000"); // ��ȭ ȹ�� �ӽ�
                                ChangeMessage("1000" + itemList[ObjectNumber]["Name"].ToString() + "�� ȹ���ߴ�!");
                                break;
                        }

                        // ȹ���� ��ȭ ǰ�� �ִ� ���
                        exploreCount++;
                        if (exploreCount != maxExplore)
                        {
                            StartCoroutine(CountGoToNext(5f));
                        }
                        else
                        {
                            EndExplore();
                        }
                        break;
                    case "6":
                        // �� �ɾ�� �������� ����� �� ���
                        battleManager.StartBattle();
                        StopCoroutine("Typing");
                        ChangeMessage(m_Message3);
                        // ����
                        break;
                }
                

                // ���ڸ� ����� �������� ����� �� ���
                break;
            case 2:
                NextExplore();
                break;
            // �����ϰ� �������� �������� ����� �� ���
            case 4:
                EndExplore();
                // �������� �������� ����� �� ���
                break;
        }
    }
    private void NextExplore()
    {
        Debug.Log("����");
        exploreCount++;
        if (exploreCount != maxExplore)
        {
            if (temObject != null)
            {
                Destroy(temObject);
            }
            else if (battleManager.GetExistTemMonster())
            {
                battleManager.DestroyTemMonster();
            }
            Character.instance.MyPlayerController.SetRunState(true);
            QuestManager.instance.ChangeMoveBG(true);
            StopCoroutine("Typing");
            StartCoroutine("CountOneSecond", 0.1f);
            StartCoroutine("CountNNSecond", 3f);
        }
        else
        {
            EndExplore();
        }
    }
    private void EndExplore()
    {
        Debug.Log("����");
        messageManager.setMessagePanel(false);
        Character.instance.SetCharacterInput(true, true, true);
        if (Character.instance.MyJob == Job.Slayer)
        {
            Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0001");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Minigame");
        }
        else
        {
            Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0013"); // ���������� Town
            UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
        }
    }
    private string ProbabilityOfHowMany() // ���� �������� �� ���� Ȯ��
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
        NextExplore();
    }

    public void SetBattleManager(BattleManager newBattleManager)
    {
        battleManager = newBattleManager;
    }

    /*    1�ʿ� �� ��, �ѹ� �ѹ� �Ȱ� �ִٴ� �޽����� ȭ�鿡 ����ش�.
     *    N�ʿ� �� ��, ���缭�� ���𰡸� �߰��Ѵ�
     *    �߰��ϴ� ���𰡴� - ex) ����, ����, ����, ��ȭ, ����, ��[������ or ����]
     *    �߰��� ���𰡿� ���� ��� �ൿ�� �� �������� �ش�.
     *    �������� ���� �ൿ�Ѵ�.*/
}
