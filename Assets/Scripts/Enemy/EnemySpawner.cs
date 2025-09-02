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
                wave.SetEnemy(i, normalEnemyData[RandomPicker(enemyProbability)]);
            }
            else
            {
                wave.SetEnemy(i, null);
            }
        }

    }

    public int RandomPicker(float[] Probability)
    {
        float sum = 0;
        for (int i = 0; i < Probability.Length; i++)
        {
            sum += Probability[i];
        }
        if (sum == 0)
        {
            throw new System.Exception("Array of Probability sum of 0");
        }
        for (int i = 0; i < Probability.Length; i++)
        {
            Probability[i] /= sum;
            if (i > 0)
                Probability[i] += Probability[i - 1];
        }

        float randomNumber = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < Probability.Length; i++)
        {
            if (randomNumber <= Probability[i])
                return i;
        }
        throw new System.Exception("Probability Out Of Range");
    }
}
