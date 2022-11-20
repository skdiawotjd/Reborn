using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustChatManager : MonoBehaviour
{
    private GameObject MainCanvas;
    private ConversationManager ConversationManager;

    void Awake()
    {
        MainCanvas = GameObject.Find("Main Canvas");

        ConversationManager = MainCanvas.transform.GetChild(0).GetChild(1).GetComponent<ConversationManager>();
    }
    void Start()
    {
        MainCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        //ConversationManager.NpcNumberChatType = "0-0";
        Character.instance.SetCharacterInput(false, false);
        SetPrefab();
        Character.instance.MyPlayerController.EventConversation.Invoke();

        //StartCoroutine(CompleteChat());
    }

    IEnumerator CompleteChat()
    {
        while(ConversationManager.ConversationCount != -1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Character.instance.SetCharacterStat(6, "0000");
        //Character.instance.InitializeMapNumber();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }

    private void SetPrefab()
    {
        // 해당 씬에 배치될 그림이나 오브젝트 등을 생성
    }
}
