using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 入力管理
/// </summary>
public class InputManager : MonoBehaviour
{
    public static Vector3 GetMoveAxis()
    {
        Vector3 axis = Vector3.zero;
        axis.x = Input.GetAxis("Horizontal");
        axis.z = Input.GetAxis("Vertical");
        return axis;
    }

    public static float GetCameraRotateAxis()
    {
        return Input.GetAxis("CameraRotate");
    }
}
