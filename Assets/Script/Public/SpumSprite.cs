using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpumSprite : MonoBehaviour
{
    [SerializeField]
    private GameObject Original;
    [SerializeField]
    private SPUM_SpriteList spriteList;


    
    void Start()
    {
        Asd();
    }

    
    void Update()
    {
        
    }

    private void Asd()
    {
        spriteList._hairListString[0] = "Assets/Resources/SPUM/SPUM_Sprites/Items/0_Hair/Hair_1.png";

        spriteList.ResyncData();

        /*var obj = this.gameObject;
        


        PrefabUtility.ApplyObjectOverride(spriteList, "Assets/Resources/Public/TemPlayerCharacter.prefab", InteractionMode.AutomatedAction);
        PrefabUtility.ApplyPrefabInstance(obj, InteractionMode.AutomatedAction);*/
    }


}
