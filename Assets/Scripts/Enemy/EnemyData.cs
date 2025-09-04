using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int InitialHealth;
    public RuntimeAnimatorController EnemyAnimatorController;
    public Vector2 enemySize = Vector2.one;
    [SerializeReference] public EnemyBehaviour[] behaviours;

    [Header("드랍 정보")]
    public DropItemData[] dropItemDatas;
    [SerializeField] public float[] dropRate;
}
