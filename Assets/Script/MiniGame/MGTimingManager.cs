using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGTimingManager : MiniGameManager
{
    private Slider timingSlider;
    private Good good;
    private float timingValue;
    private bool timingChangeDirection;
    private bool timingGameActive = false;
    private Image perfectFloor;
    private float randomNumber;
    private int temNumber;
    private int timingRound;
    [SerializeField]
    private GameObject Anvil;
    private GameObject temImage;
    private void Awake()
    {
        good = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<Good>();
        perfectFloor = GameObject.Find("Canvas").transform.GetChild(3).GetChild(2).GetComponent<Image>();
        timingSlider = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Slider>();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
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
                StartCoroutine("CountTime", 0.5f);
                timingRound++;
                if (timingRound < 5)
                {
                    SetMainWork();
                }
                else if (timingRound == 5)
                {
                    GameEnd(true);
                }
            }
        }
    }

    public override void GameStart()
    {
        timingValue = 0f;
        timingChangeDirection = true;
        timingGameActive = true;
        SetMainWork();
    }

    public override void GameEnd(bool clear)
    {
        timingSlider.gameObject.SetActive(false);
        if (Character.instance.MyMapNumber == "0003")
        {
            Destroy(temImage);
        }
        timingGameActive = false;
        Character.instance.SetCharacterInput(true, true, true);
    }
    public override void SetRound(int num) // SetTimingRound()
    {
        timingRound = 0;
        if (Character.instance.MyMapNumber == "0003")
        {
            temImage = Instantiate(Anvil) as GameObject;
        }
        SetMainWork();
    }
    public override void SetMainWork() // SetTimingPosition()
    {
        if (timingRound < 5)
        {
            randomNumber = Random.Range(2.0f, 8.0f); // 2에서 8까지 랜덤 넘버를 잡는다 >> 타이밍 카운터가 움직일 범위
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
    public override IEnumerator ChangeTimingValue(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (timingGameActive)
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
    public override IEnumerator CountTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }
}
