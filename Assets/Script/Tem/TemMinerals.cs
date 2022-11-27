using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemMinerals : MonoBehaviour
{
    public int Type;
    public string Val;

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
        Debug.Log("Mineral에서 충돌체크 " + collision.gameObject.name);
        Character.instance.SetCharacterStat((CharacterStatType)Type, Val);
        Destroy(this.gameObject);
    }
}
