using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{

	protected override void OnDead(Transform attackerTrans)
	{
		base.OnDead(attackerTrans);

		Vector3 effectPos = SelfTransform.localPosition + Vector3.up;
		EffectManager.PlayEffect(EffectID.Explosion, effectPos);

		gameObject.SetActive(false);
	}
}
