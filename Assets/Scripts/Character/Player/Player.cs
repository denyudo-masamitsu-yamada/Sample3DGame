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

    // 現在の攻撃アニメID
    AnimationID attackAnimID = AnimationID.Attack_1;

    // 予約攻撃アニメID
    AnimationID reserveAttackAnimID = AnimationID.Attack_1;

    // 入力受付？
    bool isAttackAcceptInput = false;
    

    public override void Init(CharaType charaType, UICharaHpBar uiCharaHpBar)
    {
        base.Init(charaType, uiCharaHpBar);


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
        if (actionState == state)
        {
            OnSameActionState();
            return;
        }

        // 終了処理
        OnFinishActionState();

        actionState = state;

        // 開始処理
        OnStartActionState();
    }

    /// <summary>
    /// 同じ状態の処理
    /// </summary>
    void OnSameActionState()
    {
        switch (actionState)
        {
            case ActionState.Attack:
                reserveAttackAnimID++;
                isAttackAcceptInput = false;
                break;
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    void OnFinishActionState()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                break;
        }
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    void OnStartActionState()
    {
        switch (actionState)
        {
            case ActionState.Attack:
                isAttackAcceptInput = false;
                attackAnimID = AnimationID.Attack_1;
                reserveAttackAnimID = attackAnimID;
                PlayAnimation(attackAnimID);
                break;
            case ActionState.Damage:
                PlayAnimation(AnimationID.Damage);
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
        if (actionState == ActionState.Attack)
        {
            if (isAttackAcceptInput && InputManager.IsAttack())
            {
                ChangeActionState(ActionState.Attack);
                return;
            }
        }
        else if (actionState == ActionState.Idle ||
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

    /// <summary>
    /// 攻撃更新処理
    /// </summary>
    void UpdateAttack()
    {
        var enemies = CharacterManager.Instance.GetEnemies();
        UpdateTargeting(enemies);
        UpdateLookTarget();
    }

    /// <summary>
    /// 次攻撃の入力受付開始の通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected override void OnStartNextAttackCombo()
    {
        isAttackAcceptInput = true;
    }

    /// <summary>
    /// 次攻撃の入力受付終了の通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected override void OnEndNextAttackCombo()
    {
        if (attackAnimID == AnimationID.Attack_3)
        {
            return;
        }

        // 同じなら、終了！
        if (reserveAttackAnimID == attackAnimID)
        {
            return;
        }

        PlayAnimation(reserveAttackAnimID);
        attackAnimID = reserveAttackAnimID;
    }

    protected override void OnIdle()
    {
        ChangeActionState(ActionState.Idle);
    }

    protected override void OnDamage(Transform attackerTrans)
    {
        ChangeActionState(ActionState.Damage);
    }
}
