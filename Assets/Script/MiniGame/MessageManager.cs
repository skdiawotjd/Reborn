using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI ContentsText;
    [SerializeField]
    public Image ContentsPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setMessagePanel(bool state)
    {
        ContentsPanel.gameObject.SetActive(state);
    }
}
