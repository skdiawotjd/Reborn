using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemMineral : MonoBehaviour
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
        Debug.Log("Mineral���� �浹üũ " + collision.gameObject.name);
        /*if (collision.gameObject.name == "")
        {
            SetCharacterInterActionCollider(false);
        }*/
    }
}
