using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float castRadius = 0.4f;
    [SerializeField] private float reflectionRenderDistance = 1f;
    [SerializeField] private float offsetSpeed = 1f;

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderTrajectory(Vector2 start)
    {
        lineRenderer.positionCount = 0;
        Vector2 end = GetMousePos();
        Vector2 dir = (end - start).normalized;
        RaycastHit2D hit = Physics2D.CircleCast(start, castRadius, dir, 100f, obstacleLayerMask);
        AddLinePosition(start);

        if (hit.collider != null)
        {
            AddLinePosition(hit.point);

            Vector2 reflectDir = Vector2.Reflect(dir, hit.normal);
            AddLinePosition(hit.point + reflectDir * reflectionRenderDistance);
            Vector2 newStart = hit.point + reflectDir * 0.01f; // 약간 떨어진 위치에서 다시 시작
        }
        else
        {
            AddLinePosition(start + dir * 100f);
        }

        MoveOffset();
    }

    private void AddLinePosition(Vector2 position)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    private Vector2 GetMousePos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void MoveOffset()
    {
        lineRenderer.material.mainTextureOffset += new Vector2(Time.deltaTime * offsetSpeed, 0f);
    }


}
