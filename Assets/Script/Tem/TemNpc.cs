using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemNpc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Npc���� �浹üũ " + collision.gameObject.name);
        if (collision.gameObject.name == "R_Weapon")
        {
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
            Debug.Log("��� ���� 2 - �ݸ��� �浹(NPC)");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
