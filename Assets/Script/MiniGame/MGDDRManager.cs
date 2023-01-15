using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MGDDRManager : MiniGameManager
{
    private int keyStack; // ���� ����� ������ Ű�� ����
    private int keyCount; // ���� ���� �˸°� ���� Ű�� ����
    private float playTime; // ���� ���� �ð�
    private bool gameActive = false; // ���� ���ɿ���
    private int[] RandomKey;
    private int round;
    private int maxRound;
    private int[] keyOfRound;
    private Vector3 managerTrans;
    [SerializeField]
    private Arrow arrow;
    private TextMeshProUGUI timeText;
    Arrow[] arrowArray;
    private Slider timeSlider;
    private GameObject temImage;
    private SpriteRenderer temSprite;
    [SerializeField]
    private GameObject MinigameClothImage;
    [SerializeField]
    private Sprite MinigameCloth2;
    [SerializeField]
    private Sprite MinigameCloth3;
    [SerializeField]
    private Sprite Tshirt1;
    [SerializeField]
    private Sprite Tshirt2;
    [SerializeField]
    private Sprite Tshirt3;
    [SerializeField]
    private Sprite Tshirt4;
    [SerializeField]
    private GameObject BookObject;
    [SerializeField]
    private GameObject recipeSelectButton;
    [SerializeField]
    private GameObject ingredientImage;
    private Sprite ingotImage;
    private Canvas DDRCanvas;
    private Image foundryPanel;
    private Image ingredientPanel;
    private Image productsImage;
    private TextMeshProUGUI productsImageText;
    private TextMeshProUGUI foundryText;
    private Image recipeButtonPanel;
    private Button foundryButton;
    private Button exitButton;
    private Transform recipeContents;
    private string itemNumberString;
    private string itemNumberChar;
    private List<GameObject> temButton;
    private List<GameObject> temImagePrefab;
    private List<string> ingredientNumberList;
    private bool canFoundry;
    private int index;
    private string productsNumber;
    private List<Dictionary<string, object>> foundryRecipeList;
    private Transform[] childList;

    private void Awake()
    {
        timeText = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        timeSlider = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Slider>();

        managerTrans = new Vector3(0, 0, 0);

        keyCount = 0;
        round = 0;

        keyOfRound = new int[5];
        maxRound = keyOfRound.Length - 1;
        keyOfRound[0] = 3;
        keyOfRound[1] = 4;
        keyOfRound[2] = 5;
        keyOfRound[3] = 5;
        keyOfRound[4] = 5;
        DDRCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        foundryPanel = DDRCanvas.transform.GetChild(8).GetComponent<Image>();
        ingredientPanel = foundryPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        productsImage = foundryPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        productsImageText = productsImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        foundryButton = foundryPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        foundryText = foundryPanel.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        recipeButtonPanel = foundryPanel.transform.GetChild(1).GetComponent<Image>();
        recipeContents = recipeButtonPanel.transform.GetChild(0).GetChild(0).GetChild(0);
        exitButton = foundryPanel.transform.GetChild(2).GetComponent<Button>();
        ingredientNumberList = new List<string>();
        temImagePrefab = new List<GameObject>();
        temButton = new List<GameObject>();

        childList = recipeContents.GetComponentsInChildren<Transform>();
    }

    protected override void Start()
    {
        base.Start();
        foundryRecipeList = CSVReader.Read("FoundryRecipe");
        foundryButton.onClick.AddListener(CallSetRound);
        exitButton.onClick.AddListener(ExitButtonPress);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PressKey(0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PressKey(1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PressKey(2);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PressKey(3);
            }
        }
    }

    public override void GameStart()
    {
        if (true) // if ������
        {
            Clearing("Ingredient");
            Clearing("Products");
            Clearing("Recipe");
            foundryPanel.gameObject.SetActive(true);
            for (int i = 0; i < temButton.Count; i++)
            {
                Destroy(temButton[i].gameObject);
            }
            RecipeLoad();
        }
    }
    private void RecipeLoad()
    {
        for (int i = 0; i < foundryRecipeList.Count; i++)
        {
            itemNumberString = foundryRecipeList[i]["RecipeNumber"].ToString();
            GenerateRecipe(itemNumberString);
        }
    }
    private void GenerateRecipe(string itemNumber)
    {
        temButton.Add(Instantiate(recipeSelectButton));
        temButton[temButton.Count - 1].transform.SetParent(recipeContents.transform);
        temButton[temButton.Count - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WhatIsItemName(itemNumber) + " ������ �����ϱ�";
        temButton[temButton.Count - 1].GetComponent<Button>().onClick.AddListener(() => PressRecipeButton(itemNumber));
    }
    public void PressRecipeButton(string itemNumber)
    {
        canFoundry = true;
        Clearing("Ingredient");
        for (int i = 0; i < foundryRecipeList.Count; i++)
        {
            if (foundryRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
            {
                switch(foundryRecipeList[i]["SourceImage"].ToString())
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

                for (int j = 0; j < foundryRecipeList[i].Count - 6; j++)
                {
                    index = j;
                    temImagePrefab.Add(Instantiate(ingredientImage));
                    temImagePrefab[index].transform.SetParent(ingredientPanel.transform);
                    //temImagePrefab.GetComponent<Image>().sprite = "��� 1�� �̹���";
                    ingredientNumberList.Add(foundryRecipeList[i]["Ingredient" + index].ToString());
                    temImagePrefab[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingredientNumberList[j].Substring(0, 4) + " " + ingredientNumberList[j][4] + "��";
                    if (canFoundry)
                    {
                        if (Character.instance.MyItemManager.IsExistItem(ingredientNumberList[index].Substring(0, 4)))
                        {
                            canFoundry = Character.instance.MyItemManager.CanDeleteItem(ingredientNumberList[index].Substring(0, 4) + "-" + ingredientNumberList[index][4]);
                        }
                        else
                        {
                            canFoundry = false;
                        }
                    }
                }
            }
        }
        if (canFoundry)
        {
            foundryButton.interactable = true;
            foundryText.text = "���� ����";
            foundryText.color = Color.white;
        }
        else
        {
            foundryButton.interactable = false;
            foundryText.text = "���� �Ұ�";
            foundryText.color = Color.red;
        }
    }
    private string WhatIsItemName(string itemNumber)
    {
        for (int i = 0; i < foundryRecipeList.Count; i++)
        {
            if (foundryRecipeList[i]["RecipeNumber"].ToString() == itemNumber)
            {
                productsNumber = foundryRecipeList[i]["ItemNumber"].ToString();
                return foundryRecipeList[i]["ItemName"].ToString();
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
                foundryText.text = string.Empty;
                foundryButton.interactable = false;
                break;
            case "Recipe":
                if(childList != null)
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
        if (true) // if ������
        {
            for (int i = 0; i < ingredientNumberList.Count; i++)
            {
                Character.instance.SetCharacterStat(CharacterStatType.MyItem, ingredientNumberList[i].Substring(0, 4) + "-" + ingredientNumberList[i][4]);
            }
            Character.instance.SetCharacterStat(CharacterStatType.MyItem, productsNumber + "1");
        }

        timeSlider.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        Destroy(temImage);
        gameActive = false;
        Character.instance.SetCharacterInput(true, true, true);
        QuestManager.instance.QuestClear(true);
    }
    public override void SetRound(int nextRound)
    {
        SetMainWork(keyOfRound[nextRound]);
        Generate();
        gameActive = true;
    }
    public override void SetGame() // MinigameDdr()
    {
        playTime = 60.0f; // �÷��� Ÿ���� ���Ѵ�
        timeSlider.maxValue = playTime; // �÷��� Ÿ�ӿ� �°� �ð� ���α׷��� ���� �ִ밪�� �����ش�
        if (Character.instance.MyMapNumber == "0002")
        {
            temImage = Instantiate(MinigameClothImage) as GameObject;
            temSprite = temImage.GetComponent<SpriteRenderer>();
        }
        else
        {
            //temImage = Instantiate(BookObject) as GameObject;
        }
    }
    public override void SetMainWork(int key) // setKey()
    {
        keyStack = key; // �̹� ������ Ű ������ ��������
        managerTrans.x = -(key - 1);
        gameObject.transform.position = managerTrans;
        RandomKey = new int[keyStack];  // Ű ������ŭ �迭�� ������ش�
        arrowArray = new Arrow[keyStack]; // Ű ������ŭ �ַο찡 �� �迭�� ������ش�

        for (int i = 0; i < keyStack; i++) // �迭�ȿ� 0~3���� ���� ���� / 0 = L , 1 = R , 2 = U, 3 = D
        {
            RandomKey[i] = Random.Range(0, 4);
        }
    }
    public override void Generate()
    {
        for (int i = 0; i < arrowArray.Length; i++)
        {
            arrowArray[i] = Instantiate(arrow, new Vector3(transform.position.x + 2f * i, transform.position.y, transform.position.z), Quaternion.identity) as Arrow;
            // �θ� ������Ʈ ����. �θ��� transform�� �ޱ� ����
            switch(Character.instance.MyMapNumber)
            {
                case "0002":
                    if(RandomKey[i] == 0)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt1;
                    }
                    else if(RandomKey[i] == 1)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt2;
                    }
                    else if(RandomKey[i] == 2)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt3;
                    }
                    else if (RandomKey[i] == 3)
                    {
                        arrowArray[i].GetComponent<SpriteRenderer>().sprite = Tshirt4;
                    }
                    arrowArray[i].transform.localScale = Vector3.one;
                    break;
                default:
                    arrowArray[i].transform.Rotate(0, 0, 90 * RandomKey[i]);
                    break;
            }
        }
    }
    public override void PressKey(int key)
    {
        if (gameActive) // ddr ����
        {
            if (RandomKey[keyCount] == key)
            {
                if (RandomKey[keyCount] == key) // Ű �ڽ��� ���ڿ� ���� Ű�� ��ġ����
                {
                    if (keyCount != keyStack) // Ű ī��Ʈ�� �ִ밪�� �ƴ� ��
                    {
                        arrowArray[keyCount].ArrowAnim();
                        keyCount++; // Ű ī��Ʈ�� ������Ų��
                        if (keyCount == keyStack) // Ű ī��Ʈ�� �ִ뿡 ���� ���� ��
                        {
                            keyCount = 0;
                            if (round++ != maxRound)
                            {
                                if (Character.instance.MyMapNumber == "0002")
                                {
                                    if (round == maxRound)
                                    {
                                        temSprite.sprite = MinigameCloth3;
                                    }
                                    else
                                    {
                                        temSprite.sprite = MinigameCloth2;
                                    }
                                }
                                SetRound(round);
                            }
                            else
                            {
                                round = 0;
                                GameEnd(true);
                            }
                        }
                    }
                }
            }
            else if (playTime > 5)
            {
                Debug.Log("�� �� ����");
                playTime -= 5;
            }
            else
            {
                playTime = 0;
                timeSlider.value = playTime;
                for(int i = keyCount; i < keyStack; i++)
                {
                    arrowArray[i].DestroyImage();
                }
                GameEnd(false);
            }
        }
    }
    public void CallSetRound()
    {
        foundryPanel.gameObject.SetActive(false);
        timeSlider.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        SetGame();
        SetRound(0);
        StartCoroutine("CountTime", 0.1);
    }
    public void ExitButtonPress()
    {
        foundryPanel.gameObject.SetActive(false);
        Character.instance.SetCharacterInput(true, true, true);
    }
    public override IEnumerator CountTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (playTime > 0f && gameActive) // ddr ���� ������
        {
            playTime -= 0.1f;
            timeSlider.value = playTime;
            StartCoroutine("CountTime", 0.1f);
        }
    }
}
