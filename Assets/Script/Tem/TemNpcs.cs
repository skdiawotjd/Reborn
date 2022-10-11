using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemNpcs : MonoBehaviour
{
    [SerializeField]
    private int NpcNumber;
    private ConversationManager ConversationManager;

    void Start()
    {
        ConversationManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(1).GetComponent<ConversationManager>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon" )
        {
            ConversationManager.NpcToChat = NpcNumber;

            Debug.Log("대사 시작 1 - 콜리전 충돌(NPC 넘버 " + ConversationManager.NpcToChat + " )");
        }
    }
}
