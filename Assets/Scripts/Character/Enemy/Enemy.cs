using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected enum ActionState
    {
        None,

        Idle,
        Move,
        Attack,
        WaitAttack,

        Damage,
        Dead,
    }

    ActionState actionState = ActionState.None;

    public override void Init(CharaType charaType, UICharaHpBar uiCharaHpBar)
    {
        base.Init(charaType, uiCharaHpBar);

    }

    protected override void UpdateExecute()
    {

    }

    protected void ChangeActionState(ActionState actionState)
    {
        if (this.actionState == actionState)
        {
            OnSameActionState(this.actionState);
            return;
        }

        OnFinishActionState(this.actionState);

        this.actionState = actionState;

        OnStartActionState(this.actionState);
    }

    /// <summary>
    /// 切り替えるアクションが同じの時に呼ばれる
    /// </summary>
    /// <param name="actionState"></param>
    protected virtual void OnSameActionState(ActionState actionState)
    {

    }

    /// <summary>
    /// アクション終了時に呼ばれる
    /// </summary>
    /// <param name="actionState"></param>
    protected virtual void OnFinishActionState(ActionState actionState)
    {

    }

    /// <summary>
    /// アクション開始時に呼ばれる
    /// </summary>
    /// <param name="actionState"></param>
    protected virtual void OnStartActionState(ActionState actionState)
    {
        switch (actionState)
        {
            case ActionState.Idle:
                PlayAnimation(AnimationID.Idle);
                break;
            case ActionState.Damage:
                PlayAnimation(AnimationID.Damage);
                break;
            case ActionState.Dead:
                PlayAnimation(AnimationID.Dead);
                break;
        }
    }

    protected ActionState GetActionState()
    {
        return actionState;
    }

    protected override void OnIdle()
    {
        ChangeActionState(ActionState.Idle);
    }

    protected override void OnDead(Transform attackerTrans)
    {
        base.OnDead(attackerTrans);

        ChangeActionState(ActionState.Dead);
    }

    protected override void OnDamage(Transform attackerTrans)
    {
        ChangeActionState(ActionState.Damage);
    }
}
