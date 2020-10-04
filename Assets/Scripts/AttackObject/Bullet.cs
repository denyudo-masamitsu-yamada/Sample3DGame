using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	float speed = 10.0f;

	[SerializeField]
	float lifeTime = 10.0f;

	Character ownerChara = null;

	Vector3 moveDir = Vector3.zero;
	float activeTime = 0.0f;

	private void Update()
	{
		transform.localPosition += moveDir * speed * Time.deltaTime;

		activeTime += Time.deltaTime;
		if (activeTime >= lifeTime)
		{
			// 削除
			DestroyImmediate(gameObject);
		}
	}

	public void StartMove(Character ownerChara, Vector3 pos, Vector3 dir)
	{
		this.ownerChara = ownerChara;

		transform.localPosition = pos;
		moveDir = dir;
	}

	private void OnTriggerEnter(Collider other)
	{
		// 自分自身の攻撃、仲間なら無視する
		if (other.gameObject.layer == gameObject.layer)
		{
			return;
		}

		Character hitChara = other.GetComponent<Character>();
		if (hitChara != null)
		{
			hitChara.Damage(ownerChara.GetStatus().AttackPower, transform);
		}
	}
}
