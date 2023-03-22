using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettingManager : UIManager
{
    [SerializeField]
    private List<Button> SettingButton;
    [SerializeField]
    private List<GameObject> SettingPanel;
    [SerializeField]
    private List<Toggle> SettingToggle;
    [SerializeField]
    private List<Slider> SettingSlider;

    [SerializeField]
    private GameObject LoadButtonItemConent;

    private int CurSaveDataCount;

    void Awake()
    {
        CurSaveDataCount = 0;
    }

    protected override void Start()
    {
        base.Start();

        GameManager.instance.AddGenerateGameEvent(SetLoadExitButton);
        GameManager.instance.AddLoadEvent(SetActivePanel);
        InitializeSettingManager();
    }

    protected override void StartUI()
    {
        //Debug.Log("UISetting ���� �� �ؾ��� �� ����");
    }

    protected override void EndUI()
    {
        //Debug.Log("UISetting ���� �� �ؾ��� �� ����");
    }

    public override void SetActivePanel()
    {
        for(int i = 0; i < SettingPanel.Count; i++)
        {
            if (SettingPanel[i].activeSelf)
            {
                SettingPanel[i].SetActive(false);
                return;
            }
        }
        base.SetActivePanel();
    }

    private void InitializeSettingManager()
    {
        // �ʱ�ȭ
        // ���� ũ�⿡ �°� UI�� ����
        foreach (int asd in Enum.GetValues(typeof(UISoundOrder)))
        {
            SettingToggle[(int)asd].isOn = SoundManager.instance.SourceMute(UISoundOrder.Background);
            SettingSlider[(int)asd].value = SoundManager.instance.SourceVolume(UISoundOrder.Background);
        }

        // ���
        // �Ͻ�����
        SettingButton[(int)UISettingButtonOrder.Pause].onClick.AddListener(() => { SetPause(false); });
        // �簳�ϱ�
        SettingButton[(int)UISettingButtonOrder.Resume].onClick.AddListener(() => { SetPause(true); });
        // �����ϱ�
        SettingButton[(int)UISettingButtonOrder.Save].onClick.AddListener(() => { GameManager.instance.SaveData(); });
        // �ҷ�����
        SettingButton[(int)UISettingButtonOrder.Load].onClick.AddListener(() => { CheckLoadButton(); });

        // Background Mute
        SettingToggle[(int)UISoundOrder.Background].onValueChanged.AddListener((value) => { SoundManager.instance.MuteAudioSource(UISoundOrder.Background, value); });
        // Effect Mute
        SettingToggle[(int)UISoundOrder.Effect].onValueChanged.AddListener((value) => { SoundManager.instance.MuteAudioSource(UISoundOrder.Effect, value); });
        // Background Volume
        SettingSlider[(int)UISoundOrder.Background].onValueChanged.AddListener((value) => { SoundManager.instance.VolumeAudioSource(UISoundOrder.Background, value); });
        // Effect Volume
        SettingSlider[(int)UISoundOrder.Background].onValueChanged.AddListener((value) => { SoundManager.instance.VolumeAudioSource(UISoundOrder.Background, value); });
        
        // �޴��� ����
        SettingButton[(int)UISettingButtonOrder.BackOk].onClick.AddListener(() => { MoveToStartScene(); });

        // ����
        // �� â ����
        foreach (int Order in Enum.GetValues(typeof(UISettingPanelOrder)))
        {
            //Debug.Log(SettingButton[Order + 1].name + "�� " + (UISettingPanelOrder)Order + "�� ����");
            SettingButton[Order + 1].onClick.AddListener(() => { SetActivePanel((UISettingPanelOrder)Order); });
        }

        // �ݱ�
        //  ���� â �ݱ�
        SettingButton[(int)UISettingButtonOrder.SettingClose].onClick.AddListener(SetActivePanel);
        // ���� â �ݱ�
        SettingButton[(int)UISettingButtonOrder.Save].onClick.AddListener(() => { StartCoroutine(ClosePanelTimer()); });
        //  �Ͻ����� â �ݱ�
        SettingButton[(int)UISettingButtonOrder.Resume].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Pause); });
        // �ε� â �ݱ�
        //SettingButton[(int)UISettingButtonOrder.LoadClose].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Load); });
        SettingButton[(int)UISettingButtonOrder.LoadClose].onClick.AddListener(StartLoadExitButton);
        // ���� â �ݱ�
        SettingButton[(int)UISettingButtonOrder.SoundClose].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Sound); });
        // ���� â �ݱ�
        SettingButton[(int)UISettingButtonOrder.BackCancel].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Back); });
    }

    // â ���ݱ� ���
    public void SetActivePanel(UISettingPanelOrder Type)
    {
        SettingPanel[(int)Type].SetActive(!SettingPanel[(int)Type].activeSelf);
    }

    // ���� ���
    private void SetPause(bool CharacterInput)
    {
        Character.instance.SetCharacterInput(CharacterInput, CharacterInput);
        GameManager.instance.ActivateDay();
    }

    // ���� �ڷ�ƾ
    IEnumerator ClosePanelTimer()
    {
        yield return new WaitForSeconds(0.5f);

        if (SettingPanel[(int)UISettingPanelOrder.Save].activeSelf)
        {
            SetActivePanel(UISettingPanelOrder.Save);
        }
    }


    // �ҷ����� ���
    public void CheckLoadButton()
    {
        GameManager.instance.SetSaveDataCount();
        //Debug.Log(".json�� " + GameManager.instance.SaveDataCount + "�� ����");
        // ���� ���� ������ ���� ���� ���嵥���� ���� �ٸ���
        //Debug.Log("pre " + CurSaveDataCount + " " + GameManager.instance.SaveDataCount);
        if (CurSaveDataCount != GameManager.instance.SaveDataCount)
        {
            // ��ư�� ���� ����
            for (; CurSaveDataCount < GameManager.instance.SaveDataCount; CurSaveDataCount++)
            {
                /*GameObject NewLoadButton = Instantiate(Resources.Load("Public/LoadButton")) as GameObject;
                NewLoadButton.transform.SetParent(SettingPanel[(int)UISettingPanelOrder.Load].transform, false);
                int SaveDataButtonCount = CurSaveDataCount;
                NewLoadButton.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.LoadData(SaveDataButtonCount); });
                NewLoadButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CurSaveDataCount.ToString();
                NewLoadButton.GetComponent<RectTransform>().anchoredPosition = new Vector2((NewLoadButton.GetComponent<RectTransform>().anchoredPosition.x + (200 * CurSaveDataCount)), NewLoadButton.GetComponent<RectTransform>().anchoredPosition.y);*/


                GameObject NewLoadButton = Instantiate(Resources.Load("UI/LoadButton")) as GameObject;
                NewLoadButton.transform.SetParent(LoadButtonItemConent.transform, false);
                NewLoadButton.GetComponent<LoadButtonManager>().SetLoadButtonData(GameManager.instance.LoadSaveList(CurSaveDataCount));
                int SaveDataButtonCount = CurSaveDataCount;
                NewLoadButton.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.LoadData(SaveDataButtonCount); });
                NewLoadButton.GetComponent<Button>().onClick.AddListener(() => { SetLoadExitButton(); });
            }
        }
        //Debug.Log("pro " + CurSaveDataCount + " " + GameManager.instance.SaveDataCount);
    }

    private void MoveToStartScene()
    {
        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "");
        SceneManager.LoadScene("Start");

        SetActivePanel(UISettingPanelOrder.Back);
        SetActivePanel();
    }

    public void StartLoadExitButton()
    {
        SetActivePanel(UISettingPanelOrder.Load);
        SetActivePanel();
        Panel.transform.parent.gameObject.SetActive(!Panel.transform.parent.gameObject.activeSelf);
    }

    private void SetLoadExitButton()
    {
        SettingButton[(int)UISettingButtonOrder.LoadClose].onClick.RemoveAllListeners();
        SettingButton[(int)UISettingButtonOrder.LoadClose].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Load); });
    }
}
