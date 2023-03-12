using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBG : MonoBehaviour
{
    private float speed = 3f;

    void Start()
    {

    }
    void Update()
    {
        if (QuestManager.instance.moveBG)
        {
            if (transform.position.x < -19.2f)
            {
                transform.Translate(Vector3.right * 19.2f);
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
        else if (!QuestManager.instance.moveBG)
        {

        }
        
    }
}
