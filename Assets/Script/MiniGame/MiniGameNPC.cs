using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : BasicNpc
{
    private MiniGameManager miniGameManager;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void FunctionStart()
    {
        if(Character.instance.ActivePoint >= 10)
        {
            switch (Character.instance.MyMapNumber) // Character.instance.MyPosition ������ �ҷ��´�.
            {
                case "0002": // ddr
                case "0102":
                case "0202":
                    miniGameManager = GameObject.Find("MGDDRManager").GetComponent<MGDDRManager>();
                    break;
                case "0003": // Ÿ�̹�
                case "0103":
                    miniGameManager = GameObject.Find("MGTimingManager").GetComponent<MGTimingManager>();
                    break;
                case "0004": // ����
                case "0104":
                    miniGameManager = GameObject.Find("MGQuizManager").GetComponent<MGQuizManager>();
                    break;
                case "0005": // ������Ʈ
                case "0105":
                case "0205":
                case "0305":
                    miniGameManager = GameObject.Find("MGObjectManager").GetComponent<MGObjectManager>();
                    break;
                default:
                    break;
            }

            Character.instance.SetCharacterInput(false, false, false);
            miniGameManager.GameStart();
        }
        else
        {
            // ���� Ȱ�� ����Ʈ�� �����ϴٴ� ����
        }
    }

    public override void FunctionEnd()
    {
    }
}
