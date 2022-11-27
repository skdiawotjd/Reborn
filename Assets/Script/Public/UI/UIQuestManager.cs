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
        //Debug.Log("UIQuest 시작 시 해야할 일 실행");
        questTextGenerate();
    }

    protected override void EndUI()
    {
        //Debug.Log("UIQuest 끝날 시 해야할 일 실행");
    }


    public void questTextGenerate()
    {
        GameObject a = Instantiate(questText, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        MainText = a.GetComponent<TextMeshProUGUI>();

        MainText.text = QuestManager.instance.todayQuest;
        MainText.transform.SetParent(Panel.transform);
    }
}
