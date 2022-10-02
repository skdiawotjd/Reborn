using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : MonoBehaviour
{
    private MiniGameManager miniGameManager;
    private int myPosition;
    // Start is called before the first frame update
    void Start()
    {
        miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Character.instance.SetCharacterStat(6, "0004");
       switch (Character.instance.MyPosition) // Character.instance.MyPosition 변수를 불러온다.
        {
            case "0002": // ddr
                myPosition = 0;
                break;
            case "0003": // 타이밍
                myPosition = 1;
                break;
            case "0004": // 퀴즈
                myPosition = 2;
                break;
            case "0005": // 오브젝트
                myPosition = 3;
                break;
            default:
                break;
        }
        Debug.Log(Character.instance.MyPosition);
        if (collision.gameObject.name == "R_Weapon")
        {
            Character.instance.SetCharacterInput(false, false);
            miniGameManager.GameStart(myPosition);
        }
    }
}
