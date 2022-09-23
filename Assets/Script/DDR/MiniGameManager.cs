using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    private QuestManager questManager;

    // DDR ���� 
    private int keyStack; // ���� ����� ������ Ű�� ����
    private int keyCount; // ���� ���� �˸°� ���� Ű�� ����
    private float playTime; // ���� ���� �ð�
    private bool gameActive = false; // ���� ���ɿ���
    private int[] RandomKey;
    private int round;
    private int maxRound;
    private int[] keyOfRound;
    private Vector3 managerTrans;
    public Arrow Arrow;
    public TextMeshProUGUI text;
    private TextMeshProUGUI timeText;
    Arrow[] arrowArray;
    public Slider timeSlider;
    private bool gameClear;

    // Timing ����

    private Slider timingSlider;
    private Good good;
    private float timingValue;
    private bool timingChangeDirection;
    private bool timingGameActive = false;
    private int timingCount;
    private Image perfectFloor;
    private float randomNumber;
    private int temNumber;
    private int timingRound;

    // Quiz ����

    private bool quizGameActive = false;
    private int answerChoiceNumber;
    private int answerNumber;
    private int quizRound;
    private Color temColor;
    List<Dictionary<string, object>> quizList;
    private Image quizPanel;
    private Image contextPanel;
    private TextMeshProUGUI quizText;
    private TextMeshProUGUI contextText;
    private Image[] answerPanel;
    private TextMeshProUGUI[] answerText;


    void Start()
    {
        questManager = GetComponent<QuestManager>();
        timeText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        timingSlider = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Slider>();
        perfectFloor = GameObject.Find("Canvas").transform.GetChild(3).GetChild(2).GetComponent<Image>();
        good = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<Good>();
        quizPanel = GameObject.Find("Canvas").transform.GetChild(5).GetComponent<Image>();
        quizText = quizPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        contextPanel = quizPanel.transform.GetChild(1).GetComponent<Image>();
        contextText = contextPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        answerPanel = new Image[4];
        answerText = new TextMeshProUGUI[answerPanel.Length];
        for (int i=0; i < answerPanel.Length; i++)
        {
            answerPanel[i] = quizPanel.transform.GetChild(i+2).GetComponent<Image>();
            answerText[i] = answerPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        temColor = Color.white;
        managerTrans = new Vector3(0, 0, 0);
        keyCount = 0;
        round = 0;
        keyOfRound = new int[3];
        maxRound = keyOfRound.Length -1;
        keyOfRound[0] = 3;
        keyOfRound[1] = 4;
        keyOfRound[2] = 5;


        Character.instance.transform.position = new Vector3(0f, 0f, 0f);


    }
    public void GameStart(int gameType)
    {
        switch(gameType)
        {
            case 0:// Ÿ�̹� ���߱� ����
                timingValue = 0f;
                timingChangeDirection = true;
                timingGameActive = true;
                timingCount = 0;
                SetTimingPosition();
                break;
            case 1:// ddr ����
                timeSlider.gameObject.SetActive(true);
                timeText.gameObject.SetActive(true);
                MiniGameDdr();
                SetRound(0);
                StartCoroutine("CountTime", 0.1);
                break;
            case 2:// Quiz ����
                quizRound = 0;
                answerChoiceNumber = 0;
                QuizGenerate();
                QuizSetting();
                timeText.gameObject.SetActive(true);
                quizPanel.gameObject.SetActive(true);
                StartCoroutine(CountTime(0.1f));
                break;
            default:
                break;
        }
    }
    private void GameEnd(int gameType, bool clear)
    {
        switch (gameType)
        {
            case 0:// Ÿ�̹� ���߱� ��
                timingSlider.gameObject.SetActive(false);
                timingGameActive = false;
                break;
            case 1:// ddr ��
                timeSlider.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                gameActive = false;
                break;
            case 2:// Quiz ��
                timeText.gameObject.SetActive(false);
                quizPanel.gameObject.SetActive(false);
                quizGameActive = false;
                break;
            default:
                break;
        }
        text.text = clear ? "Clear!" : "Failed";
        text.gameObject.SetActive(true);
        StartCoroutine(WaitingTime(3f));
        text.gameObject.SetActive(false);
        questManager.QuestClear((int)Character.instance.MyJob, clear);
        Character.instance.SetCharacterInput(true, true);
    }
    void Update()
    {
        if(gameActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PressKey(0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PressKey(1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PressKey(2);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PressKey(3);
            }
        }
        if (timingGameActive)
        {
            // Ÿ�ָ̹��߱�
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timingGameActive = false;
                StopCoroutine("ChangeTimingValue");
                good.gameObject.SetActive(true);
                if (timingSlider.value > (randomNumber - 1.5f) && timingSlider.value < (randomNumber + 1.5f))
                {
                    good.ChangeSource(true);
                    Debug.Log("����");
                }
                else
                {
                    good.ChangeSource(false);
                    Debug.Log("����");
                }
                StartCoroutine("WaitingTime", 0.5f);
                timingRound++;
                if (timingRound < 5)
                {
                    SetTimingPosition();
                }
                else if (timingRound == 5)
                {
                    GameEnd(0, true);
                }
            }
        }
        if (quizGameActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PressKey(0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PressKey(1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PressKey(2);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PressKey(3);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                quizGameActive = false;
                good.gameObject.SetActive(true);
                if (answerChoiceNumber == answerNumber)
                {
                    good.ChangeSource(true);
                }
                else
                {
                    good.ChangeSource(false);
                }
                StartCoroutine("WaitingTime", 0.5f);
                quizRound++;
                if(quizRound < 5)
                {
                    QuizSetting();
                }
                else if (quizRound == 5)
                {
                    GameEnd(2, true);
                }

            }
        }
    }


    // ���⼭���� ddr ����
    void SetRound(int nextRound)
    {
        SetKey(keyOfRound[nextRound]);
        generate();
        gameActive = true;
    }
    void SetKey(int key)
    {
        keyStack = key; // �̹� ������ Ű ������ ��������
        managerTrans.x = -(key - 1);
        gameObject.transform.position = managerTrans;
        RandomKey = new int[keyStack];  // Ű ������ŭ �迭�� ������ش�
        arrowArray = new Arrow[keyStack]; // Ű ������ŭ �ַο찡 �� �迭�� ������ش�

        for (int i = 0; i < keyStack; i++) // �迭�ȿ� 0~3���� ���� ���� / 0 = L , 1 = R , 2 = U, 3 = D
        {
            RandomKey[i] = Random.Range(0, 4);
        }
    }
    void generate() // ���� Arrow�� ���ӿ�����Ʈ �迭�� �����ϰ� Ű ���⿡ ���� ȸ�������ش�
    {
        for (int i = 0; i < arrowArray.Length; i++)
        {
            arrowArray[i] = Instantiate(Arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity) as Arrow;
            // �θ� ������Ʈ ����. �θ��� transform�� �ޱ� ����

            arrowArray[i].transform.Rotate(0, 0, 90 * RandomKey[i]);

        }
    }
    void PressKey(int key) // ����� ���ȴ°�? �� �Ǵ�
    {
        if (timingGameActive) // Ÿ�̹� ����
        {
            if (RandomKey[keyCount] == key)
            {
                CompareKey(key);
            }
            else if (playTime > 5)
            {
                Debug.Log("�� �� ����");
                playTime -= 5;
            }
            else
            {
                playTime = 0;
                timeSlider.value = playTime;
                GameEnd(1, false);
            }
        }
        else if (quizGameActive) // ���� ����
        {
            switch (key)
            {
                case 0:
                    if (answerChoiceNumber == 2 || answerChoiceNumber == 3)
                    {
                        SelectedChange(0);
                    }
                    break;
                case 1:
                    if (answerChoiceNumber == 1 || answerChoiceNumber == 3)
                    {
                        SelectedChange(1);
                    }
                    break;
                case 2:
                    if (answerChoiceNumber == 0 || answerChoiceNumber == 1)
                    {
                        SelectedChange(2);
                    }
                    break;
                case 3:
                    if (answerChoiceNumber == 0 || answerChoiceNumber == 2)
                    {
                        SelectedChange(3);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    void CompareKey(int key) // ����� ���ȴٸ� keyCount�� ���� �����ش�.
    {
        if (RandomKey[keyCount] == key) // Ű �ڽ��� ���ڿ� ���� Ű�� ��ġ����
        {
            if (keyCount != keyStack) // Ű ī��Ʈ�� �ִ밪�� �ƴ� ��
            {
                arrowArray[keyCount].ArrowAnim();
                keyCount++; // Ű ī��Ʈ�� ������Ų��
                if (keyCount == keyStack) // Ű ī��Ʈ�� �ִ뿡 ���� ���� ��
                {
                    keyCount = 0;
                    if (round++ != maxRound)
                    {
                        SetRound(round);
                    }
                    else
                    {
                        GameEnd(1, true);
                    }


                }
            }

        }
    }
    void MiniGameDdr()
    {
        playTime = 60.0f; // �÷��� Ÿ���� ���Ѵ�
        timeSlider.maxValue = playTime; // �÷��� Ÿ�ӿ� �°� �ð� ���α׷��� ���� �ִ밪�� �����ش�
    }

    // ���⼭���� Ÿ�̹� ���߱�

    void SetTimingRound() // Ÿ�̹� ���߱�
    {
        timingRound = 0;
        SetTimingPosition();
    }
    void SetTimingPosition() // Ÿ�̹� ���߱�
    {
        if (timingRound < 5)
        {
            randomNumber = Random.Range(2.0f, 8.0f);
            temNumber = (int)(randomNumber * 100f);
            perfectFloor.rectTransform.anchoredPosition = new Vector3(temNumber, perfectFloor.rectTransform.anchoredPosition.y);
            timingValue = 0;
            timingSlider.value = timingValue;
            timingChangeDirection = true;

            timingSlider.gameObject.SetActive(true);
            timingGameActive = true;
            StartCoroutine("ChangeTimingValue", 0.1f);
        }
    }

    // ���⼭���� Quiz

    void SelectedChange(int position) // Quiz
    {
        temColor = answerPanel[answerChoiceNumber].color; // ���� �г��� �÷��� �ӽ� �÷��� �ִ´�
        temColor.a = 0.4f; // �ӽ� �÷��� ���İ��� 0.4f, �� 100 ������ �����
        answerPanel[answerChoiceNumber].color = temColor; // �г��� �÷����� �ӽ� �÷����� �����Ų��.
        switch(position)
        {
            case 0:
                answerChoiceNumber -= 2; // ���õ� �ѹ��� ����
                break;
            case 1:
                answerChoiceNumber -= 1;
                break;
            case 2:
                answerChoiceNumber += 2;
                break;
            case 3:
                answerChoiceNumber += 1;
                break;
        }
        

        temColor = answerPanel[answerChoiceNumber].color; // ���� �г��� �÷��� �ӽ� �÷��� �ִ´�
        temColor.a = 1f; // �ӽ� �÷��� ���İ��� 1f, �� 255�� ����� (������ - ���õ�)
        answerPanel[answerChoiceNumber].color = temColor; // �г��� �÷����� �ӽ� �÷����� �����Ų��.
    }
    void QuizGenerate()
    {
        quizList = CSVReader.Read("QuizText");

    }
    void QuizSetting()
    {
        quizGameActive = true;
        playTime = 60.0f; // �÷��� Ÿ���� ���Ѵ�

        contextText.text = quizList[quizRound]["Context"].ToString();
        answerText[0].text = quizList[quizRound]["Answer1"].ToString();
        answerText[1].text = quizList[quizRound]["Answer2"].ToString();
        answerText[2].text = quizList[quizRound]["Answer3"].ToString();
        answerText[3].text = quizList[quizRound]["Answer4"].ToString();
        answerNumber = int.Parse(quizList[quizRound]["AnswerNumber"].ToString()) - 1;
    }

    IEnumerator CountTime(float delayTime) // 0.1�ʿ� �ѹ��� �ð��� ���δ�
    { 
        Debug.Log("Time : " + playTime); 
        yield return new WaitForSeconds(delayTime);
        if(playTime > 0f && gameActive) // ddr ���� ������
        {
            playTime -= 0.1f;
            timeSlider.value = playTime;
            StartCoroutine("CountTime", 0.1f);
        }
        else if(playTime > 0f && quizGameActive) // ���� ���� ������
        {
            playTime -= 0.1f;
            timeText.text = playTime.ToString();
            StartCoroutine(CountTime(0.1f));

        }
        else if(playTime < 0f && quizGameActive)
        {
            playTime = 0f;
            GameEnd(2, false);
        }
    }

    IEnumerator ChangeTimingValue(float delayTime) // Ÿ�̹� ���߱� �ڷ�ƾ
    {
        yield return new WaitForSeconds(delayTime);
        if(timingGameActive)
        {
            if (timingChangeDirection && timingValue < 10)
            {
                timingSlider.value = timingValue;
                timingValue += 0.2f;
                StartCoroutine("ChangeTimingValue", 0.0075f);
            }
            else if (!timingChangeDirection && timingValue > 0)
            {
                timingSlider.value = timingValue;
                timingValue -= 0.2f;
                StartCoroutine("ChangeTimingValue", 0.0075f);
            }
            else
            {
                timingChangeDirection = !timingChangeDirection;
                StartCoroutine("ChangeTimingValue", 0.0075f);
                //timingGameActive = false;
            }
        }
    }
    IEnumerator WaitingTime(float delayTime) // ���� �ð� ���
    {
        yield return new WaitForSeconds(delayTime);
    }
}
