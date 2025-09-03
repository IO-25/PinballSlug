using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] float placementOffset;
    const float laserLength = 100;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        lineRenderer.SetPosition(0, Vector3.left * placementOffset + transform.parent.localPosition);
        lineRenderer.SetPosition(1, Vector3.left  * (laserLength + placementOffset) + transform.parent.localPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            //Kill Player
            Debug.Log("Kill Player");
        }
    }

}
