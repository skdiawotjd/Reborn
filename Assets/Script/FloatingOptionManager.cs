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
            if (i == 0 && !ItIsBox) // 몬스터 일 시
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
                        temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "열어본다";
                    }
                    else
                    {
                        temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "말 걸어본다";
                    }
                    break;
                case 1:
                    temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "무시하고 지나간다";
                    break;
                case 2:
                    temButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "도망간다";
                    break;
            }
        }
    }
    
    public void DestroyButton()
    {
        for (int i=0; i< 3; i++)
        {
            int index = i;
            Debug.Log("버튼" + index + "번 삭제");
            Destroy(temButton[index].gameObject);
        }
    }
}
