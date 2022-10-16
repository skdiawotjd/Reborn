using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TownManager : MonoBehaviour
{

    public string myPosition;
    private Image ChoicePanel;
    // Start is called before the first frame update
    void Start()
    {
        ChoicePanel = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Initialize()
    {

    }

    public void ChoiceButtonActive()
    {
        ChoicePanel.gameObject.SetActive(true);
    }
    public void DDRButton()
    {
        if (Character.instance.MyJob.ToString() == "Slayer")
        {
            Character.instance.SetCharacterStat(6, "0002"); // ddr
            TownSceneMove();
        }
    }
    public void TimingButton()
    {
        if (Character.instance.MyJob.ToString() == "Slayer")
        {
            Character.instance.SetCharacterStat(6, "0003"); // 타이밍
            TownSceneMove();
        }

    }
    public void ObjectButton()
    {
        if (Character.instance.MyJob.ToString() == "Slayer")
        {
            Character.instance.SetCharacterStat(6, "0005"); // 오브젝트
            TownSceneMove();
        }

    }
    public void TownSceneMove()
    {
        Character.instance.MyPlayerController.DisableCollider();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
    }
}
