using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootProjectileRandom")]
public class ShootProjectileRandom : EnemyBehaviour
{
    [SerializeField] GameObject ProjectilePrefab;
    public override void EnemyAction()
    {
        throw new System.NotImplementedException();
    }
}
