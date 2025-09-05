using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootLaser")]
public class ShootLaserFloor : EnemyBehaviour
{
    [SerializeField] EnemyLaserContainer LaserContainerPrefab;

    public override IEnumerator ActionCorutine(Transform transform)
    {
        EnemyAction(transform);
        yield return null;
    }


    public override void EnemyAction(Transform t)
    {
        EnemyLaserContainer lasercontainer = Instantiate(LaserContainerPrefab, t);
        lasercontainer.warningDuration = ActionCooldown;
    }
}
