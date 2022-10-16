using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNPC : MonoBehaviour
{
    public TownManager townManager;
    // Start is called before the first frame update
    void Start()
    {
        townManager = GameObject.Find("TownManager").transform.GetComponent<TownManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon")
        {
            townManager.ChoiceButtonActive();
        }
    }
}
