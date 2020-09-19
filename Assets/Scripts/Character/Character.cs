using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        Dead = 101,
    }

    [SerializeField]
    float moveSpeed = 5.0f;

    public Transform SelfTransform { get; private set; }
    protected NavMeshAgent SelfNavmeshAgent { get; private set; }
    protected Animator SelfAnimator { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {
        SelfTransform = transform;
        SelfNavmeshAgent = GetComponent<NavMeshAgent>();
        SelfAnimator = GetComponent<Animator>();
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
    protected virtual void OnAttackHit()
    {
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
}


