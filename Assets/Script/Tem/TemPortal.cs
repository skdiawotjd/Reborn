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
                Character.instance.SetCharacterStat(6, "0002"); // ddr
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 1:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, "0003"); // 타이밍
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 2:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, "0004"); // 퀴즈
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 3:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, "0001"); // 노예의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
                break;
            case 4:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, "0000"); // Home
                UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
                break;
            case 5:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, "0001"); // 노예의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 6:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(6, "0005"); // 오브젝트
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 7:
                Character.instance.MyPlayerController.DisableCollider();
                UnityEngine.SceneManagement.SceneManager.LoadScene("JustChat");
                break;
            default:
                break;
        }
    }
}
