using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 관련")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("점프 관련")]
    [SerializeField] private float maxJumpTime = 0.5f; // 최대 점프 시간
    [SerializeField] private float jumpInitialVelocity = 10f; // 점프 초기 속도
    [SerializeField] private float jumpCancelVelocity = 2f; // 점프 중단 속도
    [SerializeField] private float fallMultiplier = 5f;    // 내려갈 때 가속도
    [SerializeField] private Transform groundCheck; // 바닥 체크용 트랜스폼
    [SerializeField] private LayerMask groundLayerMask; // 바닥 레이어 마스크
    [SerializeField] private Vector2 groundCheckBoxSize = new(1.2f, 0.4f); // 바닥 체크 거리

    private bool isJumping = false;
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
        bool isGrounded = IsGrounded();
        bool isSittingInput = Input.GetKey(KeyCode.S);
        bool isJumpingInput = Input.GetButton("Jump");

        if (isGrounded)
        {
            isSitting = isSittingInput;

            if (isJumpingInput)
            {
                if (isSittingInput)
                    oneWayPlatform.Drop();
                else
                    StartCoroutine(Jump());
            }

            if(isSittingInput)
                animationController.SetBool_Upper("IsShooting", false);

        }

        animationController.SetBool("IsDropping", oneWayPlatform.IsDropping);

        // 애니메이션 상태 업데이트
        if (!oneWayPlatform.IsDropping)
        {
            animationController.SetBool("IsJumping", !isGrounded);
            animationController.SetBool("IsSitting", isSittingInput && isGrounded);
        }
    }

    IEnumerator Jump()
    {
        if (isJumping) yield break;
        isJumping = true;

        float time = 0f;
        while (time < maxJumpTime && Input.GetButton("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpInitialVelocity);
            time += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector2(rb.velocity.x, jumpCancelVelocity);
        isJumping = false;
    }


    private void ApplyGravityModifiers()
    {
        if (rb.velocity.y < 0) // 하강 중
            rb.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
    }

    private bool IsGrounded()
        => Physics2D.OverlapBox(groundCheck.position, groundCheckBoxSize, 0f, groundLayerMask);

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckBoxSize);
        }
    }
}
