using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class EnemyBehaviour : ScriptableObject
{
    public int ActionCooldown;
    abstract public void EnemyAction(Transform t);

    public virtual IEnumerator ActionCorutine(Transform transform)
    {
        if (ActionCooldown <= 0)
            throw new System.Exception("ActionCooldown is less or equal to 0");
        while (true)
        {
            yield return new WaitForSeconds(ActionCooldown);
            EnemyAction(transform);
        }
    }
}
