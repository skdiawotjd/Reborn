using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // z축 땜에
    Vector3 cameraPosition;
    // 카메라 따라가는 속도
    public float cameraSpeed;

    void Start()
    {
        cameraPosition = new Vector3(0, 0, -10);
        cameraSpeed = 10.0f;
    }

    void FixedUpdate()
    {
        //transform.position = player.transform.position + cameraPosition;
        transform.position = Vector3.Lerp(transform.position, Character.instance.transform.position + cameraPosition, Time.deltaTime * cameraSpeed);
    }
}