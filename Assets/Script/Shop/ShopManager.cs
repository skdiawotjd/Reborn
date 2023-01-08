using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private ResidenceNPC ResidenceNPC;

    void Awake()
    {
        ResidenceNPC = (Instantiate(Resources.Load("Prefabs/NPC/ResidenceNPC"), new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<ResidenceNPC>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetShopMainQuest()
    {

    }
}
