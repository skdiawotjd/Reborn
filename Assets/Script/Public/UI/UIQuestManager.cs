using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestManager : UIManager
{
    [SerializeField]
    private Object questText;
    private TextMeshProUGUI MainText;

    protected override void Start()
    {
        base.Start();
    }

    protected override void StartUI()
    {
        //Debug.Log("UIQuest ���� �� �ؾ��� �� ����");
        questTextGenerate();
    }

    protected override void EndUI()
    {
        //Debug.Log("UIQuest ���� �� �ؾ��� �� ����");
    }


    public void questTextGenerate()
    {
        GameObject a = Instantiate(questText, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        MainText = a.GetComponent<TextMeshProUGUI>();

        MainText.text = QuestManager.instance.todayQuest;
        MainText.transform.SetParent(Panel.transform);
    }
}
