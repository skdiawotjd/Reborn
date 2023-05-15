using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFrameWorkManager : MonoBehaviour
{
    MiniGameManager MinigameManager;

    private Canvas minigameCanvas;
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
    private List<Dictionary<string, object>> foundryRecipeList;
    private Transform[] childList;
    private string minigameManagerType;
    // Timing
    public Image perfectFloor;
    public Slider timingSlider;
    public Good good;
    // DDR
    private Sprite ingotImage;

    void Start()
    {
        switch(Character.instance.MyMapNumber)
        {
            case "0003":
            case "0004":
            case "0104":
                Initialize();
                break;
        }
    }

    void Update()
    {
        
    }

    private void RecipeLoad()
    {
        switch(minigameManagerType)
        {
            case "DDR":
                for (int i = 0; i < foundryRecipeList.Count; i++)
                {
                    itemNumberString = foundryRecipeList[i]["RecipeNumber"].ToString();
                    GenerateRecipe(itemNumberString);
                }
                break;
            case "Timing":
                for (int i = 0; i < Character.instance.MyItem.Count; i++)
                {
                    itemNumberString = Character.instance.MyItem[i].Substring(0, 4);
                    itemNumberChar = itemNumberString.Substring(0, 1);
                    if (int.Parse(itemNumberChar) == 3)
                    {
                        GenerateRecipe(itemNumberString);
                    }
                }
                break;
        }
    }
    private void GenerateRecipe(string itemNumber)
    {
        switch (minigameManagerType)
        {
            case "DDR":
            case "Timing":
                temButton.Add(Instantiate(recipeSelectButton));
                temButton[temButton.Count - 1].transform.SetParent(recipeContents.transform);
                temButton[temButton.Count - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WhatIsItemName(itemNumber) + " 아이템 제작하기";
                temButton[temButton.Count - 1].GetComponent<Button>().onClick.AddListener(() => PressRecipeButton(itemNumber));
                break;
        }
    }
    public void PressRecipeButton(string itemNumber)
    {
        switch (minigameManagerType)
        {
            case "DDR":
                canManufacture = true;
                Clearing("Ingredient");
                for (int i = 0; i < foundryRecipeList.Count; i++)
                {
                    if (foundryRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
                    {
                        switch (foundryRecipeList[i]["SourceImage"].ToString())
                        {
                            case "ironingot":
                            case "bronzeingot":
                            case "silveringot":
                            case "goldingot":
                            case "platinumingot":
                                productsImage.sprite = ingotImage;
                                break;

                        }
                        productsImageText.text = foundryRecipeList[i]["ItemName"].ToString();
                        productsNumber = foundryRecipeList[i]["ItemNumber"].ToString();
                        for (int j = 0; j < foundryRecipeList[i].Count - 6; j++)
                        {
                            index = j;
                            temImagePrefab.Add(Instantiate(ingredientImage));
                            temImagePrefab[index].transform.SetParent(ingredientPanel.transform);
                            //temImagePrefab.GetComponent<Image>().sprite = "재료 1번 이미지";
                            ingredientNumberList.Add(foundryRecipeList[i]["Ingredient" + index].ToString());
                            temImagePrefab[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingredientNumberList[j].Substring(0, 4) + " " + ingredientNumberList[j][4] + "개";
                            if (canManufacture)
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
                if (canManufacture)
                {
                    manufactureButton.interactable = true;
                    manufactureText.text = "제작 가능";
                    manufactureText.color = Color.white;
                }
                else
                {
                    manufactureButton.interactable = false;
                    manufactureText.text = "제작 불가";
                    manufactureText.color = Color.red;
                }
                break;
            case "Timing":
                canManufacture = true;
                Clearing("Ingredient");
                for (int i = 0; i < ManufactureRecipeList.Count; i++)
                {
                    if (ManufactureRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
                    {
                        if (ManufactureRecipeList[i]["SourceImage"].ToString() == "oldsword")
                        {
                            productsImage.sprite = oldSwordImage;
                        }
                        else
                        {
                            productsImage.sprite = oldArmorImage;
                        }
                        productsImageText.text = ManufactureRecipeList[i]["ItemName"].ToString();

                        for (int j = 0; j < ManufactureRecipeList[i].Count - 6; j++)
                        {
                            index = j;
                            temImagePrefab.Add(Instantiate(ingredientImage));
                            temImagePrefab[index].transform.SetParent(ingredientPanel.transform);
                            //temImagePrefab.GetComponent<Image>().sprite = "재료 1번 이미지";
                            temImagePrefab[index].GetComponent<Image>().sprite = ImageManager.instance.GetImage(ManufactureRecipeList[i]["Ingredient" + index].ToString().Substring(0, 4));
                            /*if (ManufactureRecipeList[i]["Ingredient"+ index].ToString().Substring(0,4) == "1100")
                            {
                                temImagePrefab[index].GetComponent<Image>().sprite = ImageManager.instance.GetImage("1100");
                            }
                            if (ManufactureRecipeList[i]["Ingredient" + index].ToString().Substring(0, 4) == "1101")
                            {
                                temImagePrefab[index].GetComponent<Image>().sprite = ImageManager.instance.GetImage("1101");
                            }*/
                            ingredientNumberList.Add(ManufactureRecipeList[i]["Ingredient" + index].ToString());
                            temImagePrefab[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingredientNumberList[j].Substring(0, 4) + " " + ingredientNumberList[j][4] + "개";
                            if (canManufacture)
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
                if (canManufacture)
                {
                    manufactureButton.interactable = true;
                    manufactureText.text = "제작 가능";
                    manufactureText.color = Color.white;
                }
                else
                {
                    manufactureButton.interactable = false;
                    manufactureText.text = "제작 불가";
                    manufactureText.color = Color.red;
                }
                break;
        }
        
    }
    private string WhatIsItemName(string itemNumber)
    {
        switch (minigameManagerType)
        {
            case "DDR":
                for (int i = 0; i < foundryRecipeList.Count; i++)
                {
                    if (foundryRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
                    {
                        productsNumber = foundryRecipeList[i]["ItemNumber"].ToString();
                        return foundryRecipeList[i]["ItemName"].ToString();
                    }
                }
                return null;
            case "Timing":
                for (int i = 0; i < ManufactureRecipeList.Count; i++)
                {
                    if (ManufactureRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
                    {
                        productsNumber = ManufactureRecipeList[i]["ItemNumber"].ToString();
                        return ManufactureRecipeList[i]["ItemName"].ToString();
                    }
                }
                return null;
        }
        return null;
    }
    private void Clearing(string type)
    {
        switch (minigameManagerType)
        {
            case "DDR":
            case "Timing":
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
                break;
        }
        
    }
    public void GameStart()
    {
        switch (minigameManagerType)
        {
            case "DDR":
            case "Timing":
                Clearing("Ingredient");
                Clearing("Products");
                Clearing("Recipe");
                manufacturePanel.gameObject.SetActive(true);
                MinigameManager.SetPanelActive(true);
                for (int i = 0; i < temButton.Count; i++)
                {
                    Destroy(temButton[i].gameObject);
                }
                RecipeLoad();
                break;
        }
    }
    public void GameEnd()
    {
        switch (minigameManagerType)
        {
            case "DDR":
            case "Timing":
                for (int i = 0; i < ingredientNumberList.Count; i++)
                {
                    Debug.Log(i + "번째 : " + ingredientNumberList[i].Substring(0, 4));
                    Debug.Log(i + "번째 : " + ingredientNumberList[i][4]);
                    Character.instance.SetCharacterStat(CharacterStatType.MyItem, ingredientNumberList[i].Substring(0,4) + "-" + ingredientNumberList[i][4]);
                }
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, productsNumber + "1");
                break;
            default:
                Debug.Log("error");
                break;
        }
    }
    public void ExitButtonPress()
    {
        manufacturePanel.gameObject.SetActive(false);
        MinigameManager.SetPanelActive(false);
        Character.instance.SetCharacterInput(true, true, true);
    }
    public void CallSetRound()
    {
        switch (minigameManagerType)
        {
            case "DDR":
                manufacturePanel.gameObject.SetActive(false);
                MinigameManager.SetPanelActive(false);
                MinigameManager.SetGame();
                MinigameManager.SetRound(0);
                break;
            case "Timing":
                manufacturePanel.gameObject.SetActive(false);
                MinigameManager.SetPanelActive(false);
                MinigameManager.SetRound(5);
                break;
        }
    }
    private void Initialize()
    {
        MinigameManager = GameObject.FindWithTag("MinigameManager").GetComponent<MiniGameManager>();
        switch (MinigameManager.GetGameType())
        {
            case 3:
                minigameManagerType = "DDR";
                break;
            case 4:
                minigameManagerType = "Timing";
                break;
            case 5:
                minigameManagerType = "Quiz";
                break;
        }
        switch (minigameManagerType)
        {
            case "DDR": // DDR
            case "Timing": // Timing
                manufacturePanel = gameObject.transform.GetChild(0).GetComponent<Image>();
                minigameCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
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
                childList = recipeContents.GetComponentsInChildren<Transform>();
                if (minigameManagerType == "DDR")
                {
                    foundryRecipeList = CSVReader.Read("FoundryRecipe");
                }else if(minigameManagerType ==  "Timing")
                {
                    ManufactureRecipeList = CSVReader.Read("ManufactureRecipe");
                    oldSwordImage = Resources.Load<Sprite>("oldSword");
                }
                manufactureButton.onClick.AddListener(CallSetRound);
                exitButton.onClick.AddListener(ExitButtonPress);
                break;
            case "Quiz":
                break;
        }
    }
    public void SetDisabledPanel()
    {
        manufacturePanel.gameObject.SetActive(false);
        MinigameManager.SetPanelActive(false);
    }
}
