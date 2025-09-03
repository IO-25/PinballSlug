using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public Player player { get; set; }
    public EnemySpawner enemySpawner { get; set; }

    public void Start()
    {
        LoadStage(0);
    }

    public void LoadStage(int stagenumber)
    {
        enemySpawner.Init(GameManager.Instance.StageDatabase[stagenumber]);
    }
}
