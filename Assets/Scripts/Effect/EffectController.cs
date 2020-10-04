using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
	ParticleSystem[] particleSystems = null;

	private void Awake()
	{
		// PlayOnAwakeがTRUEの場合、再生されるので、停止させる。
		particleSystems = GetComponentsInChildren<ParticleSystem>();
		Stop();

		// 最初は非アクティブにしておく
		gameObject.SetActive(false);
	}

	private void Update()
	{
		foreach (var particle in particleSystems)
		{
			if (particle.isPlaying)
			{
				return;
			}
		}

		gameObject.SetActive(false);
	}

	public void Play(Vector3 pos)
	{
		gameObject.SetActive(true);
		transform.localPosition = pos;

		foreach (var particle in particleSystems)
		{
			particle.Play();
		}
	}

	public void Stop()
	{
		foreach (var particle in particleSystems)
		{
			particle.Stop();
		}
	}

	public bool IsActive()
	{
		return gameObject.activeInHierarchy;
	}

	public Transform GetTransform()
	{
		return transform;
	}
}
