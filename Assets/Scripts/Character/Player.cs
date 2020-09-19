using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー制御
/// </summary>
public class Player : Character
{
    [SerializeField]
    CameraController cameraCtrl = null;

    public override void Init()
    {
        base.Init();


    }

    protected override void UpdateExecute()
    {
        UpdateMove();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void UpdateMove()
    {
        Vector3 moveAxis = InputManager.GetMoveAxis();

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(cameraCtrl.MainCameraTrans.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * moveAxis.z + cameraCtrl.MainCameraTrans.right * moveAxis.x;

        // 移動量が入っていたら、移動させる
        if (moveForward.sqrMagnitude > 0.0f)
        {
            Move(moveForward);
            SelfTransform.localRotation = Quaternion.LookRotation(moveForward.normalized);

            PlayAnimation(AnimationID.Run);
        }
        else
        {
            PlayAnimation(AnimationID.Idle);
        }
    }
}
