using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] float placementOffset;
    const float laserLength = 20;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        transform.parent.GetComponent<Enemy>().OnDeadActions += OnDead;
        lineRenderer.SetPosition(0, Vector3.left * placementOffset + transform.parent.position);
        lineRenderer.SetPosition(1, Vector3.left  * (laserLength + placementOffset) + transform.parent.position);
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, Vector3.left * placementOffset + transform.parent.position);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.parent.GetComponent<PlayerHealth>().TakeDamage(1);
    }

    public void OnDead()
    {
        transform.parent.GetComponent<Enemy>().OnDeadActions -= OnDead;
        Destroy(this.gameObject);
    }
}
