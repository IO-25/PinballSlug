using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; // 레이저 시각화를 위한 LineRenderer
    [SerializeField] private AnimationCurve laserWidthOverTime; // 레이저 굵기 변화 곡선
    [SerializeField] private float widthMultiplier = 2f; // 레이저 크기 배율

    /*
    [SerializeField] private float expandDuration = 0.3f; // 최소 굵기에서 최대 굵기로 변화하는 시간
    [SerializeField] private float holdMaxDuration = 0.9f; // 최대 굵기 유지 시간
    [SerializeField] private float shrinkDuration = 0.3f; // 최대 굵기에서 최소 굵기로 변화하는 시간
    [SerializeField] private float minLaserWidth = 0.4f; // 레이저 굵기 배율
    [SerializeField] private float maxLaserWidth = 2.5f; // 레이저 굵기 배율
    */

    [SerializeField] private int damage = 5; // 공격 데미지
    [SerializeField] private float damageInterval = 0.1f; // 데미지 적용 간격
    [SerializeField] private float maxDistance = 100f; // 최대 사거리
    [SerializeField] private LayerMask hitBlockerMask; // 레이저가 충돌할 레이어 마스크
    [SerializeField] private LayerMask damageTargetMask; // 레이저가 충돌할 레이어 마스크
    private float nextDamageTime = 0f;

    private void Awake()
    {
        if(lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShotLaser(Vector2 firePoint, Vector2 direction)
    {
        transform.position = firePoint;
        transform.right = direction.normalized;

        lineRenderer.SetPosition(0, transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxDistance, hitBlockerMask);
        if (hit.collider != null)
            lineRenderer.SetPosition(1, hit.point);
        else
            lineRenderer.SetPosition(1, transform.position + transform.right * 100f);

        StartCoroutine(PlayAnimation());
    }

    private void ApplyDamage()
    {
        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + damageInterval;

        // BoxCast 실행
        float colliderWidth = lineRenderer.widthMultiplier;
        float colliderLength = Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * (colliderLength / 2);
        Vector2 size = new(colliderLength, colliderWidth);
        float angle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

        Collider2D[] colls = Physics2D.OverlapBoxAll(origin, size, angle, damageTargetMask);

        // 충돌 시 데미지 적용
        foreach (var coll in colls)
        {
            if (coll.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(damage);
        }
    }

    IEnumerator PlayAnimation()
    {
        float time = 0;
        float endTime = laserWidthOverTime.keys[laserWidthOverTime.length - 1].time;

        while (time < endTime)
        {
            time += Time.deltaTime;
            lineRenderer.widthMultiplier = laserWidthOverTime.Evaluate(time) * widthMultiplier;
            ApplyDamage();
            yield return null;
        }

        /*
        float time = 0;
        // 굵기 증가
        while (time < expandDuration)
        {
            time += Time.deltaTime;
            lineRenderer.widthMultiplier = Mathf.Lerp(minLaserWidth, maxLaserWidth, time / expandDuration) * widthMultiplier;
            yield return null;
        }

        // 최대 굵기 유지
        yield return new WaitForSeconds(holdMaxDuration);

        // 굵기 감소
        while (time < shrinkDuration)
        {
            time += Time.deltaTime;
            lineRenderer.widthMultiplier = Mathf.Lerp(maxLaserWidth, minLaserWidth, time / expandDuration) * widthMultiplier;
            yield return null;
        }
        */

        Destroy(gameObject);
    }

}
