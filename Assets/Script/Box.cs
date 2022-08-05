using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int hp;

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
        if (hp == 0)
        {
            Destroy(gameObject);
            Debug.Log("»ç¶óÁü");
        }
        else
        {
            Debug.Log("hp--");
            hp--;
        }
    }
}
