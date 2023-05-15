using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    private List<Dictionary<string, object>> ItemList;
    private Dictionary<string, Sprite> imageDictionary = new Dictionary<string, Sprite>();
    private Sprite temSprite;
    public static ImageManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        ItemList = CSVReader.Read("CSV/ItemList");
    }
    void Start()
    {
        temSprite = Resources.Load<Sprite>("Image/Manufacture/1100");
        imageDictionary.Add(temSprite.name, temSprite);
        temSprite = Resources.Load<Sprite>("Image/Manufacture/1101");
        imageDictionary.Add(temSprite.name, temSprite);
    }

    public Sprite GetImage(string itemNumber)
    {
        return Resources.Load<Sprite>("Image/Manufacture/" + itemNumber);
        //return imageDictionary[itemNumber];
    }

    public string GetImage(string itemNumber, int Type)
    {
        int Order = 0;
        for( ; Order < ItemList.Count; Order++)
        {
            if(ItemList[Order]["ItemNumber"].ToString() == itemNumber)
            {
                break;
            }
        }

        switch(Type)
        {
            case 0:
                return ItemList[Order]["ItemNameKor"].ToString();
            case 1:
                return ItemList[Order]["ItemNameEng"].ToString();
            case 2:
                //return Resources.Load<Sprite>(ItemList[Order]["Path"].ToString());
                return ItemList[Order]["Path"].ToString();
        }

        return "";
    }
}
