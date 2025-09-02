using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyWave wavePrefab;
    [SerializeField] EnemyData[] normalEnemyData;
    [SerializeField] EnemyData LerkerData;

    //Currently 60% for Test
    float spawnProbability = 0.6f;
    float[] enemyProbability = { 65, 25, 10 };

    //Test Update, Remove at the End
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            GenerateWave();
    }

    public void GenerateWave()
    {
        //Need to Fix with Object Pooling
        EnemyWave wave = Instantiate(wavePrefab);
        for (int i = 0; i < EnemyWave.LANECOUNT; i++)
        {
            if (Random.Range(0.0f, 1.0f) <= spawnProbability)
            {
                EnemyData selectedEnemy = normalEnemyData[RandomManager.RandomPicker(enemyProbability)];
                if (selectedEnemy.enemySize.y + i > EnemyWave.LANECOUNT)
                {
                    wave.SetEnemy(i, null);
                    continue;
                }
                wave.SetEnemy(i, selectedEnemy);
                if (selectedEnemy.enemySize.y > 1)
                {
                    for (int j = 1; j < selectedEnemy.enemySize.y; j++)
                    {
                        wave.SetEnemy(i + j, null);
                        i++;
                    }
                }
            }
            else
            {
                wave.SetEnemy(i, null);
            }
        }

    }
}
