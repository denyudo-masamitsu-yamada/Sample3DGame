using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField]
    Player player = null;

    [SerializeField]
    UICharaHpBar uiCharaHpBar = null;

    Enemy[] enemyList = null;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player.Init(CharaType.Player, uiCharaHpBar);

        enemyList = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemyList)
        {
            enemy.Init(CharaType.Enemy, uiCharaHpBar);
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    public Enemy[] GetEnemies()
    {
        return enemyList;
    }
}
