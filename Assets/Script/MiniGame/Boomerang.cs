using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Boomerang : MonoBehaviour, IPoolObject
{
    private Vector2 m_Position;
    private Vector3 m_Rotation;
    private Rigidbody2D t_Rigidbody;
    private float m_Degree;
    private int degreeCount;
    public string idName;
    private float myOrder;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetOrder(int order)
    {
        myOrder = order * 90f;
    }
    private void RotateBoomerang()
    {
        StartCoroutine(Rotating(0.001f));
    }
    private void PositionBoomerang()
    {
        StartCoroutine(Positioning(0.001f));
    }
    private void ReturnObject()
    {
        StopAllCoroutines();
        AdventureGameManager.instance.pool.TakeToPool<Boomerang>(this.idName, this);
        transform.SetParent(AdventureGameManager.instance.pool.transform);
    }
    IEnumerator Positioning(float delayTime)
    {
        yield return YieldCache.WaitForSeconds(delayTime);
        if(degreeCount == 720)
        {
            ReturnObject();
        } else
        {
            m_Degree += 1f;
            m_Position.x = Mathf.Cos(m_Degree * Mathf.Deg2Rad) * 1.5f + t_Rigidbody.position.x;
            m_Position.y = Mathf.Sin(m_Degree * Mathf.Deg2Rad) * 1.5f + t_Rigidbody.position.y;
            transform.position = m_Position;
            degreeCount++;
            StartCoroutine(Positioning(delayTime));
        }
    }
    IEnumerator Rotating(float delayTime)
    {
        yield return YieldCache.WaitForSeconds(delayTime);
        m_Rotation.z += 0.2f;
        transform.Rotate(m_Rotation);
        StartCoroutine(Rotating(delayTime));
    }

    public void OnCreatedInPool()
    {
        m_Rotation = new Vector3(0f, 0f, 0f);
        t_Rigidbody = Character.instance.GetComponent<Rigidbody2D>();
    }

    public void OnGettingFromPool()
    {
        m_Position = t_Rigidbody.position;
        degreeCount = 0;
        m_Degree = myOrder;
        PositionBoomerang();
        RotateBoomerang();
    }
}
