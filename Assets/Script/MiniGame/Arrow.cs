using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Arrow : MonoBehaviour
{
    private Vector3 m_Position;

    private void Start()
    {
        m_Position = new Vector3(0.3f, 0.3f, 0.0f);
    }
    public void ArrowAnim()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.1f, 1f, 0.25f, 1f);
        StartCoroutine(AnimCoroutine());
    }
    public void DestroyImage() { Destroy(gameObject); }

    IEnumerator AnimCoroutine()
    {
        while (m_Position.x < 0.5f)
        {
            m_Position.x += 0.01f;
            m_Position.y += 0.01f;
            gameObject.transform.localScale = m_Position;
            yield return 0.01f;
        }

        Destroy(gameObject);  
    }


}
