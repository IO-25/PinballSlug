using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "ScriptableObjects/EnemyBehaviour/ShootProjectilePlayer")]
public class ShootProjectilePlayer : EnemyBehaviour
{
    [SerializeField] GameObject ProjectilePrefab;

    public override void EnemyAction(Transform t)
    {
        if (StageManager.Instance.player == null)
            throw new System.Exception("No Player Target in StageManager");
        Projectile projectile = Instantiate(ProjectilePrefab, t).GetComponent<Projectile>();
        Vector3 targetposition = StageManager.Instance.player.transform.position;
        projectile.SetDirection((targetposition - t.position));

    }
}
