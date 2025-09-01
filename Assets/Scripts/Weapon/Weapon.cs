using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("공격 관련")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float accuracy = 1f;
    [SerializeField] private float attackDamage = 1;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private GameObject bulletPrefab;

    [Header("탄 퍼짐 설정")]
    [SerializeField] private GameObject spreadCircle; // 퍼짐 범위 시각화용 오브젝트
    [SerializeField] private float baseSpread = 0f;    // 최소 퍼짐 각도(도 단위)
    [SerializeField] private float maxSpread = 15f;    // 최대 퍼짐 각도
    [SerializeField] private float maxDistance = 10f;  // 최대 거리(이 이상이면 maxSpread 적용)

    private float nextAttackTime = 0f;

    private void Update()
    {
        // 탄 퍼짐 시각화
        if (spreadCircle == null) return;

        Vector2 start = firePoint.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(start, end);

        // 거리 비율 계산
        float t = Mathf.Clamp01(distance / maxDistance);
        float spread = Mathf.Lerp(baseSpread, maxSpread, t);

        Debug.Log($"Distance: {distance}, Spread: {spread}");
        spreadCircle.transform.position = end;
        spreadCircle.transform.localScale = Vector3.one * (Mathf.Tan(spread * Mathf.Deg2Rad) * distance * 2f);
    }

    public virtual void Fire()
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackRate;

        Vector2 start = firePoint.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        // 거리 비율 계산
        float t = Mathf.Clamp01(distance / maxDistance);
        float spread = Mathf.Lerp(baseSpread, maxSpread, t);

        // 랜덤 각도 추가
        float randomAngle = Random.Range(-spread, spread);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + randomAngle;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));

        Instantiate(bulletPrefab, firePoint.position, rot);
    }

}
