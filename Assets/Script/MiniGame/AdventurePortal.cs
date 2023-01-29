using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurePortal : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "AttackArea")
        if (collision.gameObject.name == "PlayerCharacter")
        {
            AdventureGameManager.instance.MGManager.NextRound();
            Character.instance.SetCharacterPosition();
            this.gameObject.SetActive(false);
        }
            
    }
}
