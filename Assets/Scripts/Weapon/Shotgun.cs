using UnityEngine;

public class Shotgun : Weapon
{
    public ShotgunData ShotgunData => weaponData as ShotgunData;

    public override void Fire(Vector2 firePoint)
    {
        if (ShotgunData == null) return;
        if (Time.time < nextAttackTime) return;
        if (weaponData.useAmmo && currentAmmo <= 0) return;

        nextAttackTime = Time.time + (1 / weaponData.attackRate);

        Vector2 start = firePoint;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        // ÅºÈ¯ »ý¼º
        for (int i = 0; i < ShotgunData.bulletCount; i++)
        {
            float randomAngle = GetRandomSpreadAngle(distance);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + randomAngle;
            Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));
            Instantiate(weaponData.bulletPrefab, firePoint, rot);
        }

        // Åº¾à °¨¼Ò
        if (weaponData.useAmmo)
            currentAmmo = Mathf.Max(0, currentAmmo - 1);

        PlayFireSFX();
    }
}
