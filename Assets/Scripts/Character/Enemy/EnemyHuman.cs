using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyHuman : Enemy
{
	[SerializeField]
	float attackTimingTime = 5.0f;

	[SerializeField]
	GameObject bulletPrefab = null;

	[SerializeField]
	Transform createBulletTrans = null;

	float startAttackTime = 0.0f;

	protected override void UpdateExecute()
	{
		base.UpdateExecute();

		ActionState actionState = GetActionState();
		if (actionState == ActionState.Idle)
		{
			Player targetPlayer = FindTargetPlayer();
			if (targetPlayer != null)
			{
				SetTarget(targetPlayer);
				ChangeActionState(ActionState.Attack);
			}
		}
		else if (actionState == ActionState.Attack)
		{
			startAttackTime += Time.deltaTime;
			if (startAttackTime >= attackTimingTime)
			{
				startAttackTime = 0.0f;
				PlayAnimation(AnimationID.Attack_1);

				// 攻撃待ちをする
				ChangeActionState(ActionState.WaitAttack);
			}
		}
	}

	Player FindTargetPlayer()
	{
		Player targetPlayer = CharacterManager.Instance.GetPlayer();
		if (targetPlayer.IsDead())
		{
			return null;
		}

		Vector3 dir = targetPlayer.SelfTransform.localPosition - SelfTransform.localPosition;
		float sqrDistance = dir.sqrMagnitude;
		float checkDistance = GetStatus().CheckTargetDistance;
		if (sqrDistance <= checkDistance * checkDistance)
		{
			float angle = Vector3.Angle(dir.normalized, SelfTransform.forward);
			if (angle <= GetStatus().CheckTargetAngle)
			{
				return targetPlayer;
			}
		}

		return null;
	}

	protected override void OnDamage(Transform attackerTrans)
	{
		base.OnDamage(attackerTrans);

		// 攻撃者の方向にむかせる
		Vector3 dir = (attackerTrans.localPosition - SelfTransform.localPosition).normalized;
		SelfTransform.localRotation = Quaternion.LookRotation(dir);
	}

	protected override void OnAttack()
	{
		if (TargetingChara == null)
		{
			return;
		}

		Vector3 dir = TargetingChara.SelfTransform.localPosition - SelfTransform.localPosition;
		SelfTransform.localRotation = Quaternion.LookRotation(dir.normalized);

		GameObject bulletInstance = Instantiate(bulletPrefab);
		Bullet bullet = bulletInstance.GetComponent<Bullet>();
		bullet.StartMove(this, createBulletTrans.position, SelfTransform.forward);

	}
}
