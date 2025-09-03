using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootLaser")]
public class ShootLaserFloor : EnemyBehaviour
{
    [SerializeField] EnemyLaser LaserPrefab;

    public override IEnumerator ActionCorutine(Transform transform)
    {
        yield return new WaitForSeconds(ActionCooldown);
        EnemyAction(transform);
    }


    public override void EnemyAction(Transform t)
    {
        EnemyLaser laser = Instantiate(LaserPrefab, t);
    }
}
