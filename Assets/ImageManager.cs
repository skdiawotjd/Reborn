using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
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
        return imageDictionary[itemNumber];
    }

    void Update()
    {
        
    }
}
