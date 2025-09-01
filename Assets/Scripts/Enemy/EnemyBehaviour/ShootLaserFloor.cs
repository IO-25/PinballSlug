using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootLaser")]
public class ShootLaserFloor : EnemyBehaviour
{
    [SerializeField] GameObject LaserPrefab;
    public override void EnemyAction()
    {
        throw new System.NotImplementedException();
    }
}
