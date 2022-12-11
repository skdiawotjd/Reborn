using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadButtonManager : MonoBehaviour
{
    //private enum LoadButtonOrder { Name, Age, PlayTime, Position};

    [SerializeField]
    private Image CharacterImage;
    [SerializeField]
    private List<TextMeshProUGUI> LoadButtonName;
    [SerializeField]
    private List<TextMeshProUGUI> LoadButtonData;

    public void SetLoadButtonData(string Data)
    {
        string[] Words = Data.Split(',');

        for (int i = 0; i < LoadButtonData.Count; i++)
        {
            LoadButtonData[i].text = Words[i + 1];
        }
    }
}
