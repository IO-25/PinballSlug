using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�̵� ����")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("���� ����")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 5f;    // ������ �� ���ӵ�
    [SerializeField] private float lowJumpMultiplier = 4f;   // ���� ��ư ���� �� �� ���ӵ�
    [SerializeField] private Transform groundCheck; // �ٴ� üũ�� Ʈ������
    [SerializeField] private LayerMask groundLayerMask; // �ٴ� ���̾� ����ũ
    [SerializeField] private Vector2 groundCheckSize = new(0.5f, 0.4f); // �ٴ� üũ �Ÿ�

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

        // �ִϸ��̼� ���� ������Ʈ
        if (!oneWayPlatform.IsDropping)
        {
            animationController.SetBool("IsJumping", !IsGrounded());
            animationController.SetBool("IsSitting", isSittingInput);
        }
    }

    private void ApplyGravityModifiers()
    {
        if (rb.velocity.y < 0) // �ϰ� ��
            rb.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // ��� ���ε� ��ư�� �� ���
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
