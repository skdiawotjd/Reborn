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

    private string MyName;
    private SocialClass MySocialClass;
    private Job MyJob;
    private int MyAge;
    private int MyRound;
    private int TodoProgress;
    private int[] MyStackByJob;
    private float MyWorkSpeed;
    private Inventory MyInven;

    public static Character instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyName = "Admin";
        MySocialClass = SocialClass.Slayer;
        MyJob = Job.Slayer;
        MyAge = 10;
        MyRound = 1;
        TodoProgress = 0;
        MyStackByJob = new int[16];
        for (int i = 0; i < MyStackByJob.Length; i++)
        {
            MyStackByJob[i] = 0;
        }
        MyWorkSpeed = 1.0f;
        MyInven = gameObject.GetComponent<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
