using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ĳ������ Transform
    private Transform playerTransform;
    // z�� ����
    private Vector3 cameraPosition;

    private Vector2 center;

    [SerializeField]
    Vector2 mapSize;

    [SerializeField]
    // ī�޶� ���󰡴� �ӵ�
    public float cameraSpeed;
    private float height;
    private float width;

    void Start()
    {
        cameraPosition = new Vector3(0, 0, -10);
        cameraSpeed = 5.0f;

        playerTransform = Character.instance.GetComponent<Transform>();

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    void FixedUpdate()
    {
        //transform.position = player.transform.position + cameraPosition;
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        /*transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPosition, Time.deltaTime * cameraSpeed);*/
        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, Time.deltaTime * cameraSpeed);

        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}