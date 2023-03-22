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
        //Debug.Log("UISetting 시작 시 해야할 일 실행");
    }

    protected override void EndUI()
    {
        //Debug.Log("UISetting 끝날 시 해야할 일 실행");
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
        // 초기화
        // 사운드 크기에 맞게 UI값 조절
        foreach (int asd in Enum.GetValues(typeof(UISoundOrder)))
        {
            SettingToggle[(int)asd].isOn = SoundManager.instance.SourceMute(UISoundOrder.Background);
            SettingSlider[(int)asd].value = SoundManager.instance.SourceVolume(UISoundOrder.Background);
        }

        // 기능
        // 일시정지
        SettingButton[(int)UISettingButtonOrder.Pause].onClick.AddListener(() => { SetPause(false); });
        // 재개하기
        SettingButton[(int)UISettingButtonOrder.Resume].onClick.AddListener(() => { SetPause(true); });
        // 저장하기
        SettingButton[(int)UISettingButtonOrder.Save].onClick.AddListener(() => { GameManager.instance.SaveData(); });
        // 불러오기
        SettingButton[(int)UISettingButtonOrder.Load].onClick.AddListener(() => { CheckLoadButton(); });

        // Background Mute
        SettingToggle[(int)UISoundOrder.Background].onValueChanged.AddListener((value) => { SoundManager.instance.MuteAudioSource(UISoundOrder.Background, value); });
        // Effect Mute
        SettingToggle[(int)UISoundOrder.Effect].onValueChanged.AddListener((value) => { SoundManager.instance.MuteAudioSource(UISoundOrder.Effect, value); });
        // Background Volume
        SettingSlider[(int)UISoundOrder.Background].onValueChanged.AddListener((value) => { SoundManager.instance.VolumeAudioSource(UISoundOrder.Background, value); });
        // Effect Volume
        SettingSlider[(int)UISoundOrder.Background].onValueChanged.AddListener((value) => { SoundManager.instance.VolumeAudioSource(UISoundOrder.Background, value); });
        
        // 메뉴로 가기
        SettingButton[(int)UISettingButtonOrder.BackOk].onClick.AddListener(() => { MoveToStartScene(); });

        // 열기
        // 각 창 열기
        foreach (int Order in Enum.GetValues(typeof(UISettingPanelOrder)))
        {
            //Debug.Log(SettingButton[Order + 1].name + "에 " + (UISettingPanelOrder)Order + "를 달음");
            SettingButton[Order + 1].onClick.AddListener(() => { SetActivePanel((UISettingPanelOrder)Order); });
        }

        // 닫기
        //  설정 창 닫기
        SettingButton[(int)UISettingButtonOrder.SettingClose].onClick.AddListener(SetActivePanel);
        // 저장 창 닫기
        SettingButton[(int)UISettingButtonOrder.Save].onClick.AddListener(() => { StartCoroutine(ClosePanelTimer()); });
        //  일시정지 창 닫기
        SettingButton[(int)UISettingButtonOrder.Resume].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Pause); });
        // 로드 창 닫기
        //SettingButton[(int)UISettingButtonOrder.LoadClose].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Load); });
        SettingButton[(int)UISettingButtonOrder.LoadClose].onClick.AddListener(StartLoadExitButton);
        // 사운드 창 닫기
        SettingButton[(int)UISettingButtonOrder.SoundClose].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Sound); });
        // 사운드 창 닫기
        SettingButton[(int)UISettingButtonOrder.BackCancel].onClick.AddListener(() => { SetActivePanel(UISettingPanelOrder.Back); });
    }

    // 창 여닫기 기능
    public void SetActivePanel(UISettingPanelOrder Type)
    {
        SettingPanel[(int)Type].SetActive(!SettingPanel[(int)Type].activeSelf);
    }

    // 정지 기능
    private void SetPause(bool CharacterInput)
    {
        Character.instance.SetCharacterInput(CharacterInput, CharacterInput);
        GameManager.instance.ActivateDay();
    }

    // 저장 코루틴
    IEnumerator ClosePanelTimer()
    {
        yield return new WaitForSeconds(0.5f);

        if (SettingPanel[(int)UISettingPanelOrder.Save].activeSelf)
        {
            SetActivePanel(UISettingPanelOrder.Save);
        }
    }


    // 불러오기 기능
    public void CheckLoadButton()
    {
        GameManager.instance.SetSaveDataCount();
        //Debug.Log(".json이 " + GameManager.instance.SaveDataCount + "개 있음");
        // 실제 저장 데이터 수와 현재 저장데이터 수가 다르면
        //Debug.Log("pre " + CurSaveDataCount + " " + GameManager.instance.SaveDataCount);
        if (CurSaveDataCount != GameManager.instance.SaveDataCount)
        {
            // 버튼을 새로 생성
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
