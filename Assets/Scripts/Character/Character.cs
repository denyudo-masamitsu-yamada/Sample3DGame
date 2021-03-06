﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharaType
{
    None,
    Player,
    Enemy,
}

/// <summary>
/// キャラクターベースクラス
/// </summary>
public class Character : MonoBehaviour
{
    readonly int AnimRequestIDHash = Animator.StringToHash("request_id");

    protected enum AnimationID
    {
        Idle = 0,
        Run = 1,

        Attack_1 = 10,
        Attack_2 = 11,
        Attack_3 = 12,

        Damage = 100,
        Dead = 110,
    }

    [SerializeField]
    float moveSpeed = 5.0f;

    [SerializeField]
    Transform headBoneTrans = null;

    [SerializeField]
    CharacterStatus status = new CharacterStatus();

    public Transform SelfTransform { get; private set; }
    protected NavMeshAgent SelfNavmeshAgent { get; private set; }
    protected Animator SelfAnimator { get; private set; }
    protected Character TargetingChara { get; private set; } = null;
    protected Character TargetedChara { get; private set; } = null;

    /// <summary>
    /// 現在のHP
    /// </summary>
    public int CurrentHP { get; private set; }

    /// <summary>
    /// キャラタイプ
    /// </summary>
    public CharaType CharaType { get; private set; }

    AttackHitCollider[] attackHitColliders = null;
    List<int> hitObjInstanceIDs = new List<int>();
    UICharaHpBar uiCharaHpBar = null;

    /// <summary>
    /// Headのボーンを取得
    /// </summary>
    /// <returns></returns>
    public Transform GetHeadBoneTransform()
    {
        return headBoneTrans;
    }

    /// <summary>
    /// ステータス取得
    /// </summary>
    /// <returns></returns>
    public CharacterStatus GetStatus()
    {
        return status;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init(CharaType charaType, UICharaHpBar uiCharaHpBar)
    {
        CharaType = charaType;

        SelfTransform = transform;
        SelfNavmeshAgent = GetComponent<NavMeshAgent>();
        SelfAnimator = GetComponent<Animator>();

        attackHitColliders = GetComponentsInChildren<AttackHitCollider>();
        foreach (var attackHit in attackHitColliders)
        {
            attackHit.Init(this);
        }

        CurrentHP = status.HP;

        this.uiCharaHpBar = uiCharaHpBar;
        uiCharaHpBar.Register(this);
    }

    private void Update()
    {
        UpdateExecute();
    }

    protected virtual void UpdateExecute() { }

    void LateUpdate()
    {
        LateUpdateExecute();
    }

    protected virtual void LateUpdateExecute() { }

    /// <summary>
    /// 移動する
    /// </summary>
    /// <param name="axis"></param>
    protected void Move(Vector3 axis)
    {
        SelfNavmeshAgent.Move(axis * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// アニメーション再生
    /// </summary>
    /// <param name="requestID"></param>
    protected void PlayAnimation(AnimationID animID)
    {
        SelfAnimator.SetInteger(AnimRequestIDHash, (int)animID);
    }

    /// <summary>
    /// 攻撃開始通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected virtual void OnAttack()
    {

    }

    /// <summary>
    /// 攻撃開始通知（終了がある場合に使う）
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected virtual void OnStartAttackHit()
    {
        hitObjInstanceIDs.Clear();
        SetAttackHitColliderEnable(true);
    }

    /// <summary>
    /// 攻撃終了通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected virtual void OnEndAttackHit()
    {
        SetAttackHitColliderEnable(false);
    }

    /// <summary>
    /// 次攻撃の入力受付開始の通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected virtual void OnStartNextAttackCombo()
    {
    }

    /// <summary>
    /// 次攻撃の入力受付終了の通知
    /// モーションイベントで呼ばれるコールバック
    /// </summary>
    protected virtual void OnEndNextAttackCombo()
    {

    }

    protected virtual void OnIdle()
    {
    }

    /// <summary>
    /// 攻撃ヒットの通知
    /// </summary>
    public virtual bool NotifyAttackHit(GameObject hitObj)
    {
        // すでにヒットしていたら、何もしない
        int instanceID = hitObj.GetInstanceID();
        if (hitObjInstanceIDs.Contains(instanceID))
        {
            return false;
        }

        hitObjInstanceIDs.Add(instanceID);

        Character hitChara = hitObj.GetComponent<Character>();
        if (hitChara != null)
        {
            hitChara.Damage(GetStatus().AttackPower, SelfTransform);
        }

        return true;
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage, Transform attackerTrans)
    {
        // ダメージ計算し、最小ダメージを１にする。
        int calcDamage = damage - status.Defence;
        calcDamage = Mathf.Max(calcDamage, 1);

        CurrentHP -= calcDamage;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            OnDead(attackerTrans);
        }
        else
        {
            OnDamage(attackerTrans);
        }
    }

    /// <summary>
    /// ダメージ時に呼ばれる
    /// </summary>
    protected virtual void OnDamage(Transform attackerTrans) { }

    /// <summary>
    /// 死亡時に呼ばれる
    /// </summary>
    protected virtual void OnDead(Transform attackerTrans)
    {
        // 自分自身をターゲットしているキャラのターゲット情報をNULLにする
        if (TargetedChara != null)
        {
            TargetedChara.SetTarget(null);
            TargetedChara = null;
        }

        uiCharaHpBar.Unregister(this);
    }

    /// <summary>
    /// 死亡？
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return CurrentHP <= 0;
    }

    /// <summary>
    /// 攻撃コライダーの有効設定
    /// </summary>
    /// <param name="enable"></param>
    void SetAttackHitColliderEnable(bool enable)
    {
        foreach (var attackHit in attackHitColliders)
        {
            attackHit.SetColliderEnable(enable);
        }
    }

    /// <summary>
    /// ターゲット設定
    /// </summary>
    /// <param name="chara"></param>
    protected void SetTarget(Character chara)
    {
        // ターゲットキャラ
        TargetingChara = chara;

        if (TargetingChara != null)
        {
            // 自身をターゲットしているキャラ
            TargetingChara.TargetedChara = this;
        }
    }

    /// <summary>
    /// ターゲット更新
    /// </summary>
    /// <param name="targets"></param>
    protected void UpdateTargeting(Character[] targets)
    {
        // ターゲット検索
        Character targetChara = null;
        float minSqrDist = float.MaxValue;
        foreach (var target in targets)
        {
            // 角度計算
            Vector3 targetDir = target.SelfTransform.localPosition - SelfTransform.localPosition;
            float angle = Vector3.Angle(targetDir, SelfTransform.forward);
            if (angle < status.CheckTargetAngle)
            {
                // 距離計算
                float sqrDist = targetDir.sqrMagnitude;
                if (minSqrDist > sqrDist &&
                    sqrDist <= status.CheckTargetDistance * status.CheckTargetDistance)
                {
                    minSqrDist = sqrDist;
                    targetChara = target;
                }
            }
        }

        SetTarget(targetChara);
    }

    /// <summary>
    /// ターゲットの方向に向く
    /// </summary>
    protected void UpdateLookTarget()
    {
        if (TargetingChara != null)
        {
            Vector3 dir = (TargetingChara.SelfTransform.localPosition - SelfTransform.localPosition).normalized;
            SelfTransform.localRotation = Quaternion.LookRotation(dir);
        }
    }
}


