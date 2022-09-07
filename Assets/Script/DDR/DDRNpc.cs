using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRNpc : MonoBehaviour
{
    private MiniGameManager DDRManager;
    // Start is called before the first frame update
    void Start()
    {
        DDRManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon")
        {
            Debug.Log("asd");
            Character.instance.SetCharacterInput(false, false);
            //DDRManager.DdrStart();
            //DDRManager.TimingStart();
            DDRManager.QuizStart();

        }
    }
}
