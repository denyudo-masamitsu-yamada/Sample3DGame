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

	protected override void OnDead(Transform attackerTrans)
	{
		base.OnDead(attackerTrans);

		Vector3 effectPos = SelfTransform.localPosition + Vector3.up;
		EffectManager.PlayEffect(EffectID.Explosion, effectPos);

		gameObject.SetActive(false);
	}

	protected override void UpdateExecute()
	{
		base.UpdateExecute();

		createBulletTiming -= Time.deltaTime;
		if (createBulletTiming <= 0.0f)
		{
			GameObject bulletInstance = Instantiate(bulletPrefab);
			Bullet bullet = bulletInstance.GetComponent<Bullet>();
			bullet.StartMove(this, createBulletTrans.position, SelfTransform.forward);

			createBulletTiming = createBulletIntervalTime;
		}
	}
}
