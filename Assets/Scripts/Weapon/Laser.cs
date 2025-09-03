using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; // ������ �ð�ȭ�� ���� LineRenderer
    [SerializeField] private AnimationCurve laserWidthOverTime; // ������ ���� ��ȭ �
    [SerializeField] private float widthMultiplier = 2f; // ������ ũ�� ����

    /*
    [SerializeField] private float expandDuration = 0.3f; // �ּ� ���⿡�� �ִ� ����� ��ȭ�ϴ� �ð�
    [SerializeField] private float holdMaxDuration = 0.9f; // �ִ� ���� ���� �ð�
    [SerializeField] private float shrinkDuration = 0.3f; // �ִ� ���⿡�� �ּ� ����� ��ȭ�ϴ� �ð�
    [SerializeField] private float minLaserWidth = 0.4f; // ������ ���� ����
    [SerializeField] private float maxLaserWidth = 2.5f; // ������ ���� ����
    */

    [SerializeField] private int damage = 5; // ���� ������
    [SerializeField] private float damageInterval = 0.1f; // ������ ���� ����
    [SerializeField] private float maxDistance = 100f; // �ִ� ��Ÿ�
    [SerializeField] private LayerMask hitBlockerMask; // �������� �浹�� ���̾� ����ũ
    [SerializeField] private LayerMask damageTargetMask; // �������� �浹�� ���̾� ����ũ
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

        // BoxCast ����
        float colliderWidth = lineRenderer.widthMultiplier;
        float colliderLength = Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * (colliderLength / 2);
        Vector2 size = new(colliderLength, colliderWidth);
        float angle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

        Collider2D[] colls = Physics2D.OverlapBoxAll(origin, size, angle, damageTargetMask);

        // �浹 �� ������ ����
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
        // ���� ����
        while (time < expandDuration)
        {
            time += Time.deltaTime;
            lineRenderer.widthMultiplier = Mathf.Lerp(minLaserWidth, maxLaserWidth, time / expandDuration) * widthMultiplier;
            yield return null;
        }

        // �ִ� ���� ����
        yield return new WaitForSeconds(holdMaxDuration);

        // ���� ����
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
