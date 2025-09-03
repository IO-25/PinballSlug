using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] float placementOffset;
    const float laserLength = 100;
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        lineRenderer.SetPosition(0, Vector3.left * placementOffset);
        lineRenderer.SetPosition(1, Vector3.left  * (laserLength + placementOffset));
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
