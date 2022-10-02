using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNPCContoroller : MonoBehaviour
{
    private SPUM_Prefabs Spum;
    private Vector3 m_Position;
    private GameObject RotationObject;
    private float InputX;
    private float InputY;
    private float moveSpeed;
    private float direction;
    private Vector3 horizontal;
    private Vector2 Offset;
    private Collider2D BodyCollider;

    // Start is called before the first frame update
    void Awake()
    {
        horizontal = new Vector3(1.0f, 1.0f, 1.0f);
        m_Position.y = transform.position.y;
        Spum = gameObject.GetComponent<SPUM_Prefabs>();
        BodyCollider = gameObject.GetComponent<Collider2D>();
        moveSpeed = 1.0f;
        direction = -1f;
        InvokeRepeating("Move", 0.001f, 0.001f);
        Spum._anim.SetBool("Run", true);
        Spum._anim.SetFloat("RunState", 0.5f);
        RotationObject = gameObject.transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(this.transform.position.x <= -7 && direction == -1)
        {
            StartCoroutine(WaitingTime(3f, 1));
        } else if(this.transform.position.x >= 7 && direction == 1)
        {
            StartCoroutine(WaitingTime(3f, -1));
        }
    }
    private void ChangeDirection(int dir)
    {
        switch (dir)
        {
            case 0:
                direction = 0f;
                Spum._anim.SetBool("Run", false);
                Spum._anim.SetFloat("RunState", 0.0f);
                break;
            case 1:
                direction = 1f;
                horizontal.x = -1.0f;
                if (RotationObject.transform.localScale.x != horizontal.x)
                {
                    RotationObject.transform.localScale = horizontal;
                }
                Spum._anim.SetBool("Run", true);
                Spum._anim.SetFloat("RunState", 0.5f);
                break;
            case -1:
                direction = -1f;
                horizontal.x = 1.0f;
                if (RotationObject.transform.localScale.x != horizontal.x)
                {
                    RotationObject.transform.localScale = horizontal;
                }
                Spum._anim.SetBool("Run", true);
                Spum._anim.SetFloat("RunState", 0.5f);
                break;
            default:
                break;
        }
    }
    private void Move()
    {
        m_Position.x = transform.position.x + direction * moveSpeed * Time.deltaTime;
        

        this.transform.position = m_Position;
    }

    IEnumerator WaitingTime(float delayTime, int change) // 일정 시간 대기
    {

        ChangeDirection(0);
        yield return new WaitForSeconds(delayTime);
        ChangeDirection(change);
    }
}
