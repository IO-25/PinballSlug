using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    public int InitialHealth;
    public Sprite enemySprite;
    public bool[] AvailableRow = new bool[8];
    public int AtkCooldown;
    public Vector2 enemySize = Vector2.one;
}
