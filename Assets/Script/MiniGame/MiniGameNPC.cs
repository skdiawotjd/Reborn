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
        switch (Character.instance.MyPosition) // Character.instance.MyPosition 변수를 불러온다.
        {
            case 5:
                myPosition = 0;
                break;
            case 6:
                myPosition = 1;
                break;
            case 7:
                myPosition = 2;
                break;
            default:
                break;
        }
        myPosition = 2;

        if (collision.gameObject.name == "R_Weapon")
        {
            Character.instance.SetCharacterInput(false, false);
            miniGameManager.GameStart(myPosition);
        }
    }
}
