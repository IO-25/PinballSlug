using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class EnemyBehaviour : ScriptableObject
{
    public int ActionCooldown;
    abstract public void EnemyAction();
}
