using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Good : MonoBehaviour
{
    private Vector3 my_Position;
    private Vector3 m_Position;
    private Sprite p_Sprite;
    private Sprite m_Sprite;

    // Start is called before the first frame update
    private void Awake()
    {
        p_Sprite = Resources.Load<Sprite>("MiniGame/perfect");
        m_Sprite = Resources.Load<Sprite>("MiniGame/Miss");
        my_Position = transform.position;
        m_Position = new Vector3(0.0f, 0.0f, 0.0f);
    }
    public void ChangeSource(bool Image)
    {
        m_Position = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.localScale = m_Position;
        gameObject.GetComponent<Image>().sprite = Image ? p_Sprite : m_Sprite;
        StartCoroutine("GoodAnimCoroutine");
    }
    IEnumerator GoodAnimCoroutine()
    {
        while (m_Position.x < 1f)
        {
            m_Position.x += 0.01f;
            m_Position.y += 0.01f;
            gameObject.transform.localScale = m_Position;
            yield return 0.01f;
        }
        gameObject.transform.localScale = my_Position;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
