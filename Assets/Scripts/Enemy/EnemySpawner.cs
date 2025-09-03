using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("General Data")]
    [SerializeField] EnemyWave wavePrefab;
    [SerializeField] StageData stageData;
    bool isLerkerSpawnable = false;

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
        StartCoroutine(LerkerSpawner());
        StartCoroutine(WaveGenrator());
    }

    public IEnumerator LerkerSpawner()
    {
        while (true)
        {
            if (!isLerkerSpawnable)
            {
                yield return new WaitForSeconds(stageData.LerkerSpawnDelay);
                isLerkerSpawnable = true;
            }
            else
                yield return null;
        }
    }

    public IEnumerator WaveGenrator()
    {
        while (true)
        {
            GenerateWave();
            yield return new WaitForSeconds(stageData.SpawnDelay);
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
