using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃のコライダー
/// </summary>
public class AttackHitCollider : MonoBehaviour
{
    Character ownerChara = null;
    Collider selfCollider = null;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="owner"></param>
    public void Init(Character owner)
    {
        ownerChara = owner;

        selfCollider = GetComponent<Collider>();
        selfCollider.enabled = false;
    }

    /// <summary>
    /// コライダーの有効設定
    /// </summary>
    /// <param name="enable"></param>
    public void SetColliderEnable(bool enable)
    {
        selfCollider.enabled = enable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ownerChara.gameObject.layer)
        {
            return;
        }

        // オーナーキャラに通知する。
        ownerChara.NotifyAttackHit(other.gameObject);
    }
}
