using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageData : ScriptableObject
{
    [Header("Normal Enemy Pool")]
    [SerializeField] public float spawnProbability = 0.6f;
    [SerializeField] public EnemyData[] normalEnemyData;
    [SerializeField] public float[] enemyProbability = { 65, 25, 10 };

    [Header("Lerker Enemy Pool")]
    [SerializeField] public EnemyData LerkerData;
    [SerializeField] public float LerkerSpawnDelay;
    [SerializeField] public int[] lerkerSpawnIndex = { 0, 4 };
    [SerializeField] public float[] lerkerSpawnIndexProbability = { 75, 25 };

    [Header("General Spawn Info")]
    [SerializeField] public float SpawnDelay;
    [SerializeField] public Vector3 moveDistance;
}
