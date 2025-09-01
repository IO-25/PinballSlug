using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("점프 관련")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 5f;    // 내려갈 때 가속도
    [SerializeField] private float lowJumpMultiplier = 4f;   // 점프 버튼 빨리 뗄 때 가속도
    [SerializeField] private Transform groundCheck; // 바닥 체크용 트랜스폼
    [SerializeField] private LayerMask groundLayerMask; // 바닥 레이어 마스크
    [SerializeField] private float groundCheckDistance = 0.4f; // 바닥 체크 거리

    private Rigidbody2D rb;
    private PlayerOneWayPlatform oneWayPlatform;
    private PlayerAnimationController animationController;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oneWayPlatform = GetComponent<PlayerOneWayPlatform>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        // 좌우 이동
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        animationController.SetBool_Upper("IsMoving", moveX != 0);
        animationController.SetBool_Lower("IsMoving", moveX != 0);
        
        animationController.SetAnimSpeed(moveX >= 0 ? 1f : -1f);

        Debug.Log(IsGrounded());
        animationController.SetBool_Lower("IsJumping", !IsGrounded());
        animationController.SetBool_Upper("IsJumping", !IsGrounded());


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // 뛰어내리기
            if (Input.GetKey(KeyCode.S))
            {
                oneWayPlatform.Drop();
            }
            // 점프
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        // === 중력 보정 ===
        if (rb.velocity.y < 0) // 하강 중
            rb.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // 상승 중인데 버튼을 뗀 경우
            rb.velocity += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * Vector2.up;
    }

    private bool IsGrounded()
        => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
