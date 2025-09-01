using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifetime = 5f;
    public GameObject hitEffect;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetDirection(transform.right); // 초기 방향 설정
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        // 적과 충돌하면 데미지
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
    private void OnDrawGizmos()
    {
        if (rb == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(rb.velocity.normalized * 0.5f));
    }

}
