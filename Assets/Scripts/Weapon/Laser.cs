using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; // 레이저 시각화를 위한 LineRenderer
    [SerializeField] private AnimationCurve laserWidthOverTime; // 레이저 굵기 변화 곡선
    [SerializeField] private int damage = 5; // 공격 데미지
    [SerializeField] private float maxDistance = 100f; // 최대 사거리
    [SerializeField] private float duration = 1f; // 레이저 지속 시간
    [SerializeField] private LayerMask hitLayerMask; // 레이저가 충돌할 레이어 마스크
    [SerializeField] private LayerMask damageLayerMask; // 레이저가 충돌할 레이어 마스크

    [SerializeField, Range(0f, 1f)] private float hitTiming = 0.5f; // 데미지 적용 타이밍 (0~1 사이)

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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxDistance, hitLayerMask);
        if (hit.collider != null)
            lineRenderer.SetPosition(1, hit.point);
        else
            lineRenderer.SetPosition(1, transform.position + transform.right * 100f);

        StartCoroutine(PlayAnimation());
        StartCoroutine(ActiveCollider());
    }

    IEnumerator ActiveCollider()
    {
        yield return new WaitForSeconds(duration * hitTiming);

        // BoxCast 실행
        float colliderWidth = lineRenderer.widthMultiplier;
        float colliderLength = Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * (colliderLength / 2);
        Vector2 size = new(colliderLength, colliderWidth);
        float angle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

        Collider2D[] colls = Physics2D.OverlapBoxAll(origin, size, angle, damageLayerMask);

        // 충돌 시 데미지 적용
        foreach (var coll in colls)
        {
            if (coll.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log("Laser Hit: " + coll.gameObject.name);
            }
        }

        yield return null;
    }

    IEnumerator PlayAnimation()
    {
        lineRenderer.enabled = true;

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            lineRenderer.widthMultiplier = laserWidthOverTime.Evaluate(time / duration);
            yield return null;
        }
        lineRenderer.enabled = false;

        Destroy(gameObject);
    }

}
