using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _itemCountText;
    [SerializeField]
    private GameObject Description;

    private int _itemCount;

    public int ItemCount
    {
        get { return _itemCount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCount(int Count)
    {
        _itemCount = Count;
        _itemCountText.text = _itemCount.ToString();
    }

    private void OnMouseEnter()
    {
        Description.SetActive(true);
    }

    private void OnMouseExit()
    {
        Description.SetActive(false);
    }
}
