using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBG : MonoBehaviour
{
    [SerializeField][Range(1f, 20f)] float speed = 3f;
    [SerializeField] float posValue;
    [SerializeField] bool star;
    Vector2 startPos;
    float newPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.instance.moveBG)
        {
           // Time.timeScale = 1f;
            //newPos = Mathf.Repeat(Time.time * speed, posValue);
            //transform.position = startPos + Vector2.left * newPos;
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
            //Time.timeScale = 0f;
        }
        
    }
}
