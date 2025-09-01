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
    [SerializeField] private float groundCheckDistance = 0.4f; // �ٴ� üũ �Ÿ�

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
        // �¿� �̵�
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
            // �پ����
            if (Input.GetKey(KeyCode.S))
            {
                oneWayPlatform.Drop();
            }
            // ����
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        // === �߷� ���� ===
        if (rb.velocity.y < 0) // �ϰ� ��
            rb.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // ��� ���ε� ��ư�� �� ���
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
