using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターステータス
/// </summary>
[System.Serializable]
public struct CharacterStatus
{
    public int HP;
    public int AttackPower;
    public int Defence;
    public float CheckTargetDistance;
    public float CheckTargetAngle;
}
