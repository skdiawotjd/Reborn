using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FloatingOptionManager : MonoBehaviour
{
    public Image optionPanel;
    [SerializeField]
    private Button optionButtonPrefab;
    private TextMeshProUGUI optionText;
    private Button[] temButton;

    void Start()
    {
        optionPanel = transform.GetChild(0).GetComponent<Image>();
        temButton = new Button[3];
    }


    void Update()
    {
        
    }
    public void GenerateOptionButton(bool ItIsBox, UnityAction<int> addEvent)
    {
        if (temButton[0])
        {
            DestroyButton();
        }
        for (int i = 0; i < 3; i++)
        {
            temButton[i] = Instantiate(optionButtonPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Button;
            int index = i;
            if (i == 0 && !ItIsBox) // ���� �� ��
            {
                temButton[i].onClick.AddListener(() => { addEvent(3); });
            } else
            {
                temButton[i].onClick.AddListener(() => { addEvent(index); });
            }
            //temButton[i].onClick.AddListener(() => { Debug.Log(index); });
            temButton[i].transform.SetParent(optionPanel.transform, false);
            switch(i)
            {

                case 0:
                    if(ItIsBox)
                    {
                        temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�����";
                    }
                    else
                    {
                        temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�� �ɾ��";
                    }
                    break;
                case 1:
                    temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�����ϰ� ��������";
                    break;
                case 2:
                    temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "��������";
                    break;
            }
        }
    }
    
    public void DestroyButton()
    {
        for (int i=0; i< 3; i++)
        {
            int index = i;
            Debug.Log("��ư" + index + "�� ����");
            Destroy(temButton[index].gameObject);
        }
    }
}
