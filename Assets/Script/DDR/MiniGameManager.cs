using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
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
    public Arrow Arrow;
    public GameObject ArrowBox;
    public TextMeshProUGUI text;
    private TextMeshProUGUI timeText;
    Arrow[] arrowArray;
    public Slider timeSlider;
    


    void Start()
    {
        timeText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
    public void gameStart()
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

    }

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
            //arrowArray[i] = Instantiate(Arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity);
            arrowArray[i] = Instantiate(Arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity) as Arrow;
            //arrowArray[i].GetComponent<RectTransform>().SetParent(ArrowBox.transform, false);

            //RectTransform btnpos = button.GetComponent<RectTransform>(); // ��ư�� Transform�� �����´�
            //btnpos.SetParent(gameObject.transform.GetChild(0).transform, false);
            // �θ� ������Ʈ ����. �θ��� transform�� �ޱ� ����

            arrowArray[i].transform.Rotate(0, 0, 90 * RandomKey[i]);
            
        }
    }
    void PressKey(int key) // ����� ���ȴ°�? �� �Ǵ�
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
        }
    }
    void CompareKey(int key) // ����� ���ȴٸ� keyCount�� ���� �����ش�.
    {
        if (RandomKey[keyCount] == key) // Ű �ڽ��� ���ڿ� ���� Ű�� ��ġ����
        {
            if(keyCount != keyStack) // Ű ī��Ʈ�� �ִ밪�� �ƴ� ��
            {
                //Destroy(arrowArray[keyCount]); // �� ���� Ű�� ���ش�
                arrowArray[keyCount].ArrowAnim();
                keyCount++; // Ű ī��Ʈ�� ������Ų��
                if(keyCount == keyStack) // Ű ī��Ʈ�� �ִ뿡 ���� ���� ��
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
        playTime = 60.0f; // �÷��� Ÿ���� ���Ѵ�
        timeSlider.maxValue = playTime; // �÷��� Ÿ�ӿ� �°� �ð� ���α׷��� ���� �ִ밪�� �����ش�
    }
    IEnumerator CountTime(float delayTime) // 0.1�ʿ� �ѹ��� �ð��� ���δ�
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
}
