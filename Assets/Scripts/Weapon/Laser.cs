using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; // ������ �ð�ȭ�� ���� LineRenderer
    [SerializeField] private AnimationCurve laserWidthOverTime; // ������ ���� ��ȭ �
    [SerializeField] private int damage = 5; // ���� ������
    [SerializeField] private float maxDistance = 100f; // �ִ� ��Ÿ�
    [SerializeField] private float duration = 1f; // ������ ���� �ð�
    [SerializeField] private LayerMask hitLayerMask; // �������� �浹�� ���̾� ����ũ
    [SerializeField] private LayerMask damageLayerMask; // �������� �浹�� ���̾� ����ũ

    [SerializeField, Range(0f, 1f)] private float hitTiming = 0.5f; // ������ ���� Ÿ�̹� (0~1 ����)

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

        // BoxCast ����
        float colliderWidth = lineRenderer.widthMultiplier;
        float colliderLength = Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * (colliderLength / 2);
        Vector2 size = new(colliderLength, colliderWidth);
        float angle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

        Collider2D[] colls = Physics2D.OverlapBoxAll(origin, size, angle, damageLayerMask);

        // �浹 �� ������ ����
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
