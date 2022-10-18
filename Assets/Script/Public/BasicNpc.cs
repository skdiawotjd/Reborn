using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicNpc : MonoBehaviour
{
    [SerializeField]
    protected int _npcNumber;
    protected ConversationManager ConversationManager;
    // 0 - ���� ���, 1 - Ȯ�� ���, 2 - true, 3 - false
    protected int ChatType;

    public int NpcNumber
    {
        get
        {
            return _npcNumber;
        }
    }

    protected virtual void Start()
    {
        ChatType = 0;
        ConversationManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(1).GetComponent<ConversationManager>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon")
        {
            FunctionStart();
        }
    }

    protected abstract void FunctionStart();
    public abstract void FunctionEnd();
}