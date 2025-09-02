using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyWave wavePrefab;
    [SerializeField] EnemyData[] normalEnemyData;
    [SerializeField] EnemyData LerkerData;

    //Testing values
    //Need to change with Stage Values later
    float spawnProbability = 0.6f;
    float[] enemyProbability = { 65, 25, 10 };
    int[] lerkerSpawnIndex = { 0, 4 };
    float[] lerkerSpawnIndexProbability = { 75, 25 };
    float lastLerkerTime = 0.0f;
    float lerkerSpawnTime = 5.0f;
    bool isLerkerSpawnable = false;


    //Test Update, Remove Debug at the End
    public void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.V))
            GenerateWave();
        //Debug End

        if (!isLerkerSpawnable && Time.fixedTime - lastLerkerTime >= lerkerSpawnTime)
        {
            isLerkerSpawnable=true;
        }
    }

    public void GenerateWave()
    {
        EnemyWave wave = Instantiate(wavePrefab);
        int? lerkerIndex = null;
        //Spawn Lerker if possible
        if (isLerkerSpawnable)
        {
            lerkerIndex = lerkerSpawnIndex[RandomManager.RandomPicker(lerkerSpawnIndexProbability)];
            wave.SetEnemy((int)lerkerIndex, LerkerData);
            isLerkerSpawnable = false;
            lastLerkerTime = Time.fixedTime;
        }

        for (int i = 0; i < EnemyWave.LANECOUNT; i++)
        {
            if (wave.enemy[i].isInitialized)
                continue;
            if (Random.Range(0.0f, 1.0f) <= spawnProbability)
            {
                EnemyData selectedEnemy = normalEnemyData[RandomManager.RandomPicker(enemyProbability)];
                if (selectedEnemy.enemySize.y + i > EnemyWave.LANECOUNT ||
                    (lerkerIndex != null && i < lerkerIndex && i + selectedEnemy.enemySize.y > lerkerIndex))
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
