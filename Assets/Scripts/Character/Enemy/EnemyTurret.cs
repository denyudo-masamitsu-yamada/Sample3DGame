using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
	[SerializeField]
	GameObject bulletPrefab = null;

	[SerializeField]
	Transform createBulletTrans = null;

	[SerializeField]
	float createBulletIntervalTime = 5.0f;

	float createBulletTiming = 0.0f;

	public override void Init(CharaType charaType, UICharaHpBar uiCharaHpBar)
	{
		base.Init(charaType, uiCharaHpBar);

		createBulletTiming = createBulletIntervalTime;
	}

	protected override void UpdateExecute()
	{
		base.UpdateExecute();

		ActionState actionState = GetActionState();
		if (actionState == ActionState.Idle)
		{
			createBulletTiming -= Time.deltaTime;
			if (createBulletTiming <= 0.0f)
			{
				createBulletTiming = createBulletIntervalTime;
				ChangeActionState(ActionState.Attack);
			}
		}
		else if (actionState == ActionState.Attack)
		{
			GameObject bulletInstance = Instantiate(bulletPrefab);
			Bullet bullet = bulletInstance.GetComponent<Bullet>();
			bullet.StartMove(this, createBulletTrans.position, SelfTransform.forward);

			ChangeActionState(ActionState.Idle);
		}
	}

	protected override void OnDead(Transform attackerTrans)
	{
		base.OnDead(attackerTrans);
		ChangeActionState(ActionState.Dead);
	}

	protected override void OnStartActionState(ActionState actionState)
	{
		base.OnStartActionState(actionState);

		if (actionState == ActionState.Dead)
		{
			Vector3 effectPos = SelfTransform.localPosition + Vector3.up;
			EffectManager.PlayEffect(EffectID.Explosion, effectPos);
			
			gameObject.SetActive(false);
		}
	}
}
