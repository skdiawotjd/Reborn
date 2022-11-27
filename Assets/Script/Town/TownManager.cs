using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TownManager : MonoBehaviour
{
    private void Awake()
    {
        //SceneLoadManager.instance.MapSetting();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TownSceneMove(string mapNumber)
    {
        Character.instance.MyPlayerController.DisableCollider();
        Character.instance.SetCharacterStat(CharacterStatType.MyPositon, mapNumber);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
    }
}
