using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootProjectileRandom")]
public class ShootProjectileRandom : EnemyBehaviour
{
    [SerializeField] GameObject ProjectilePrefab;

    public override void EnemyAction(Transform t)
    {
        Projectile projectile = Instantiate(ProjectilePrefab, t).GetComponent<Projectile>();
        float randomangle = Random.Range(-80.0f, 80.0f);
        projectile.SetDirection(Quaternion.Euler(0,0,randomangle) * -t.right);
    }
}
