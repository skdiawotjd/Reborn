using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemPortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon")
        {
            MoveScene(2);
        }
    }

    public void MoveScene(int SceneType)
    {
        switch (SceneType)
        {
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGameDdr");
                break;
        }
    }
}