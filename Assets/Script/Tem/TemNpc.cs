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
        Debug.Log("Npc에서 충돌체크 " + collision.gameObject.name);
        /*if (collision.gameObject.name == "")
        {
            SetCharacterInterActionCollider(false);
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
