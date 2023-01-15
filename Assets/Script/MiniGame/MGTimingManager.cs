using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MGTimingManager : MiniGameManager
{
    private Slider timingSlider;
    private Good good;
    private float timingValue;
    private bool timingChangeDirection;
    private bool timingGameActive = false;

    private Canvas timingCanvas;
    private Image perfectFloor;
    private float randomNumber;
    private int temNumber;
    private int timingRound;
    [SerializeField]
    private GameObject Anvil;
    private GameObject temImage;
    private Image manufacturePanel;
    private Image ingredientPanel;
    private Image productsImage;
    private TextMeshProUGUI productsImageText;
    private TextMeshProUGUI manufactureText;
    private Image recipeButtonPanel;
    private Button manufactureButton;
    private Button exitButton;
    private Transform recipeContents;
    [SerializeField]
    private GameObject ingredientImage;
    [SerializeField]
    private GameObject recipeSelectButton;
    [SerializeField]
    private Sprite oldArmorImage;
    private Sprite oldSwordImage;
    private string itemNumberString;
    private string itemNumberChar;
    private List<GameObject> temButton;
    private List<GameObject> temImagePrefab;
    private List<string> ingredientNumberList;
    private bool canManufacture;
    private int index;
    private string productsNumber;
    private List<Dictionary<string, object>> ManufactureRecipeList;
    private Transform[] childList;

    private void Awake()
    {
        timingCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        good = timingCanvas.transform.GetChild(4).GetComponent<Good>();
        perfectFloor = timingCanvas.transform.GetChild(3).GetChild(2).GetComponent<Image>();
        timingSlider = timingCanvas.transform.GetChild(3).GetComponent<Slider>();
        manufacturePanel = timingCanvas.transform.GetChild(8).GetComponent<Image>();
        ingredientPanel = manufacturePanel.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        productsImage = manufacturePanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        productsImageText = productsImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        manufactureButton = manufacturePanel.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        manufactureText = manufacturePanel.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        recipeButtonPanel = manufacturePanel.transform.GetChild(1).GetComponent<Image>();
        recipeContents = recipeButtonPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        exitButton = manufacturePanel.transform.GetChild(2).GetComponent<Button>();
        ingredientNumberList = new List<string>();
        temImagePrefab = new List<GameObject>();
        temButton = new List<GameObject>();
        oldSwordImage = Resources.Load<Sprite>("oldSword");

        childList = recipeContents.GetComponentsInChildren<Transform>();
    }

    protected override void Start()
    {
        base.Start();
        ManufactureRecipeList = CSVReader.Read("ManufactureRecipe");
        manufactureButton.onClick.AddListener(CallSetRound);
        exitButton.onClick.AddListener(ExitButtonPress);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timingGameActive)
        {
            // 타이밍맞추기
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timingGameActive = false;
                StopCoroutine("ChangeTimingValue");
                good.gameObject.SetActive(true);
                if (timingSlider.value > (randomNumber - 1.5f) && timingSlider.value < (randomNumber + 1.5f))
                {
                    good.ChangeSource(true);
                    Debug.Log("명중");
                }
                else
                {
                    good.ChangeSource(false);
                    Debug.Log("실패");
                }
                StartCoroutine("CountTime", 0.5f);
                timingRound++;
                if (timingRound < 5)
                {
                    SetMainWork();
                }
                else if (timingRound == 5)
                {
                    GameEnd(true);
                }
            }
        }
    }

    public override void GameStart()
    {
        if(true) // if 제작이면
        {
            Clearing("Ingredient");
            Clearing("Products");
            Clearing("Recipe");
            manufacturePanel.gameObject.SetActive(true);
            for(int i = 0; i < temButton.Count; i++)
            {
                Destroy(temButton[i].gameObject);
            }
            RecipeLoad();
        }
    }
    private void RecipeLoad()
    {
        for (int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            itemNumberString = Character.instance.MyItem[i].Substring(0, 4);
            itemNumberChar = itemNumberString.Substring(0, 1);
            if (int.Parse(itemNumberChar) == 3)
            {
                GenerateRecipe(itemNumberString);
            }
        }
    }
    private void GenerateRecipe(string itemNumber)
    {
        temButton.Add(Instantiate(recipeSelectButton));
        temButton[temButton.Count - 1].transform.SetParent(recipeContents.transform);
        temButton[temButton.Count - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WhatIsItemName(itemNumber) + " 아이템 제작하기";
        temButton[temButton.Count - 1].GetComponent<Button>().onClick.AddListener(() => PressRecipeButton(itemNumber));
    }
    public void PressRecipeButton(string itemNumber)
    {
        canManufacture = true;
        Clearing("Ingredient");
        for (int i = 0; i < ManufactureRecipeList.Count; i++)
        {
            if (ManufactureRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
            {
                if(ManufactureRecipeList[i]["SourceImage"].ToString() == "oldsword")
                {
                    productsImage.sprite = oldSwordImage;
                }
                else
                {
                    productsImage.sprite = oldArmorImage;
                }
                productsImageText.text = ManufactureRecipeList[i]["ItemName"].ToString();

                for(int j = 0; j < ManufactureRecipeList[i].Count-6; j++)
                {
                    index = j;
                    temImagePrefab.Add(Instantiate(ingredientImage));
                    temImagePrefab[index].transform.SetParent(ingredientPanel.transform);
                    //temImagePrefab.GetComponent<Image>().sprite = "재료 1번 이미지";
                    ingredientNumberList.Add(ManufactureRecipeList[i]["Ingredient" + index].ToString());
                    temImagePrefab[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingredientNumberList[j].Substring(0, 4) + " " + ingredientNumberList[j][4] + "개";
                    if(canManufacture)
                    {
                        if (Character.instance.MyItemManager.IsExistItem(ingredientNumberList[index].Substring(0, 4)))
                        {
                            canManufacture = Character.instance.MyItemManager.CanDeleteItem(ingredientNumberList[index].Substring(0, 4) + "-" + ingredientNumberList[index][4]);
                        }
                        else
                        {
                            canManufacture = false;
                        }
                    }
                }
            }
        }
        if(canManufacture)
        {
            manufactureButton.interactable = true;
            manufactureText.text = "제작 가능";
            manufactureText.color = Color.white;
        } else
        {
            manufactureButton.interactable = false;
            manufactureText.text = "제작 불가";
            manufactureText.color = Color.red;
        }
    }
    private string WhatIsItemName(string itemNumber)
    {
        for(int i = 0; i < ManufactureRecipeList.Count; i++)
        {
            if (ManufactureRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
            {
                productsNumber = ManufactureRecipeList[i]["ItemNumber"].ToString();
                return ManufactureRecipeList[i]["ItemName"].ToString();
            }
        }
        return null;
    }
    private void Clearing(string type)
    {
        switch (type)
        {
            case "Ingredient":
                for (int i = 0; i < temImagePrefab.Count; i++)
                {
                    Destroy(temImagePrefab[i].gameObject);
                }
                ingredientNumberList.Clear();
                temImagePrefab.Clear();
                break;
            case "Products":
                productsImage.sprite = null;
                productsImageText.text = string.Empty;
                manufactureText.text = string.Empty;
                manufactureButton.interactable = false;
                break;
            case "Recipe":
                if (childList != null)
                {
                    for (int i = 1; i < childList.Length; i++)
                    {
                        if (childList[i] != transform)
                        {
                            Destroy(childList[i].gameObject);
                        }
                    }
                }
                break;
        }
    }
    public override void GameEnd(bool clear)
    {
        if (true) // if 제작이면
        {
            for (int i = 0; i < ingredientNumberList.Count; i++)
            {
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, ingredientNumberList[i].Substring(0, 4) + "-" + ingredientNumberList[i][4]);
            }
            Character.instance.SetCharacterStat(CharacterStatType.MyItem, productsNumber + "1");
        }
        QuestManager.instance.QuestClear(true);
        timingSlider.gameObject.SetActive(false);
        if (Character.instance.MyMapNumber == "0003")
        {
            Destroy(temImage);
        }
        timingGameActive = false;
        Character.instance.SetCharacterInput(true, true, true);
    }
    public override void SetRound(int num) // SetTimingRound()
    {
        timingValue = 0f;
        timingChangeDirection = true;
        timingGameActive = true;
        timingRound = 0;
/*        if (Character.instance.MyMapNumber == "0004")
        {
            temImage = Instantiate(Anvil) as GameObject;
        }*/
        SetMainWork();
    }
    public override void SetMainWork() // SetTimingPosition()
    {
        if (timingRound < 5)
        {
            randomNumber = Random.Range(2.0f, 8.0f); // 2에서 8까지 랜덤 넘버를 잡는다 >> 타이밍 카운터가 움직일 범위
            temNumber = (int)(randomNumber * 100f);
            perfectFloor.rectTransform.anchoredPosition = new Vector3(temNumber, perfectFloor.rectTransform.anchoredPosition.y);
            timingValue = 0;
            timingSlider.value = timingValue;
            timingChangeDirection = true;

            timingSlider.gameObject.SetActive(true);
            timingGameActive = true;
            StartCoroutine("ChangeTimingValue", 0.1f);
        }
    }
    public override IEnumerator ChangeTimingValue(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (timingGameActive)
        {
            if (timingChangeDirection && timingValue < 10)
            {
                timingSlider.value = timingValue;
                timingValue += 0.2f;
                StartCoroutine("ChangeTimingValue", 0.0075f);
            }
            else if (!timingChangeDirection && timingValue > 0)
            {
                timingSlider.value = timingValue;
                timingValue -= 0.2f;
                StartCoroutine("ChangeTimingValue", 0.0075f);
            }
            else
            {
                timingChangeDirection = !timingChangeDirection;
                StartCoroutine("ChangeTimingValue", 0.0075f);
                //timingGameActive = false;
            }
        }
    }
    public void CallSetRound()
    {
        manufacturePanel.gameObject.SetActive(false);
        SetRound(5);
    }
    public void ExitButtonPress()
    {
        manufacturePanel.gameObject.SetActive(false);
        Character.instance.SetCharacterInput(true, true, true);
    }
    public override IEnumerator CountTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }
}
