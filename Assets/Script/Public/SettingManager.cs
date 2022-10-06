using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    private Button SaveButton;

    void Awake()
    {
        SaveButton = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveButton.onClick.AddListener(GameDataManager.instance.Save);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
