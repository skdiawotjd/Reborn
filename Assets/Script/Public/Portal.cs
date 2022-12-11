using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
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
    public void ChangeSceneName(int num)
    {
        SceneName = num;
    }
    /// <summary>
    /// SceneType : 0 - ddr, 1 - timing, 2 - quiz, 3 - 노예town, 4 - 노예home, 5 - 노예town, 6 - object, 7 - JustChat, 8 - 대장장이Town, 9 - 대장장이Home, 10 - 상인Town, 11 - 대상인Town, 12 - 명장Town
    /// <para>
    /// SceneType : 0 - ddr, 1 - timing, 2 - quiz, 3 - 노예town, 4 - 노예home, 5 - 노예town, 6 - object, 7 - JustChat, 8 - 대장장이Town, 9 - 대장장이Home, 10 - 상인Town, 11 - 대상인Town, 12 - 명장Town
    /// </para>
    /// </summary>
    /// <param name="SceneType"></param>
    public void MoveScene(int SceneType)
    {
        switch (SceneType)
        {
            case 0:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0002"); // ddr
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 1:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0003"); // 타이밍
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 2:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0004"); // 퀴즈
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 3:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0001"); // 노예의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
                break;
            case 4:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0000"); // Home
                UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
                break;
            case 5:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0001"); // 노예의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 6:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0005"); // 오브젝트
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 7:
                Character.instance.MyPlayerController.DisableCollider();
                UnityEngine.SceneManagement.SceneManager.LoadScene("JustChat");
                break;
            case 8:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0201"); // 대장장이의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 9:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0200"); // 대장장이의 Home
                UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
                break;
            case 10:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0101"); // 상인의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 11:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0301"); // 대상인의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 12:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0401"); // 명장의 Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            default:
                break;
        }
    }
}
