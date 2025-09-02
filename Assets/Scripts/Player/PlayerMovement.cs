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
    [SerializeField] private Vector2 groundCheckSize = new(0.5f, 0.4f); // 바닥 체크 거리

    private bool isSitting = false;
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
        HandleMovement();
        HandleJumpAndSit();
        ApplyGravityModifiers();
    }

    private void HandleMovement()
    {
        if (isSitting) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        bool isMoving = moveX != 0;
        animationController.SetBool("IsMoving", isMoving);
        animationController.SetAnimSpeed(moveX >= 0 ? 1f : -1f);
    }

    private void HandleJumpAndSit()
    {
        bool isSittingInput = Input.GetKey(KeyCode.S);
        bool isJumpingInput = Input.GetButton("Jump");

        if (IsGrounded())
        {
            isSitting = isSittingInput;

            if (isJumpingInput)
            {
                if (isSittingInput)
                    oneWayPlatform.Drop();
                else
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if(isSittingInput)
                animationController.SetBool_Upper("IsShooting", false);

        }

        animationController.SetBool("IsDropping", oneWayPlatform.IsDropping);

        // 애니메이션 상태 업데이트
        if (!oneWayPlatform.IsDropping)
        {
            animationController.SetBool("IsJumping", !IsGrounded());
            animationController.SetBool("IsSitting", isSittingInput);
        }
    }

    private void ApplyGravityModifiers()
    {
        if (rb.velocity.y < 0) // 하강 중
            rb.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // 상승 중인데 버튼을 뗀 경우
            rb.velocity += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * Vector2.up;
    }

    private bool IsGrounded()
        => Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayerMask);

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
