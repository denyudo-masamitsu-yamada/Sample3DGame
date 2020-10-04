using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectID
{
	None,   // 未定義

	Explosion,  // 爆発
	Discovery,	// 発見

	// 下に追加する
	// 途中で追加しない！
}

public class EffectManager : MonoBehaviour
{
	[System.Serializable]
	class EffectData
	{
		public GameObject effectPrefab = null;
		public int createCount = 0;
	}

	[SerializeField]
	List<EffectData> effectList = null;

	Dictionary<EffectID, List<EffectController>> effectPoolList = new Dictionary<EffectID, List<EffectController>>();

	static EffectManager instance = null;

	private void Awake()
	{
		instance = this;

		GameObject root = new GameObject("EffectRoot");

		for (int i = 0; i < effectList.Count; i++)
		{
			var effectID = (EffectID)(i + 1);
			effectPoolList.Add(effectID, new List<EffectController>());

			for (int k = 0; k < effectList[i].createCount; k++)
			{
				var instance = Instantiate(effectList[i].effectPrefab, root.transform);
				var effectCtrl = instance.AddComponent<EffectController>();
				effectPoolList[effectID].Add(effectCtrl);
			}
		}
	}

	public static EffectController PlayEffect(EffectID effectID, Vector3 pos)
	{
		foreach (var effect in instance.effectPoolList[effectID])
		{
			if (effect.IsActive())
			{
				continue;
			}

			effect.Play(pos);
			return effect;
		}

		return null;
	}
}
