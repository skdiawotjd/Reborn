using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
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


    void Start()
    {
        timeText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        timingSlider = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Slider>();
        perfectFloor = GameObject.Find("Canvas").transform.GetChild(3).GetChild(2).GetComponent<Image>();
        good = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<Good>();
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
    public void TimingStart() // 타이밍 맞추기 시작
    {
        timingValue = 0f;
        timingChangeDirection = true;
        timingGameActive = true;
        timingCount = 0;
        SetTimingPosition();
    }
    public void DdrStart()
    {
        timeSlider.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        MiniGameDdr();
        SetRound(0);
        StartCoroutine("CountTime", 0.1);
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
                SetTimingPosition();
            }
        }

    }

    void SetRound(int nextRound)
    {
        SetKey(keyOfRound[nextRound]);
        generate();
        gameActive = true;
    }
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
        }
    }
    void CompareKey(int key) // 제대로 눌렸다면 keyCount를 증가 시켜준다.
    {
        if (RandomKey[keyCount] == key) // 키 박스의 숫자와 눌린 키가 일치한지
        {
            if(keyCount != keyStack) // 키 카운트가 최대값이 아닐 때
            {
                arrowArray[keyCount].ArrowAnim();
                keyCount++; // 키 카운트를 증가시킨다
                if(keyCount == keyStack) // 키 카운트가 최대에 도달 했을 때
                {
                    keyCount = 0;
                    if(round++ != maxRound)
                    {
                        SetRound(round);
                    }
                    else
                    {
                        gameActive=false;
                        text.gameObject.SetActive(true);
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
    IEnumerator CountTime(float delayTime) // 0.1초에 한번씩 시간을 줄인다
    { 
        Debug.Log("Time : " + playTime); 
        yield return new WaitForSeconds(delayTime);
        if(playTime > 0 && gameActive)
        {
            playTime -= 0.1f;
            timeSlider.value = playTime;
            StartCoroutine("CountTime", 0.1f);
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
    IEnumerator WatingTime(float delayTime) // 일정 시간 대기
    {
        yield return new WaitForSeconds(delayTime);
    }
}
