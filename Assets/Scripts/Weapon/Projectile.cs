using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 20;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private bool useDestroyOnHit = false;
    private Rigidbody2D rb;

    private void Awake() 
        => rb = GetComponent<Rigidbody2D>();

    private void Start() 
        => SetDirection(transform.right); // 초기 방향 설정

    private void OnBecameInvisible() 
        => Destroy(gameObject);

    public void TakeDamage(int damage) 
        => Destroy(gameObject);

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

        // 타겟이 아니면 종료
        if (((1 << collision.gameObject.layer) & targetLayerMask) == 0) return;

        IDamageable damageable = collision.collider.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            if (useDestroyOnHit)
                Destroy(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        if (rb == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(rb.velocity.normalized * 0.5f));
    }

}
