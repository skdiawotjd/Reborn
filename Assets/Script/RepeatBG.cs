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
            Time.timeScale = 1f;
            newPos = Mathf.Repeat(Time.time * speed, posValue);
            transform.position = startPos + Vector2.left * newPos;
        }
        else if (!QuestManager.instance.moveBG)
        {
            //Time.timeScale = 0f;
        }
        
    }
}
