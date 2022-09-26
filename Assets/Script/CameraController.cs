using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    // 씬의 크기
    private RectTransform Background;
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

    void Start()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += LoadedsceneEvent;

        cameraPosition = new Vector3(0, 0, -10f);
        RangePosition = new Vector3(0, 0, -10f);
        cameraSpeed = 5.0f;

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

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

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        Background = GameObject.Find("Background").GetComponent<RectTransform>();

        if ((MapSize * 2) != Background.sizeDelta)
        {
            MapSize = Background.sizeDelta / 2f;
        }

        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, 1f);

        Debug.Log(scene.name + "으로 변경되었습니다.");
    }
}