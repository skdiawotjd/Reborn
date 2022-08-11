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
    private bool gameClear = true; // ���� ���ɿ���
    private int[] RandomKey;
    public GameObject Arrow;
    public TextMeshProUGUI text;
    GameObject[] arrowArray;
    public Slider timeSlider;
    


    void Start()
    {
        keyCount = 0;
        MiniGameDdr();
        SetKey();
        generate();
        StartCoroutine("CountTime", 0.1);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && gameClear)
        {
            PressKey(0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && gameClear)
        {
            PressKey(1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && gameClear)
        {
            PressKey(2);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && gameClear)
        {
            PressKey(3);
        }
    }

    void SetKey()
    {
        keyStack = 7; // �̹� ������ Ű ������ ��������
        RandomKey = new int[keyStack];  // Ű ������ŭ �迭�� ������ش�
        arrowArray = new GameObject[keyStack]; // Ű ������ŭ �ַο찡 �� �迭�� ������ش�

        for (int i = 0; i < keyStack; i++) // �迭�ȿ� 0~3���� ���� ���� / 0 = L , 1 = R , 2 = U, 3 = D
        {
            RandomKey[i] = Random.Range(0, 4);   
        }

        Visual(); // �� Ű�� �´� �׸� �����ִ� �Լ� ȣ��
    }
    void Visual()
    {

    }
    void generate() // ���� Arrow�� ���ӿ�����Ʈ �迭�� �����ϰ� Ű ���⿡ ���� ȸ�������ش�
    {
        for (int i = 0; i < arrowArray.Length; i++)
        {
            arrowArray[i] = Instantiate(Arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity);
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
                Destroy(arrowArray[keyCount]); // �� ���� Ű�� ���ش�
                keyCount++; // Ű ī��Ʈ�� ������Ų��
                if(keyCount == keyStack) // Ű ī��Ʈ�� �ִ뿡 ���� ���� ��
                {
                    gameClear = false;
                    keyCount = 0;
                    text.gameObject.SetActive(true);
                    Debug.Log("GameClear! Time : " + playTime);
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
        if(playTime > 0 && gameClear)
        {
            playTime -= 0.1f;
            timeSlider.value = playTime;
            StartCoroutine("CountTime", 0.1f);
        }
         
    }
}
