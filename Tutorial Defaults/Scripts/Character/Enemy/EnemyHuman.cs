using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHuman : Enemy
{
	protected override void OnDamage(Transform attackerTrans)
	{
		base.OnDamage(attackerTrans);

		// 攻撃者の方向にむかせる
		Vector3 dir = (attackerTrans.localPosition - SelfTransform.localPosition).normalized;
		SelfTransform.localRotation = Quaternion.LookRotation(dir);
	}
}
