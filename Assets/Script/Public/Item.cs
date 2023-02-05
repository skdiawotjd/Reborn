using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ItemCount;
    [SerializeField]
    private GameObject Description;

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
        ItemCount.text = Count.ToString();
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
