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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveButton.onClick.AddListener(GameManager.instance.LoadGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
