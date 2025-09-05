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

#if UNITY_EDITOR
    //Debug Clear
    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
            SceneChanger.GoGameClearScene();
    }
#endif
    public void LoadStage(int stagenumber)
    {
        enemySpawner.Init(GameManager.Instance.StageDatabase[stagenumber]);
    }
}
