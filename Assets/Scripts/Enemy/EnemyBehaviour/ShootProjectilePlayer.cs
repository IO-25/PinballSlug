using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootProjectilePlayer")]
public class ShootProjectilePlayer : EnemyBehaviour
{
    [SerializeField] GameObject ProjectilePrefab;

    public override void EnemyAction(Transform t)
    {
        //Check Player and Shoot at That Direction
        throw new System.NotImplementedException();
    }
}
