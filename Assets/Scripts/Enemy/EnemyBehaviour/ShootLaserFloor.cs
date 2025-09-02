using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootLaser")]
public class ShootLaserFloor : EnemyBehaviour
{
    [SerializeField] GameObject LaserPrefab;

    public override IEnumerator ActionCorutine(Transform t)
    {
        throw new System.NotImplementedException();
    }

    public override void EnemyAction(Transform t)
    {
        throw new System.NotImplementedException();
    }
}
