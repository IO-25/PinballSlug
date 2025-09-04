using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 20;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private bool useDestroyOnHit = false;
    [SerializeField] private float projectileLiveDuration = 20.0f;
    private Rigidbody2D rb;
    private Coroutine autoReturnCoroutine;

    private void Awake() 
        => rb = GetComponent<Rigidbody2D>();

    private void OnEnable()
    {
        SetDirection(transform.right); // 초기 방향 설정

        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);
        autoReturnCoroutine = StartCoroutine(AutoReturn());
    }

    IEnumerator AutoReturn()
    {
        yield return new WaitForSeconds(projectileLiveDuration);
        Return();
    }

    private void Return()
        => ObjectPoolingManager.Instance.Return(gameObject);

    public void TakeDamage(int damage) => Return();

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitEffect != null)
            ObjectPoolingManager.Instance.Get(hitEffect, transform.position, Quaternion.identity);

        // 타겟이 아니면 종료
        if (((1 << collision.gameObject.layer) & targetLayerMask) == 0) return;

        IDamageable damageable = collision.collider.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            if (useDestroyOnHit) Return();
        }
    }


    private void OnDrawGizmos()
    {
        if (rb == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(rb.velocity.normalized * 0.5f));
    }

}
