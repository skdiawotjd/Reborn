using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : BasicNpc
{
    private MiniGameManager miniGameManager;
    private int gameType;
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
        if(Character.instance.ActivePoint >= 10)
        {
            switch (Character.instance.MyMapNumber) // Character.instance.MyPosition 변수를 불러온다.
            {
                case "0002": // ddr
                case "0102":
                case "0202":
                    gameType = 0;
                    break;
                case "0003": // 타이밍
                case "0103":
                    gameType = 1;
                    break;
                case "0004": // 퀴즈
                case "0104":
                    gameType = 2;
                    break;
                case "0005": // 오브젝트
                case "0105":
                case "0205":
                case "0305":
                    gameType = 3;
                    break;
                default:
                    break;
            }
        }
        else
        {
            // 대충 활동 포인트가 부족하다는 내용
        }


        Character.instance.SetCharacterInput(false, false);
        miniGameManager.GameStart(gameType);
    }

    public override void FunctionEnd()
    {
    }
}
