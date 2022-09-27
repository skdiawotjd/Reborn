using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    // ī�޶� �⺻ ��ġ
    private Vector3 cameraPosition;
    
    private Vector3 RangePosition;
    
    private Vector2 center;

    // ���� �� ũ��
    [SerializeField]
    private Vector2 MapSize;

    [SerializeField]
    // ī�޶� ���󰡴� �ӵ�
    private float cameraSpeed;
    private float height;
    private float width;


    void Start()
    {
        DontDestroyOnLoad(this);

        GameManager.instance.SceneMove.AddListener(SetCameraRange);

        cameraPosition = new Vector3(0, 0, -10f);
        RangePosition = new Vector3(0, 0, -10f);
        cameraSpeed = 5.0f;

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

        SetCameraRange();
        LimitCameraArea();
    }

    void FixedUpdate()
    {
        //transform.position = player.transform.position + cameraPosition;
        LimitCameraArea();
    }

    private void LimitCameraArea()
    {
        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, Time.deltaTime * cameraSpeed);


        RangePosition.x = Mathf.Clamp(transform.position.x, center.x - (MapSize.x - width), (MapSize.x - width) + center.x);
        RangePosition.y = Mathf.Clamp(transform.position.y, center.y - (MapSize.y - height), (MapSize.y - height) + center.y);

        transform.position = RangePosition;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, MapSize * 2);
    }

    private void SetCameraRange()
    {
        if ((MapSize * 2) != GameManager.instance.Background.sizeDelta)
        {
            MapSize = GameManager.instance.Background.sizeDelta / 2f;
        }

        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, 1f);
    }
}