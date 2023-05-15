using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    // 카메라 기본 위치
    private Vector3 cameraPosition;
    
    private Vector3 RangePosition;
    
    private Vector2 center;

    // 현재 씬 크기
    [SerializeField]
    private Vector2 MapSize;

    [SerializeField]
    // 카메라 따라가는 속도
    private float cameraSpeed;
    private float height;
    private float width;

    private bool GameStart;

    void Start()
    {
        DontDestroyOnLoad(this);

        SceneLoadManager.instance.MapSettingEvent.AddListener(SetCameraRange);

        cameraPosition = new Vector3(0, 0, -10f);
        RangePosition = new Vector3(0, 0, -10f);
        cameraSpeed = 5.0f;

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

        GameStart = false;

        //SetCameraRange();
        //LimitCameraArea();

        //GameManager.instance.AddSceneMoveEvent(SceneMoveCameraPosition);
        GameManager.instance.AddGenerateGameEvent(StartSetCameraRange);
    }

    void FixedUpdate()
    {
        //transform.position = player.transform.position + cameraPosition;
        if(GameStart)
        {
            LimitCameraArea();
        }
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
        if ((MapSize * 2) != SceneLoadManager.instance.Background.sizeDelta)
        {
            MapSize = SceneLoadManager.instance.Background.sizeDelta / 2f;
        }

        // LimitCameraArea를 따로 하는 이유는 보정값이 없는 쌩 값으로 카메라 위치를 설정하기 위해
        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, 1f);

        RangePosition.x = Mathf.Clamp(transform.position.x, center.x - (MapSize.x - width), (MapSize.x - width) + center.x);
        RangePosition.y = Mathf.Clamp(transform.position.y, center.y - (MapSize.y - height), (MapSize.y - height) + center.y);

        transform.position = RangePosition;
    }

    /*private void SceneMoveCameraPosition()
    {
        Debug.Log("현재 카메라 위치 " + transform.position);
        Debug.Log("캐릭터 위치 " + Character.instance.gameObject.transform.position);
        transform.position = Character.instance.gameObject.transform.position;

        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, 1f);


        RangePosition.x = Mathf.Clamp(transform.position.x, center.x - (MapSize.x - width), (MapSize.x - width) + center.x);
        RangePosition.y = Mathf.Clamp(transform.position.y, center.y - (MapSize.y - height), (MapSize.y - height) + center.y);

        transform.position = RangePosition;
        Debug.Log("최종 카메라 위치 " + transform.position);
    }*/

    private void StartSetCameraRange()
    {
        GameStart = true;
    }
}