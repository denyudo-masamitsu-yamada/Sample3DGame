using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 5.0f;

    [SerializeField]
    float traceSpeed = 5.0f;

    public Camera MainCamera { get; private set; }
    public Transform MainCameraTrans { get; private set; }

    private void Awake()
    {
        MainCamera = GetComponentInChildren<Camera>();
        MainCameraTrans = MainCamera.transform;
    }

    private void LateUpdate()
    {
        // プレイヤーを追尾する。
        Player player = CharacterManager.Instance.GetPlayer();
        Vector3 playerPos = player.SelfTransform.localPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, playerPos, traceSpeed * Time.deltaTime);

        // 回転
        float axis = InputManager.GetCameraRotateAxis();
        Vector3 angle = transform.eulerAngles;
        angle.y += axis * rotateSpeed * Time.deltaTime;
        transform.eulerAngles = angle;
    }
}
