using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    public void OnDieAnimationEnd()
    {
        enemy.parentWave.SetEnemy(enemy.index, null);
        Destroy(enemy.gameObject);
    }
}
