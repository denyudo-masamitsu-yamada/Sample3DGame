using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Init(CharaType charaType, UICharaHpBar uiCharaHpBar)
    {
        base.Init(charaType, uiCharaHpBar);

    }

    protected override void UpdateExecute()
    {

    }

    protected override void OnIdle()
    {
        PlayAnimation(AnimationID.Idle);
    }

    protected override void OnDead(Transform attackerTrans)
    {
        base.OnDead(attackerTrans);

        PlayAnimation(AnimationID.Dead);
    }

    protected override void OnDamage(Transform attackerTrans)
    {
        // 攻撃者の方向にむかせる
        Vector3 dir = (attackerTrans.localPosition - SelfTransform.localPosition).normalized;
        SelfTransform.localRotation = Quaternion.LookRotation(dir);

        PlayAnimation(AnimationID.Damage);
    }
}
