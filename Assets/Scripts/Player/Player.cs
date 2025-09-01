using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("이동 관련")]
    public float moveSpeed = 5f;

    [Header("점프 관련")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;    // 내려갈 때 가속도
    public float lowJumpMultiplier = 2f;   // 점프 버튼 빨리 뗄 때 가속도
    public Transform groundCheck; // 바닥 체크용 트랜스폼
    public LayerMask groundLayerMask; // 바닥 레이어 마스크

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 좌우 이동
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // 점프 시작
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // === 중력 보정 ===
        if (rb.velocity.y < 0) // 하강 중
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // 상승 중인데 버튼을 뗀 경우
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayerMask);
    }
}
