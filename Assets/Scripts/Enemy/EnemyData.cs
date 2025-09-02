using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int InitialHealth;
    public Sprite enemySprite;
    public bool[] AvailableRow = new bool[8];
    public Vector2 enemySize = Vector2.one;
    [SerializeReference] public EnemyBehaviour[] behaviours;
}
