using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemPortal : MonoBehaviour
{
    public int SceneName;

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
            MoveScene(SceneName);
        }
    }

    public void MoveScene(int SceneType)
    {
        switch (SceneType)
        {
            case 0:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, 0002);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 1:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, 0003);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 2:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, 0004);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 3:
                Character.instance.MyPlayerController.DisableCollider();
                UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
                break;
            case 4:
                Character.instance.MyPlayerController.DisableCollider();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
                break;

        }
    }
}
