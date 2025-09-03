using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("General Data")]
    [SerializeField] EnemyWave wavePrefab;
    [SerializeField] StageData stageData;
    bool isInitialized = false;

    [Header("Lerker")]
    float lastLerkerTime = 0.0f;
    bool isLerkerSpawnable = false;

    [Header("SpawnRate")]
    float lastSpawnedTime = 0.0f;

    private void Awake()
    {
        StageManager.Instance.enemySpawner = this;
    }

    public void Init(StageData data)
    {
        stageData = data;
        if (stageData.SpawnDelay == 0.0f)
            throw new System.ArgumentException("Stage Data SpawnDelay is 0");
        EnemyWave.leftMovement = stageData.moveDistance / stageData.SpawnDelay * Time.fixedDeltaTime;
        isInitialized = true;
    }

    //Test Update, Remove Debug at the End
    public void Update()
    {
        if (!isInitialized)
            return;
        if (!isLerkerSpawnable && Time.fixedTime - lastLerkerTime >= stageData.LerkerSpawnDelay)
        {
            isLerkerSpawnable=true;
        }

        if (Time.fixedTime - lastSpawnedTime >= stageData.SpawnDelay)
        {
            lastSpawnedTime = Time.fixedTime;
            GenerateWave();
        }
    }

    public void GenerateWave()
    {
        EnemyWave wave = Instantiate(wavePrefab, transform);
        int? lerkerIndex = null;
        //Spawn Lerker if possible
        if (isLerkerSpawnable)
        {
            lerkerIndex = stageData.lerkerSpawnIndex[RandomManager.RandomPicker(stageData.lerkerSpawnIndexProbability)];
            wave.SetEnemy((int)lerkerIndex, stageData.LerkerData);
            isLerkerSpawnable = false;
            lastLerkerTime = Time.fixedTime;
        }

        for (int i = 0; i < EnemyWave.LANECOUNT; i++)
        {
            if (wave.enemy[i].isInitialized)
                continue;
            if (Random.Range(0.0f, 1.0f) <= stageData.spawnProbability)
            {
                EnemyData selectedEnemy = stageData.normalEnemyData[RandomManager.RandomPicker(stageData.  enemyProbability)];
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
        if(wave.waveEnemyCount == 0)
            Destroy(wave);
        else
            wave.isInitialized = true;
    }
}
