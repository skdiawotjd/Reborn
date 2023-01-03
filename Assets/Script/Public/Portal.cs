using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string SceneName;
    private List<Dictionary<string, object>> PortalNameList;
    private string mapName;
    // Start is called before the first frame update
    void Start()
    {
        PortalNameList = CSVReader.Read("PortalNameList");
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
    public void ChangeSceneName(string num)
    {
        SceneName = num;
    }
    public void MoveScene(string SceneType)
    {
        Character.instance.MyPlayerController.DisableCollider();
        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, SceneType);
        for(int i = 0; i < PortalNameList.Count; i++)
        {
            if(SceneType == PortalNameList[i]["MapNumber"].ToString())
            {
                mapName = PortalNameList[i]["MapName"].ToString();
            }
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(mapName);

/*        switch (SceneType)
        {
            case 0:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0002"); // ddr
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 1:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0003"); // Ÿ�̹�
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 2:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0004"); // ����
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 3:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0001"); // �뿹�� Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
                break;
            case 4:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0000"); // Home
                UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
                break;
            case 5:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0001"); // �뿹�� Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 6:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0005"); // ������Ʈ
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 7:
                Character.instance.MyPlayerController.DisableCollider();
                UnityEngine.SceneManagement.SceneManager.LoadScene("JustChat");
                break;
            case 8:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0201"); // ���������� Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 9:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0200"); // ���������� Home
                UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
                break;
            case 10:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0101"); // ������ Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 11:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0301"); // ������� Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 12:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0401"); // ������ Town
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
                break;
            case 13:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "4444"); // ������Ʈ
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 14:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0004"); // ���尣 ����
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 15:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0003"); // ���尣 ����
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            case 16:
                Character.instance.MyPlayerController.DisableCollider();
                Character.instance.SetCharacterStat(CharacterStatType.MyPositon, "0001"); // ���尣 ����Ʈ
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
                break;
            default:
                break;
        }*/
    }
}
