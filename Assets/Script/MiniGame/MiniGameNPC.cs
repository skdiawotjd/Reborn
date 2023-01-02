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
                case "0003": // ddr
                case "0103":
                case "0203":
                    if (miniGameManager == null)
                    {
                        miniGameManager = GameObject.Find("MGDDRManager").GetComponent<MGDDRManager>();
                    }
                    break;
                case "0004": // Ÿ�̹�
                case "0104":
                    if (miniGameManager == null)
                    {
                        miniGameManager = GameObject.Find("MGTimingManager").GetComponent<MGTimingManager>();
                    }
                    break;
                case "0005": // ����
                case "0105":
                    if (miniGameManager == null)
                    {
                        miniGameManager = GameObject.Find("MGQuizManager").GetComponent<MGQuizManager>();
                    }
                    break;
                case "0006": // ������Ʈ
                case "0106":
                case "0206":
                case "0306":
                    if (miniGameManager == null)
                    {
                        miniGameManager = GameObject.Find("MGObjectManager").GetComponent<MGObjectManager>();
                    }
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
