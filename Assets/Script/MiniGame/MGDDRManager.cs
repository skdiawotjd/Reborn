using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MGDDRManager : MiniGameManager
{
    private int keyStack; // ���� ����� ������ Ű�� ����
    private int keyCount; // ���� ���� �˸°� ���� Ű�� ����
    private float playTime; // ���� ���� �ð�
    private bool gameActive = false; // ���� ���ɿ���
    private int[] RandomKey;
    private int round;
    private int maxRound;
    private int[] keyOfRound;
    private Vector3 managerTrans;
    [SerializeField]
    private Arrow arrow;
    private TextMeshProUGUI timeText;
    Arrow[] arrowArray;
    private Slider timeSlider;
    private GameObject temImage;
    private SpriteRenderer temSprite;
    [SerializeField]
    private GameObject MinigameClothImage;
    [SerializeField]
    private Sprite MinigameCloth2;
    [SerializeField]
    private Sprite MinigameCloth3;
    [SerializeField]
    private Sprite Tshirt1;
    [SerializeField]
    private Sprite Tshirt2;
    [SerializeField]
    private Sprite Tshirt3;
    [SerializeField]
    private Sprite Tshirt4;
    [SerializeField]
    private GameObject BookObject;
    [SerializeField]

    private void Awake()
    {
        GameType = 3;

        timeText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        timeSlider = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Slider>();

        managerTrans = new Vector3(0, 0, 0);

        keyCount = 0;
        round = 0;

        keyOfRound = new int[5];
        maxRound = keyOfRound.Length - 1;
        keyOfRound[0] = 3;
        keyOfRound[1] = 4;
        keyOfRound[2] = 5;
        keyOfRound[3] = 5;
        keyOfRound[4] = 5;

    }

    protected override void Start()
    {
        base.Start();
        panelActiveSelf = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
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
        if (panelActiveSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FrameWorkManager.SetDisabledPanel();
                Character.instance.SetCharacterInput(true, true, true);
            }
        }
    }

    public override void GameStart()
    {
        FrameWorkManager.GameStart();
    }
    public override void GameEnd(bool clear)
    {
        FrameWorkManager.GameEnd();
        StopCoroutine("CountTime");
        timeSlider.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        Destroy(temImage);
        gameActive = false;
        Character.instance.SetCharacterInput(true, true, true);
        QuestManager.instance.MinigameClear(true);
    }
    public override void SetRound(int nextRound)
    {
        if(!gameActive)
        {
            timeSlider.gameObject.SetActive(true);
            timeText.gameObject.SetActive(true);
        }
        SetMainWork(keyOfRound[nextRound]);
        Generate();
        gameActive = true;
    }
    public override void SetGame() // MinigameDdr()
    {
        playTime = 60.0f; // �÷��� Ÿ���� ���Ѵ�
        timeSlider.maxValue = playTime; // �÷��� Ÿ�ӿ� �°� �ð� ���α׷��� ���� �ִ밪�� �����ش�
        if (Character.instance.MyMapNumber == "0002")
        {
            temImage = Instantiate(MinigameClothImage) as GameObject;
            temSprite = temImage.GetComponent<SpriteRenderer>();
        }
        else
        {
            //temImage = Instantiate(BookObject) as GameObject;
        }
        StartCoroutine("CountTime", 0.1);
    }
    public override void SetMainWork(int key) // setKey()
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
    public override void Generate()
    {
        for (int i = 0; i < arrowArray.Length; i++)
        {
            arrowArray[i] = Instantiate(arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity) as Arrow;
            // �θ� ������Ʈ ����. �θ��� transform�� �ޱ� ����
            switch(Character.instance.MyMapNumber)
            {
                case "0002":
                    if(RandomKey[i] == 0)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt1;
                    }
                    else if(RandomKey[i] == 1)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt2;
                    }
                    else if(RandomKey[i] == 2)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt3;
                    }
                    else if (RandomKey[i] == 3)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt4;
                    }
                    arrowArray[i].transform.localScale = Vector3.one;
                    break;
                default:
                    arrowArray[i].transform.Rotate(0, 0, 90 * RandomKey[i]);
                    break;
            }
        }
    }
    public override void PressKey(int key)
    {
        if (gameActive) // ddr ����
        {
            if (RandomKey[keyCount] == key)
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
                                if (Character.instance.MyMapNumber == "0002")
                                {
                                    if (round == maxRound)
                                    {
                                        temSprite.sprite = MinigameCloth3;
                                    }
                                    else
                                    {
                                        temSprite.sprite = MinigameCloth2;
                                    }
                                }
                                SetRound(round);
                            }
                            else
                            {
                                round = 0;
                                GameEnd(true);
                            }
                        }
                    }
                }
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
                for(int i = keyCount; i < keyStack; i++)
                {
                    arrowArray[i].DestroyImage();
                }
                GameEnd(false);
            }
        }
    }
    public override IEnumerator CountTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (playTime > 0f && gameActive) // ddr ���� ������
        {
            playTime -= 0.1f;
            timeSlider.value = playTime;
            StartCoroutine("CountTime", 0.1f);
        }
    }
}
