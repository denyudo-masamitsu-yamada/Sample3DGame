using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー制御
/// </summary>
public class Player : Character
{
    enum ActionState
    {
        Idle,
        Move,
        Attack,
        Damage,
    }

    [SerializeField]
    CameraController cameraCtrl = null;

    ActionState actionState = ActionState.Idle;
    AnimationID attackComboAnimID = AnimationID.Attack_1;

    public override void Init()
    {
        base.Init();


    }

    protected override void UpdateExecute()
    {
        UpdateInput();

        switch (actionState)
        {
            case ActionState.Idle:
                UpdateIdle();
                break;
            case ActionState.Move:
                UpdateMove();
                break;
            case ActionState.Attack:
                UpdateAttack();
                break;
        }
    }

    void ChangeActionState(ActionState state)
    {
        // 終了処理
        switch (actionState)
        {
            case ActionState.Idle:
                break;
        }

        actionState = state;

        // 前処理
        switch (actionState)
        {
            case ActionState.Attack:
                attackComboAnimID = AnimationID.Attack_1;
                PlayAnimation(attackComboAnimID);
                break;
        }
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    void UpdateInput()
    {
        // 上の処理が、優先度高い！

        // 攻撃条件
        if (actionState == ActionState.Idle ||
            actionState == ActionState.Move)
        {
            if (InputManager.IsAttack())
            {
                ChangeActionState(ActionState.Attack);
                return;
            }
        }

        // 移動条件
        if (actionState == ActionState.Idle)
        {
            Vector3 moveAxis = InputManager.GetMoveAxis();
            if (moveAxis.sqrMagnitude > 0.0f)
            {
                ChangeActionState(ActionState.Move);
                return;
            }
        }

        // 停止条件
        if (actionState == ActionState.Move)
        {
            Vector3 moveAxis = InputManager.GetMoveAxis();
            if (moveAxis.sqrMagnitude <= 0.0f)
            {
                ChangeActionState(ActionState.Idle);
                return;
            }
        }
    }

    void UpdateIdle()
    {
        PlayAnimation(AnimationID.Idle);
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
    }

    void UpdateAttack()
    {

    }

    /// <summary>
    /// 攻撃開始通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    void Hit()
    {

    }

    /// <summary>
    /// 次攻撃の通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    void NextAttackCombo()
    {
        if (attackComboAnimID == AnimationID.Attack_3)
        {
            return;
        }

        attackComboAnimID++;
        PlayAnimation(attackComboAnimID);
    }
}
