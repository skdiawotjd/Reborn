using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image Inven;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInven();
        gameObject.GetComponentInParent<PlayerController>().EventInven.AddListener(() => { VisibleInven(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void VisibleInven()
    {
        if (Inven)
        {
            Inven.gameObject.SetActive(!Inven.gameObject.activeSelf);
        }
        else
        {
            InitializeInven();
            VisibleInven();
        }
    }

    private void InitializeInven()
    {
        Inven = GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Image>();
    }
}
