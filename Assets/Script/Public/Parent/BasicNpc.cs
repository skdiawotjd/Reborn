using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicNpc : MonoBehaviour
{
    [SerializeField]
    protected int _npcNumber;
    [SerializeField]
    protected int _chatType;    // 0 - 시작 대사, 1 - 확인 대사, 2 - true, 3 - false
    [SerializeField]
    protected string _npcName;

    protected ConversationManager ConversationManager;

    public int NpcNumber
    {
        get
        {
            return _npcNumber;
        }
    }
    public int ChatType
    {
        set
        {
            _chatType = value;

        }
        get
        {
            return _chatType;
        }
    }
    public string NpcName
    {
        get
        {
            return _npcName;
        }
    }

    public void SetNpcNumber(int number)
    {
       _npcNumber = number;
    }
    public void SetChatType(int Type)
    {
        _chatType = Type;
    }
    public void SetNpcName(string Neme)
    {
        _npcName = Neme;
    }


    protected virtual void Start()
    {
        //_npcNumber = 0;
        //_chatType = 0;
        ConversationManager = GameObject.Find("Main Canvas").transform.GetChild(0).GetChild(4).GetComponent<ConversationManager>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon")
        {
            Debug.Log("ASdasdasd");
            FunctionStart();
        }
    }

    protected abstract void FunctionStart();
    public abstract void FunctionEnd();
}
