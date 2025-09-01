using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackDamage = 1;
    [SerializeField] private float attackRate = 1f;
    private float nextAttackTime = 0f;
    [SerializeField] private GameObject bulletPrefab;

    public virtual void Fire()
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackRate;

        Vector2 start = firePoint.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (end - start).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));

        Instantiate(bulletPrefab, firePoint.position, rot);
    }
}
