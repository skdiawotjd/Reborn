using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MGQuizManager : MiniGameManager
{
    private bool quizGameActive = false;
    private int answerChoiceNumber;
    private int answerNumber;
    private int quizRound;
    private int gameNumberRound;
    private int temNum;
    private Color temColor;
    private Good good;
    List<Dictionary<string, object>> quizList;
    private Image quizPanel;
    private Image contextPanel;
    private TextMeshProUGUI quizText;
    private TextMeshProUGUI contextText;
    private Image[] answerPanel;
    private TextMeshProUGUI[] answerText;
    private TextMeshProUGUI timeText;
    [SerializeField]
    private GameObject BookObject;
    private GameObject temImage;
    private float playTime; // ���� ���� �ð�

    private void Awake()
    {
        good = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<Good>();

        timeText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        quizPanel = GameObject.Find("Canvas").transform.GetChild(5).GetComponent<Image>();
        quizText = quizPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        contextPanel = quizPanel.transform.GetChild(1).GetComponent<Image>();
        contextText = contextPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        answerPanel = new Image[4];
        answerText = new TextMeshProUGUI[answerPanel.Length];
        for (int i = 0; i < answerPanel.Length; i++)
        {
            answerPanel[i] = quizPanel.transform.GetChild(i + 2).GetComponent<Image>();
            answerText[i] = answerPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        temColor = Color.white;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
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
                if (quizRound < 5)
                {
                    SetGame();
                }
                else if (quizRound == 5)
                {
                    GameEnd(true);
                }

            }
        }
    }
    public override void GameStart()
    {
        quizRound = 0;
        answerChoiceNumber = 0;
        Generate();
        SetGame();
        timeText.gameObject.SetActive(true);
        quizPanel.gameObject.SetActive(true);
        StartCoroutine(CountTime(0.1f));
    }
    public override void GameEnd(bool clear)
    {
        timeText.gameObject.SetActive(false);
        quizPanel.gameObject.SetActive(false);
        if (Character.instance.MyMapNumber == "0004" || Character.instance.MyMapNumber == "0104")
        {
            Destroy(temImage);
        }

        quizGameActive = false;
    }

    public override void PressKey(int key)
    {
        switch (key)
        {
            case 0:
                if (answerChoiceNumber == 2 || answerChoiceNumber == 3)
                {
                    SetMainWork(0);
                }
                break;
            case 1:
                if (answerChoiceNumber == 1 || answerChoiceNumber == 3)
                {
                    SetMainWork(1);
                }
                break;
            case 2:
                if (answerChoiceNumber == 0 || answerChoiceNumber == 1)
                {
                    SetMainWork(2);
                }
                break;
            case 3:
                if (answerChoiceNumber == 0 || answerChoiceNumber == 2)
                {
                    SetMainWork(3);
                }
                break;
            default:
                break;
        }
    }
    public override void SetGame() // QuizSetting()
    {
        quizGameActive = true;
        playTime = 60.0f; // �÷��� Ÿ���� ���Ѵ�
        temNum = Random.Range(1, 3);
        switch (Character.instance.MyMapNumber)
        {
            case "0004":
                gameNumberRound = quizRound * temNum;
                break;
            case "0104":
                gameNumberRound = (quizRound * temNum) + 10;
                break;
        }
        contextText.text = quizList[gameNumberRound]["Context"].ToString();
        answerText[0].text = quizList[gameNumberRound]["Answer1"].ToString();
        answerText[1].text = quizList[gameNumberRound]["Answer2"].ToString();
        answerText[2].text = quizList[gameNumberRound]["Answer3"].ToString();
        answerText[3].text = quizList[gameNumberRound]["Answer4"].ToString();
        answerNumber = int.Parse(quizList[gameNumberRound]["AnswerNumber"].ToString()) - 1;
    }
    public override void Generate() // QuizGenerate()
    {
        quizList = CSVReader.Read("QuizText");
        temImage = Instantiate(BookObject) as GameObject;
    }
    public override void SetMainWork(int position) // SelectedChange()
    {
        temColor = answerPanel[answerChoiceNumber].color; // ���� �г��� �÷��� �ӽ� �÷��� �ִ´�
        temColor.a = 0.4f; // �ӽ� �÷��� ���İ��� 0.4f, �� 100 ������ �����
        answerPanel[answerChoiceNumber].color = temColor; // �г��� �÷����� �ӽ� �÷����� �����Ų��.
        switch (position)
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
    public override IEnumerator CountTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (playTime > 0f && quizGameActive) // ���� ���� ������
        {
            playTime -= 0.1f;
            timeText.text = playTime.ToString();
            StartCoroutine(CountTime(0.1f));
        }
        else if (playTime < 0f && quizGameActive)
        {
            playTime = 0f;
            GameEnd(false);
        }
    }
    }
