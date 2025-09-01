using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float reflectionRenderDistance = 1f;

    // 1. Ray �߻�
    // 2. �浹 ����
    // 3. �ݻ� ���� ���
    // 4. LineRenderer�� �� �߰�


    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RenderTrajectory();
    }

    public void RenderTrajectory()
    {
        lineRenderer.positionCount = 0;
        Vector2 start = transform.position;
        Vector2 end = GetMousePos();
        Vector2 dir = (end - start).normalized;
        RaycastHit2D hit = Physics2D.Raycast(start, dir, 100f, obstacleLayerMask);
        AddLinePosition(start);

        if (hit.collider != null)
        {
            AddLinePosition(hit.point);

            Vector2 reflectDir = Vector2.Reflect(dir, hit.normal);
            AddLinePosition(hit.point + reflectDir * reflectionRenderDistance);
            Vector2 newStart = hit.point + reflectDir * 0.01f; // �ణ ������ ��ġ���� �ٽ� ����
        }
        else
        {
            AddLinePosition(start + dir * 100f);
        }
    }

    private void AddLinePosition(Vector2 position)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    private Vector2 GetMousePos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);


}
