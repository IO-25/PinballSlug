using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject spreadCircle; // ���� ���� �ð�ȭ�� ������Ʈ

    private int currentAmmo;
    private float nextAttackTime = 0f;

    public int CurrentAmmo => currentAmmo;

    public void Initialize(WeaponData newWeaponData)
    {
        weaponData = newWeaponData;
        currentAmmo = weaponData.maxAmmo;
    }

    private void Update()
    {
        // ź ���� �ð�ȭ
        if (spreadCircle == null) return;

        Vector2 start = firePoint.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(start, end);

        // �Ÿ� ���� ���
        float t = Mathf.Clamp01(distance / weaponData.maxDistance);
        float spread = Mathf.Lerp(weaponData.minSpread, weaponData.maxSpread, t);

        // ź ���� �� ����
        spreadCircle.transform.position = end;
        spreadCircle.transform.localScale = Vector3.one * (Mathf.Tan(spread * Mathf.Deg2Rad) * distance * 2f);
    }

    public virtual void Fire()
    {
        if (Time.time < nextAttackTime) return;
        if (weaponData.useAmmo && currentAmmo <= 0) return;

        nextAttackTime = Time.time + weaponData.attackRate;

        Vector2 start = firePoint.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        // �Ÿ� ���� ���
        float t = Mathf.Clamp01(distance / weaponData.maxDistance);
        float spread = Mathf.Lerp(weaponData.minSpread, weaponData.maxSpread, t);

        // ���� ���� �߰�
        float randomAngle = Random.Range(-spread, spread);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + randomAngle;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));

        // źȯ ����
        Instantiate(weaponData.bulletPrefab, firePoint.position, rot);

        // ź�� ����
        if(weaponData.useAmmo)
            currentAmmo = Mathf.Max(0, currentAmmo - 1);
        Debug.Log($"Current Ammo: {currentAmmo}");
    }

}
