using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : BasicNpc
{
    private MiniGameManager miniGameManager;
    private int myPosition;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void FunctionStart()
    {
        switch (Character.instance.MyPosition) // Character.instance.MyPosition ������ �ҷ��´�.
        {
            case "0002": // ddr
                myPosition = 0;
                break;
            case "0003": // Ÿ�̹�
                myPosition = 1;
                break;
            case "0004": // ����
                myPosition = 2;
                break;
            case "0005": // ������Ʈ
                myPosition = 3;
                break;
            default:
                break;
        }
        Debug.Log(Character.instance.MyPosition);

        Character.instance.SetCharacterInput(false, false);
        miniGameManager.GameStart(myPosition);
    }

    public override void FunctionEnd()
    {
    }
}
