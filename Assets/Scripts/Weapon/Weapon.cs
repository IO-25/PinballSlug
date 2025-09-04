using UnityEngine;

public enum WeaponType
{
    Pistol,
    MachineGun,
    Shotgun,
}

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] private GameObject spreadCircle;
    [SerializeField] private TrajectoryRenderer trajectoryRenderer;
    [SerializeField] private AudioSource audioSource;

    protected int currentAmmo;
    protected float nextAttackTime = 0f;

    public int CurrentAmmo => currentAmmo;
    public WeaponData WeaponData => weaponData;

    public void SetActiveTrajectory(bool active)
    {
        if (trajectoryRenderer != null)
            trajectoryRenderer.gameObject.SetActive(active);
        if (spreadCircle != null)
            spreadCircle.SetActive(active);
    }

    public virtual void Initialize()
    {
        if (weaponData == null) return;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        currentAmmo = weaponData.maxAmmo;

        SetActiveTrajectory(false);
    }

    public void Look(Vector2 firePoint)
    {
        trajectoryRenderer.RenderTrajectory(firePoint);

        // 탄 퍼짐 시각화
        if (spreadCircle == null) return;

        Vector2 start = firePoint;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(start, end);

        // 거리 비율 계산
        float t = Mathf.Clamp01(distance / weaponData.maxDistance);
        float spread = Mathf.Lerp(weaponData.minSpread, weaponData.maxSpread, t);

        // 탄 퍼짐 원 조정
        spreadCircle.transform.position = end;
        spreadCircle.transform.localScale = Vector3.one * (Mathf.Tan(spread * Mathf.Deg2Rad) * distance * 2f);
    }

    public virtual void Fire(Vector2 firePoint)
    {
        if (Time.time < nextAttackTime) return;
        if (weaponData.useAmmo && currentAmmo <= 0) return;

        nextAttackTime = Time.time + (1 / weaponData.attackRate);

        Vector2 start = firePoint;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        // 랜덤 각도 추가
        float randomAngle = GetRandomSpreadAngle(distance);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + randomAngle;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));

        // 탄환 생성
        Instantiate(weaponData.bulletPrefab, firePoint, rot);

        PlayFireSFX();

        // 탄약 감소
        if (weaponData.useAmmo)
        {
            currentAmmo = Mathf.Max(0, currentAmmo - 1);
            Debug.Log($"Current Ammo: {currentAmmo}");
        }
    }

    protected float GetSpreadAngle(float distance)
    {
        // 거리 비율 계산
        float t = Mathf.Clamp01(distance / weaponData.maxDistance);
        return Mathf.Lerp(weaponData.minSpread, weaponData.maxSpread, t);
    }

    protected float GetRandomSpreadAngle(float distance)
    {
        float spread = GetSpreadAngle(distance);
        return Random.Range(-spread, spread);
    }

    protected void PlayFireSFX()
    {
        AudioClip randomClip = WeaponData.fireSFX[Random.Range(0, WeaponData.fireSFX.Length)];
        audioSource.PlayOneShot(randomClip);
    }
}