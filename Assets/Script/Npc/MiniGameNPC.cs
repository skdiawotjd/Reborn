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
            if(miniGameManager == null)
            {
                switch (Character.instance.MyMapNumber) // Character.instance.MyPosition 변수를 불러온다.
                {
                    case "0003": // ddr
                    case "0103":
                        miniGameManager = GameObject.Find("MGDDRManager").GetComponent<MiniGameManager>();
                        break;
                    case "0004": // 타이밍
                    case "0104":
                    case "0204":
                        miniGameManager = GameObject.Find("MGTimingManager").GetComponent<MiniGameManager>();
                        break;
                    case "0005": // 퀴즈
                    case "0105":
                        miniGameManager = GameObject.Find("MGQuizManager").GetComponent<MiniGameManager>();
                        break;
                    case "0006": // 오브젝트
                    case "0106":
                    case "0206":
                    case "0306":
                        miniGameManager = GameObject.Find("MGObjectManager").GetComponent<MiniGameManager>();
                        break;
                    default:
                        break;
                }
            }
            Character.instance.SetCharacterInput(false, false, false);
            miniGameManager.GameStart();
        }
        else
        {
            // 대충 활동 포인트가 부족하다는 내용
        }
    }

    public override void FunctionEnd()
    {
    }
}
