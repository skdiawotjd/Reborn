using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    enum SocialClass { Slayer, Commons, Nobility, King }
    enum Job { Slayer, Smith, Bania, MasterSmith, Merchant,
        Baron, Viscount, Earl, Marquess, Duke, GrandDuke, 
        Knight, Scholar , Masterknight, Alchemist, 
        King }

    private SocialClass MySocialClass;
    private Job MyJob;
    private int MyAge;
    private int MyRound;

    private Inventory MyInven;

    // Start is called before the first frame update
    void Start()
    {
        MyInven = gameObject.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
