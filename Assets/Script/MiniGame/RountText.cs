using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RountText : MonoBehaviour
{
    private float timeCount;
    private TextMeshProUGUI roundText;
    private void Awake()
    {
        roundText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        timeCount = 0f;
    }
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(CountTime());
    }
    private void OnDisabled()
    {
        StopAllCoroutines();
        timeCount = 0f;
        roundText.fontSize = 120;
        gameObject.SetActive(false);
    }
    IEnumerator CountTime()
    {
        timeCount += 0.1f;
        if (timeCount > 1f)
        {
            OnDisabled();
        }
        else if (timeCount > 0.5f)
        {
            roundText.fontSize -= 3f;
        }
        else
        {
            roundText.fontSize += 3f;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CountTime());
    }
}
