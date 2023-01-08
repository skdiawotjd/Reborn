using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField]
    private Image MainQuestPanel;
    [SerializeField]
    private Image SubQuestPanel;
    [SerializeField]
    private Object questText;
    private TextMeshProUGUI MainText;

    void Start()
    {
        questTextGenerate();
    }

    public void questTextGenerate()
    {
        GameObject a = Instantiate(questText, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        MainText = a.GetComponent<TextMeshProUGUI>();

        MainText.text = QuestManager.instance.todayQuest;
        //MainText.transform.SetParent(QuestPanel.transform); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
