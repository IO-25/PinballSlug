using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float accuracy = 1f;
    [SerializeField] private float attackDamage = 1;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private GameObject bulletPrefab;

    [Header("ź ���� ����")]
    [SerializeField] private GameObject spreadCircle; // ���� ���� �ð�ȭ�� ������Ʈ
    [SerializeField] private float baseSpread = 0f;    // �ּ� ���� ����(�� ����)
    [SerializeField] private float maxSpread = 15f;    // �ִ� ���� ����
    [SerializeField] private float maxDistance = 10f;  // �ִ� �Ÿ�(�� �̻��̸� maxSpread ����)

    private float nextAttackTime = 0f;

    private void Update()
    {
        // ź ���� �ð�ȭ
        if (spreadCircle == null) return;

        Vector2 start = firePoint.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(start, end);

        // �Ÿ� ���� ���
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

        // �Ÿ� ���� ���
        float t = Mathf.Clamp01(distance / maxDistance);
        float spread = Mathf.Lerp(baseSpread, maxSpread, t);

        // ���� ���� �߰�
        float randomAngle = Random.Range(-spread, spread);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + randomAngle;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));

        Instantiate(bulletPrefab, firePoint.position, rot);
    }

}
