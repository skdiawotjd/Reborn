using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] change_img;
    [SerializeField]
    private Image tradeListPanel;
    private List<Image> tradePanelList = new List<Image>();
    [SerializeField]
    private Image[] typePanel;
    [SerializeField]
    private Transform listViewContent;
    private Image temPanel;
    private int selectPanelOrder;
    private int currentPanelOrder;

    private List<Dictionary<string, object>> TradeItemList;
    void Start()
    {

        currentPanelOrder = 0;
        TradeItemList = CSVReader.Read("TradeItemList");
        GenerateTradeList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GenerateTradeList()
    {
        for(int i = 0; i < TradeItemList.Count; i++)
        {
            temPanel = Instantiate(tradeListPanel);
            temPanel.gameObject.transform.SetParent(listViewContent, false);
            temPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TradeItemList[i]["ItemNameKor"].ToString();
            temPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "필요 아이템 개수 : " + TradeItemList[i]["RequiredNumber"].ToString() + "개";
            tradePanelList.Add(temPanel);
        }
    }
    public void PressTypePanel(int num)
    {
        Debug.Log("클릭");
        selectPanelOrder = num;
        if(currentPanelOrder != selectPanelOrder)
        {
            typePanel[currentPanelOrder].sprite = change_img[0];
            typePanel[selectPanelOrder].sprite = change_img[1];
            currentPanelOrder = selectPanelOrder;
        }


    }
}
