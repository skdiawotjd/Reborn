using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    private QuestManager questManager;

    // DDR 변수 
    private int keyStack; // 게임 진행시 나오는 키의 개수
    private int keyCount; // 진행 도중 알맞게 누른 키의 개수
    private float playTime; // 게임 진행 시간
    private bool gameActive = false; // 게임 가능여부
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

    // Timing 변수

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

    // Quiz 변수

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
            case 0:// 타이밍 맞추기 시작
                timingValue = 0f;
                timingChangeDirection = true;
                timingGameActive = true;
                timingCount = 0;
                SetTimingPosition();
                break;
            case 1:// ddr 시작
                timeSlider.gameObject.SetActive(true);
                timeText.gameObject.SetActive(true);
                MiniGameDdr();
                SetRound(0);
                StartCoroutine("CountTime", 0.1);
                break;
            case 2:// Quiz 시작
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
            case 0:// 타이밍 맞추기 끝
                timingSlider.gameObject.SetActive(false);
                timingGameActive = false;
                break;
            case 1:// ddr 끝
                timeSlider.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                gameActive = false;
                break;
            case 2:// Quiz 끝
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
            // 타이밍맞추기
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timingGameActive = false;
                StopCoroutine("ChangeTimingValue");
                good.gameObject.SetActive(true);
                if (timingSlider.value > (randomNumber - 1.5f) && timingSlider.value < (randomNumber + 1.5f))
                {
                    good.ChangeSource(true);
                    Debug.Log("명중");
                }
                else
                {
                    good.ChangeSource(false);
                    Debug.Log("실패");
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


    // 여기서부터 ddr 게임
    void SetRound(int nextRound)
    {
        SetKey(keyOfRound[nextRound]);
        generate();
        gameActive = true;
    }
    void SetKey(int key)
    {
        keyStack = key; // 이번 게임의 키 개수가 정해진다
        managerTrans.x = -(key - 1);
        gameObject.transform.position = managerTrans;
        RandomKey = new int[keyStack];  // 키 개수만큼 배열을 만들어준다
        arrowArray = new Arrow[keyStack]; // 키 개수만큼 애로우가 들어갈 배열을 만들어준다

        for (int i = 0; i < keyStack; i++) // 배열안에 0~3까지 난수 생성 / 0 = L , 1 = R , 2 = U, 3 = D
        {
            RandomKey[i] = Random.Range(0, 4);
        }
    }
    void generate() // 실제 Arrow를 게임오브젝트 배열에 생성하고 키 방향에 따라 회전시켜준다
    {
        for (int i = 0; i < arrowArray.Length; i++)
        {
            arrowArray[i] = Instantiate(Arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity) as Arrow;
            // 부모 오브젝트 설정. 부모의 transform을 받기 위함

            arrowArray[i].transform.Rotate(0, 0, 90 * RandomKey[i]);

        }
    }
    void PressKey(int key) // 제대로 눌렸는가? 를 판단
    {
        if (timingGameActive) // 타이밍 게임
        {
            if (RandomKey[keyCount] == key)
            {
                CompareKey(key);
            }
            else if (playTime > 5)
            {
                Debug.Log("잘 못 누름");
                playTime -= 5;
            }
            else
            {
                playTime = 0;
                timeSlider.value = playTime;
                GameEnd(1, false);
            }
        }
        else if (quizGameActive) // 퀴즈 게임
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
    void CompareKey(int key) // 제대로 눌렸다면 keyCount를 증가 시켜준다.
    {
        if (RandomKey[keyCount] == key) // 키 박스의 숫자와 눌린 키가 일치한지
        {
            if (keyCount != keyStack) // 키 카운트가 최대값이 아닐 때
            {
                arrowArray[keyCount].ArrowAnim();
                keyCount++; // 키 카운트를 증가시킨다
                if (keyCount == keyStack) // 키 카운트가 최대에 도달 했을 때
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
        playTime = 60.0f; // 플레이 타임을 정한다
        timeSlider.maxValue = playTime; // 플레이 타임에 맞게 시간 프로그레스 바의 최대값을 정해준다
    }

    // 여기서부터 타이밍 맞추기

    void SetTimingRound() // 타이밍 맞추기
    {
        timingRound = 0;
        SetTimingPosition();
    }
    void SetTimingPosition() // 타이밍 맞추기
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

    // 여기서부터 Quiz

    void SelectedChange(int position) // Quiz
    {
        temColor = answerPanel[answerChoiceNumber].color; // 현재 패널의 컬러를 임시 컬러에 넣는다
        temColor.a = 0.4f; // 임시 컬러의 알파값을 0.4f, 즉 100 정도로 만든다
        answerPanel[answerChoiceNumber].color = temColor; // 패널의 컬러값에 임시 컬러값을 적용시킨다.
        switch(position)
        {
            case 0:
                answerChoiceNumber -= 2; // 선택된 넘버를 변경
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
        

        temColor = answerPanel[answerChoiceNumber].color; // 현재 패널의 컬러를 임시 컬러에 넣는다
        temColor.a = 1f; // 임시 컬러의 알파값을 1f, 즉 255로 만든다 (불투명 - 선택됨)
        answerPanel[answerChoiceNumber].color = temColor; // 패널의 컬러값에 임시 컬러값을 적용시킨다.
    }
    void QuizGenerate()
    {
        quizList = CSVReader.Read("QuizText");

    }
    void QuizSetting()
    {
        quizGameActive = true;
        playTime = 60.0f; // 플레이 타임을 정한다

        contextText.text = quizList[quizRound]["Context"].ToString();
        answerText[0].text = quizList[quizRound]["Answer1"].ToString();
        answerText[1].text = quizList[quizRound]["Answer2"].ToString();
        answerText[2].text = quizList[quizRound]["Answer3"].ToString();
        answerText[3].text = quizList[quizRound]["Answer4"].ToString();
        answerNumber = int.Parse(quizList[quizRound]["AnswerNumber"].ToString()) - 1;
    }

    IEnumerator CountTime(float delayTime) // 0.1초에 한번씩 시간을 줄인다
    { 
        Debug.Log("Time : " + playTime); 
        yield return new WaitForSeconds(delayTime);
        if(playTime > 0f && gameActive) // ddr 게임 진행중
        {
            playTime -= 0.1f;
            timeSlider.value = playTime;
            StartCoroutine("CountTime", 0.1f);
        }
        else if(playTime > 0f && quizGameActive) // 퀴즈 게임 진행중
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

    IEnumerator ChangeTimingValue(float delayTime) // 타이밍 맞추기 코루틴
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
    IEnumerator WaitingTime(float delayTime) // 일정 시간 대기
    {
        yield return new WaitForSeconds(delayTime);
    }
}
